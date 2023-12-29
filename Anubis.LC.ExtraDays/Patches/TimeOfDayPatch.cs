using HarmonyLib;
namespace Anubis.LC.ExtraDays.Patches
{
    /// <summary>
    ///
    /// </summary>
    [HarmonyPatch(typeof(TimeOfDay))]
    internal static class TimeOfDayPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void NewInstance(TimeOfDay __instance)
        {
            PatchStaticHelper.TimeOfDay = __instance;
        }

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
