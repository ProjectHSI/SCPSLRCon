using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SCPSLRCon.Core.Config
{
    public class LogConfig
    {
        [Description("Enable TCP packet logs. Depending on your client this may will cause alot of log when connected to an RCon client.")]
        public bool EnableTCPPacketLog { get; set; } = true;

        [Description("Enable TCP connection logs.")]
        public bool EnableTCPConnectionLog { get; set; } = true;

        [Description("Enable TCP authentication log.")]
        public bool EnableTCPAuthLog { get; set; } = true;

        [Description("Enable TCP command logs.")]
        public bool EnableTCPCommandLog { get; set; } = true;
    }
}
