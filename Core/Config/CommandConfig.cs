using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPSLRCon.Core.Config
{
    public class CommandConfig
    {
        [Description("Adminstrative command. -- Get the number of active connections (Authenticated/Connected - Both numbers should be the same.).")]
        public bool EnableTcpConnectionsCommand { get; set; } = true;
    }
}
