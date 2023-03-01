using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SCPSLRCon.Core.Config;
using Log = PluginAPI.Core.Log;

namespace SCPSLRCon.Server.RconImplementer
{

    internal class RconPacket
    {
        private bool isFrozen = false;
        private uint Id;
        private RconPacketType Type;
        private string Body;

        public LogConfig LogConfig;

        private string overflowString = "The size of the message that the server was going to send is over 4096 bytes. Please check LocalAdmin's output to see the output.";

        public enum RconPacketType
        {
            AUTH = 3,
            EXECCOMMAND = 2,
            AUTH_RESPONSE = 2,
            RESPOSNE_VALUE = 0
        }

        public static Exception MalformedPacketException = new Exception("The RCon packet was invalid, this probably isn't due to the application failing but an RCon client not working. Server should force disconnection to the client *immediately*.");

        public RconPacket(uint id, RconPacketType type, string body)
        {
            Id = id;
            Type = type;
            Body = body;
        }

        public RconPacket(byte[] packet)
        {
            Id = BitConverter.ToUInt32(packet.Skip(3).Take(3).ToArray(), 0);

            uint TempType = BitConverter.ToUInt32(packet.Take(7).Take(3).ToArray(), 0);

            if (TempType == RconPacketType.AUTH.GetHashCode())
            {
                Type = RconPacketType.AUTH;
            }
            else if (TempType == RconPacketType.EXECCOMMAND.GetHashCode())
            {
                Type = RconPacketType.EXECCOMMAND;
            }
            else
            {
                throw new Exception("The Rcon packet was already frozen. (Please report to this plugin's repo!)");
            }

            byte[] body = packet.Take(11).ToArray();
            Body = BitConverter.ToString(body.Take(body.Length - 1).ToArray(), 0);
            Freeze();
        }

        public void Freeze()
        {
            if (isFrozen == false)
            {
                isFrozen = true;
            }
            else
            {
                throw new Exception("The Rcon packet was already frozen. (Please report to this plugin's repo!)");
            }
        }

        // 10 bytes is a constant, see https://developer.valvesoftware.com/wiki/Source_RCON_Protocol.
        public uint CalculateSize() { return (uint)Body.Length + 10; }

        public byte[] GetBuffer()
        {
            byte[] buffer;

            // --- SIZE ENCODING ---

            uint Size;

            if (LogConfig.EnableTcpPacketLog)
            {
                Log.Debug($"Id: {Id}", "SCP:SL RCon");
                Log.Debug($"Type: {Type}", "SCP:SL RCon");
            }

            try
            {
                Size = System.Convert.ToUInt32(CalculateSize());
                if (Size > 4096) { throw new OverflowException("The packet being sent was over 4096 bytes -- Server will send a default string."); }

                if (LogConfig.EnableTcpPacketLog)
                {
                    Log.Debug($"Body: {Body}", "SCP:SL RCon");
                }
            }
            catch
            {
                Log.Error("An error occurred; The message that was supposed to be transmitted is too large to fit in a 4096 byte string.");
                Log.Error("Using an override string.");

                SetBody(overflowString);

                if (LogConfig.EnableTcpPacketLog)
                {
                    Log.Debug($"The string the command responded with was too large to fit in RCon packet body.\nThe current fallback string is {overflowString}", "SCP:SL RCon");
                }

                Size = System.Convert.ToUInt32(CalculateSize());
                if (Size > 4096) { throw new OverflowException("Dead X_X"); }
            }

            buffer = System.BitConverter.GetBytes(Size);

            // -- ID ENCODING ---
            // error handling is for babies (it's already sent as a 32-bit unsigned int so if it's not we're hecked)

            BitConverter.GetBytes(Id).CopyTo(buffer, buffer.Length);

            // -- TYPE ENCODING ---
            // error handling is for babies (literally it has to be 3, 2 or 0.)

            BitConverter.GetBytes((uint)Type).CopyTo(buffer, buffer.Length);

            // -- BODY ENCODING ---

            Encoding.GetEncoding("UTF-8").GetBytes($"{Body}\0\0".ToCharArray()).CopyTo(buffer, buffer.Length);

            return buffer;
        }

        // CAUTION: Boilerplate code below this line.

        public void CheckIfFrozen()
        {
            if (isFrozen)
            {
                throw new Exception("The Rcon packet is frozen. Possible confusion between outgoing and incoming packets. (Please report to this plugin's repo!)");
            }
        }

        public void SetType(RconPacketType type)
        {
            CheckIfFrozen();
            Type = type;
        }

        public void SetId(uint id)
        {
            CheckIfFrozen();
            Id = id;
        }

        public void SetBody(string body)
        {
            CheckIfFrozen();
            Body = body;
        }

        public RconPacketType GetType() { return Type; }

        public uint GetId() { return Id; }

        public string GetBody() { return Body; }
    }
}
