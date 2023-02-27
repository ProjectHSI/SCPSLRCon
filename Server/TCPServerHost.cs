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

namespace SCPSLRCon.Server
{
    internal class TCPServerHost
    {
        public CoreConfig CoreConfig;

        public Thread CreateServer()
        {
            Thread currentThread = new Thread(Server)
            {
                IsBackground = true
            };
            currentThread.Start(CoreConfig.TcpServerPort);

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

        /*
         * THIS WILL NOT DESTROY CONNECTIONS!!! 
         * This only destroys the receiver.
        */
        public void DestroyServer(Thread thread) {
            thread.Abort();
        }
    }
}
