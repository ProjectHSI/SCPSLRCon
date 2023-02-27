using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCPSLRCon.Core.Config;

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

        [Description("These settings control which messages should be logged. These only affect the LocalAdmin view.")]
        public LogConfig LogConfig { get; set; } = new LogConfig();

        [Description("These settings control which commands are enabled, these don't affect the server and report simple diagnostic and connection infomation.")]
        public CommandConfig CommandConfig { get; set; } = new CommandConfig();
        
    }
}
