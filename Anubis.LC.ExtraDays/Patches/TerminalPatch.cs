using HarmonyLib;
namespace Anubis.LC.ExtraDays.Patches
{
    /// <summary>
    ///
    /// </summary>
    [HarmonyPatch(typeof(Terminal))]
    internal static class TerminalPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void NewInstance(Terminal __instance)
        {
            PatchStaticHelper.Terminal = __instance;
        }
    }
}
