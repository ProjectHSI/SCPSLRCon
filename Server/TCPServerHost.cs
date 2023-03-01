using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SCPSLRCon.Server;
using SCPSLRCon.Core.Config;
using System.Net.Sockets;
using System.Net;
using Log = PluginAPI.Core.Log;
using PluginAPI.Core.Attributes;

namespace SCPSLRCon.Server
{
    internal class TCPServerHost
    {
        [PluginConfig("CoreConfig.yml")]
        public CoreConfig CoreConfig = new CoreConfig();

        public Thread CreateServer()
        {
            Log.Info("Starting RCon server...", "SCP:SL RCon");

#if DEBUG
            Log.Debug("Debugging!!!", "SCP:SL RCon");
#endif

#if DEBUG
            Log.Debug("Thread Create", "SCP:SL RCon");
#endif

            Thread currentThread = new Thread(Server)
            {
                IsBackground = true
            };

#if DEBUG
            Log.Debug("Thread Start", "SCP:SL RCon");
#endif

            currentThread.Start(CoreConfig.TcpServerPort);

            Log.Info($"Port: {CoreConfig.TcpServerPort}.", "SCP:SL RCon");

#if DEBUG
            Log.Debug("Return", "SCP:SL RCon");
#endif

            return currentThread;
        }

        private static void Server(object port)
        {
            int intPort;

            try
            {
                intPort = (int)port;
            }
            catch
            {
                throw new Exception("The TCP Server Port is not an integer, please restart the server after fixing the TCP Server Port in SCP:SL RCon's \"CoreConfig.yml\" file.");
            }

            TcpListener listener = new TcpListener(IPAddress.Any, intPort);

            while (true)
            {
                Socket currentSocket = listener.AcceptSocket();
            }
        }

        public TCPServerHost()
        {
#if DEBUG
            Log.Debug("TCPServerHost class created.", "SCP:SL RCon");
#endif
        }

        /*
         * THIS WILL NOT DESTROY CONNECTIONS!!! 
         * This only destroys the receiver.
        */
        public void DestroyServer(Thread thread) {
            thread.Abort();
        }
    }
}
