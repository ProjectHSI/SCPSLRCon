using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using Log = PluginAPI.Core.Log;
using SCPSLRCon.Core.Config;
using SCPSLRCon.Core;

namespace SCPSLRCon
{
    public class Plugin
    {
        [PluginConfig]
        public CoreConfig CoreConfig;

        [PluginConfig]
        public LogConfig LogConfig;

        [PluginConfig]
        public CommandConfig CommandConfig;

        [PluginEntryPoint("SCPSLRcon", "v1.0.0", "RCon for SCP:SL.", "[Project HSI]")]
        [PluginPriority(LoadPriority.Lowest)]
        public void SCPSLRCon()
        {
            Log.Info("Loading SCP:SL Rcon...", "SCP:SL Rcon");
            Log.Info($"Version: {PluginConstants.version}", "SCP:SL RCon");
        }
    }
}
