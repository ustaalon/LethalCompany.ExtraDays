using HarmonyLib;
using Unity.Netcode;
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
        public static void SetNewProfitQuota(TimeOfDay __instance)
        {
            __instance.ResetDeadline();
        }

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Start(TimeOfDay __instance)
        {
            __instance.ReCalculateBuyingRateForCompany(true);
        }
    }
}
