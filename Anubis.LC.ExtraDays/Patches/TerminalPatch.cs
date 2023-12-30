using Anubis.LC.ExtraDays;
using HarmonyLib;
using LethalAPI.TerminalCommands.Models;

/// <summary>
/// Patching Terminal
/// </summary>
[HarmonyPatch(typeof(Terminal))]
internal static class TerminalPatch
{
    [HarmonyPatch("BeginUsingTerminal")]
    [HarmonyPostfix]
    private static void BeginUsingTerminal(Terminal __instance)
    {
        if (StartOfRound.Instance.companyBuyingRate <= 0f)
        {
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Company buying rate is lower than zero, recalculating...");
            TimeOfDay.Instance.ReCalculateBuyingRateForCompany();
        }
    }

    [HarmonyPatch("QuitTerminal")]
    [HarmonyPostfix]
    private static void QuitTerminal(Terminal __instance)
    {
        CommandHandler.HandleCommandInput("help", __instance);
    }
}