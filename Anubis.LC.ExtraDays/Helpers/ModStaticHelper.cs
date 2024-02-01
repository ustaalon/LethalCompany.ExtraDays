using BepInEx.Bootstrap;
using BepInEx.Logging;

namespace Anubis.LC.ExtraDays.Helpers
{
    public class ModStaticHelper
    {
        public const string modGUID = "ExtraDaysToDeadline";
        public const string modName = "ExtraDaysToDeadline";
        public const string modVersion = "2.2.1";

        public static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource(modGUID);

        public readonly static int DAYS_TO_INCREASE = 1;
        public readonly static int DEFAULT_AMOUNT_OF_DEADLINE_DAYS = 3;
        public readonly static string DEFAULT_AMOUNT_OF_DEADLINE_DAYS_SAVE_KEY = $"{modGUID}_deadlineDaysAmount";
        public readonly static int CONSTANT_PRICE = 350;

        public static bool IsThisModInstalled(string mod)
        {
            if (Chainloader.PluginInfos.TryGetValue(mod, out BepInEx.PluginInfo modInfo))
            {
                Logger.LogInfo($"Mod ${mod} is loaded alongside {modGUID}");
                return true;
            }
            return false;
        }
    }
}
