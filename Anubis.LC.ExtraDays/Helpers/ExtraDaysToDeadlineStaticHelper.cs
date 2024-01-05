using BepInEx.Bootstrap;
using BepInEx.Logging;

namespace Anubis.LC.ExtraDays.Helpers
{
    public static class ExtraDaysToDeadlineStaticHelper
    {
        public const string modGUID = "ExtraDaysToDeadline";
        public const string modName = "ExtraDaysToDeadline";
        public const string modVersion = "2.0.6";

        public static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(modGUID);

        public readonly static int DAYS_TO_INCREASE = 1;
        public readonly static int DEFAULT_AMOUNT_OF_DEADLINE_DAYS = 3;
        public readonly static int CONSTANT_PRICE = 350;

        public static bool IsDynamicDeadlinesModInstalled()
        {
            var DYNAMIC_DEADLINE_MOD = "Haha.DynamicDeadline";
            if (Chainloader.PluginInfos.TryGetValue(DYNAMIC_DEADLINE_MOD, out BepInEx.PluginInfo dynamicDeadlinesMod))
            {
                Logger.LogInfo($"Mod ${DYNAMIC_DEADLINE_MOD} is loaded alongside {modGUID}");
                return true;
            }
            Logger.LogInfo($"Mod ${DYNAMIC_DEADLINE_MOD} is not loaded alongside {modGUID}");
            return false;
        }
    }
}
