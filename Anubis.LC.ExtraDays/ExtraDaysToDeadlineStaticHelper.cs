using BepInEx.Bootstrap;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Anubis.LC.ExtraDays
{
    public static class ExtraDaysToDeadlineStaticHelper
    {
        public static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_GUID);
        public readonly static string DynamicDeadline = "Haha.DynamicDeadline";

        public static bool IsDynamicDeadlinesModInstalled()
        {
            if (Chainloader.PluginInfos.TryGetValue(DynamicDeadline, out BepInEx.PluginInfo dynamicDeadlinesMod))
            {
                Logger.LogInfo($"Mod ${DynamicDeadline} is loaded alongside {PluginInfo.PLUGIN_GUID}");
                return true;
            }
            Logger.LogInfo($"Mod ${DynamicDeadline} is not loaded alongside {PluginInfo.PLUGIN_GUID}");
            return false;
        }
    }
}
