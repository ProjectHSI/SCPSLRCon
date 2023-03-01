using PluginAPI.Core.Attributes;
using SCPSLRCon.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Log = PluginAPI.Core.Log;
using Command = PluginAPI.Core.Server;
using RConPacket = SCPSLRCon.Server.RconImplementer.RconPacket;

namespace SCPSLRCon.Server
{
    internal class TCPServerSlave
    {
        [PluginConfig("LogConfig.yml")]
        public LogConfig LogConfig = new LogConfig();

        [PluginConfig("CoreConfig.yml")]
        public CoreConfig CoreConfig = new CoreConfig();

        public static void Slave(object Socket)
        {
            Socket GoodSocket;

            CoreConfig CoreConfig = new TCPServerSlave().CoreConfig;
            LogConfig LogConfig = new TCPServerSlave().LogConfig;

            try
            {
                GoodSocket = (Socket)Socket;
            }
            catch
            {
                throw new Exception("That wasn't a socket.");
            }

            bool Authenticated = false;

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[] {};
                    GoodSocket.Receive(buffer);

                    RConPacket GoodClientPacket = new RConPacket(buffer);

                    if (LogConfig.EnableTcpPacketLog)
                    {
                        Log.Debug($"Id: {GoodClientPacket.GetId()}", "SCP:SL RCon");
                        Log.Debug($"Type: {GoodClientPacket.GetType()}", "SCP:SL RCon");
                        if (GoodClientPacket.GetType() == RConPacket.RconPacketType.AUTH)
                        {
                            Log.Debug($"Body: [REDACTED]", "SCP:SL RCon");
                        }
                        else
                        {
                            Log.Debug($"Body: {GoodClientPacket.GetBody()}", "SCP:SL RCon");
                        }
                    }

                    RConPacket GoodServerPacket = new RConPacket(
                        0x0,
                        RConPacket.RconPacketType.EXECCOMMAND,
                        "An error occurred, please try again."
                    );

                    if (GoodClientPacket.GetType() == RConPacket.RconPacketType.AUTH)
                    {
                        // potential timing attack
                        // right now we don't care

                        if (GoodClientPacket.GetBody() == CoreConfig.RconPassword)
                        {
                            Authenticated = true;

                            GoodServerPacket = new RConPacket(
                                GoodClientPacket.GetId(),
                                RConPacket.RconPacketType.AUTH_RESPONSE,
                                ""
                            );
                        }
                        else
                        {
                            GoodServerPacket = new RConPacket(
                                0xFFFFFFFF,
                                RConPacket.RconPacketType.AUTH_RESPONSE,
                                ""
                            );
                        }
                    }
                    else if (GoodClientPacket.GetType() == RConPacket.RconPacketType.EXECCOMMAND)
                    {
                        if (Authenticated)
                        {
                            Command.RunCommand(GoodClientPacket.GetBody());

                            GoodServerPacket = new RConPacket(
                                0xFFFFFFFF,
                                RConPacket.RconPacketType.AUTH_RESPONSE,
                                ""
                            );
                        }
                    }
                    else
                    {
                        GoodSocket.Close();
                        throw new Exception("MALFORMED TYPE!?!?!?!?!?!");
                    }

                    if (LogConfig.EnableTcpPacketLog)
                    {
                        Log.Debug($"Id: {GoodClientPacket.GetId()}", "SCP:SL RCon");
                        Log.Debug($"Type: {GoodClientPacket.GetType()}", "SCP:SL RCon");
                        Log.Debug($"Body: {GoodClientPacket.GetBody()}", "SCP:SL RCon");
                    }

                    GoodSocket.Send(GoodServerPacket.GetBuffer());

                }
                catch (SocketException e)
                {
                    if (e.ErrorCode == 995)
                    {
                        Log.Info("Connection was closed by remote client.", "SCP:SL RCon");
                    }
                }
                catch
                {
                    Log.Error("Something happened within the socket that caused an invalid packet to be sent.", "SCP:SL RCon");
                    Log.Error("This is usually because someone tried to log in with an invalid client (Like a browser).", "SCP:SL RCon");
                    Log.Error("This is a non-critical but check you're logging on using an RCon console.", "SCP:SL RCon");
                    Log.Error("See here for a good one; https://sourceforge.net/projects/rconconsole/.", "SCP:SL RCon");

                    GoodSocket.Close();
                }
            }
        }
    }
}
