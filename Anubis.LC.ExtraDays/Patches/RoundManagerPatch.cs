using Anubis.LC.ExtraDays.Helpers;
using HarmonyLib;

namespace Anubis.LC.ExtraDays.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    public class RoundManagerPatch
    {
        public static int PreviousDaysUntilDeadline = TimeOfDay.Instance.daysUntilDeadline;
        public static bool DaysUntilDeadlineModuluRan = false;

        [HarmonyPatch("AdvanceHourAndSpawnNewBatchOfEnemies")]
        [HarmonyPatch("BeginEnemySpawning")]
        [HarmonyPrefix]
        public static void Prefix_PlotOutEnemiesForNextHour()
        {
            if (TimeOfDay.Instance.daysUntilDeadline > ExtraDaysToDeadlineStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS)
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Adjusting enemies in higer deadline - Start");
                PreviousDaysUntilDeadline = TimeOfDay.Instance.daysUntilDeadline;
                TimeOfDay.Instance.daysUntilDeadline = TimeOfDay.Instance.daysUntilDeadline % ExtraDaysToDeadlineStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS;
                DaysUntilDeadlineModuluRan = true;
            }
        }

        [HarmonyPatch("AdvanceHourAndSpawnNewBatchOfEnemies")]
        [HarmonyPatch("BeginEnemySpawning")]
        [HarmonyPostfix]
        public static void Postfix_PlotOutEnemiesForNextHour()
        {
            if (DaysUntilDeadlineModuluRan == true)
            {
                TimeOfDay.Instance.daysUntilDeadline = PreviousDaysUntilDeadline;
                DaysUntilDeadlineModuluRan = false;
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Adjusting enemies in higer deadline - End");
            }
        }
    }
}
