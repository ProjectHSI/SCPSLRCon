using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPSLRCon.Core.Config
{
    public sealed class CoreConfig
    {
        [Description("Does the RCon TCP server get enabled? (Basically if the plugin's enabled.)")]
        public bool EnableTcpServer { get; set; } = true;

        [Description("Port number of the TCP server. (Useful if running multiple servers.)")]
        public int TcpServerPort { get; set; } = 7778;

        [Description("RCon password, this is the *only* security mechanism the server has.")]
        public string RconPassword { get; set; } = "SuperSecretPassword";
        
    }
}
