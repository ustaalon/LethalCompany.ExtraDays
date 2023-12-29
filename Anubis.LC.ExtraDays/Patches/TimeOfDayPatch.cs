using HarmonyLib;
namespace Anubis.LC.ExtraDays.Patches
{
    /// <summary>
    /// Patching TimeOfDay
    /// </summary>
    [HarmonyPatch(typeof(TimeOfDay))]
    internal static class TimeOfDayPatch
    {
        [HarmonyPatch("SetNewProfitQuota")]
        [HarmonyPostfix]
        private static void SetNewProfitQuota(TimeOfDay __instance)
        {
            __instance.timeUntilDeadline = __instance.totalTime * 3f;
            __instance.quotaVariables.deadlineDaysAmount = 4;
            __instance.UpdateProfitQuotaCurrentTime();
        }
    }
}
