using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using Log = PluginAPI.Core.Log;
using Config = SCPSLRCon.Core.Config.CoreConfig;
using SCPSLRCon.Core;

namespace SCPSLRCon
{
    public class Plugin
    {
        [PluginConfig]
        public Config Config;

        [PluginEntryPoint("SCPSL RCon", "v1.0.0", "RCon for SCP:SL.", "[Project HSI]")]
        [PluginPriority(LoadPriority.Lowest)]
        public void SCPSLRCon()
        {
            Log.Info("Loading SCP:SL RCon...", "SCP:SL RCon");
            Log.Info($"Version: {PluginConstants.version}", "SCP:SL RCon");
        }
    }
}
