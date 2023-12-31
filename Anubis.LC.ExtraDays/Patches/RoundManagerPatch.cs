using Anubis.LC.ExtraDays.Helpers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anubis.LC.ExtraDays.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    public class RoundManagerPatch
    {
        public static int PreviousDaysUntilDeadline = TimeOfDay.Instance.daysUntilDeadline;
        public static bool DaysUntilDeadlineModuluRan = false;

        [HarmonyPatch("PlotOutEnemiesForNextHour")]
        [HarmonyPatch("AdvanceHourAndSpawnNewBatchOfEnemies")]
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

        [HarmonyPatch("PlotOutEnemiesForNextHour")]
        [HarmonyPatch("AdvanceHourAndSpawnNewBatchOfEnemies")]
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
