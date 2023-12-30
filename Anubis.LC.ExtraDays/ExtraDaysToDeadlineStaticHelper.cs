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
        public readonly static string DYNAMIC_DEADLINE_MOD = "Haha.DynamicDeadline";
        public readonly static int DAYS_TO_INCREASE = 1;
        public readonly static int DEFAULT_AMOUNT_OF_DEADLINE_DAYS = 3;

        public static bool IsDynamicDeadlinesModInstalled()
        {
            if (Chainloader.PluginInfos.TryGetValue(DYNAMIC_DEADLINE_MOD, out BepInEx.PluginInfo dynamicDeadlinesMod))
            {
                Logger.LogInfo($"Mod ${DYNAMIC_DEADLINE_MOD} is loaded alongside {PluginInfo.PLUGIN_GUID}");
                return true;
            }
            Logger.LogInfo($"Mod ${DYNAMIC_DEADLINE_MOD} is not loaded alongside {PluginInfo.PLUGIN_GUID}");
            return false;
        }
    }
}
