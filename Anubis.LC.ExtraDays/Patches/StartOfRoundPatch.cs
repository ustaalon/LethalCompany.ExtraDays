using HarmonyLib;
namespace Anubis.LC.ExtraDays.Patches
{
    /// <summary>
    /// Patching TimeOfDay
    /// </summary>
    [HarmonyPatch(typeof(StartOfRound))]
    internal static class StartOfRoundPatch
    {
        [HarmonyPatch("ResetShip")]
        [HarmonyPostfix]
        public static void Postfix_ResetShip(StartOfRound __instance)
        {
            TimeOfDay.Instance.ResetDeadline(true);
        }
    }
}
