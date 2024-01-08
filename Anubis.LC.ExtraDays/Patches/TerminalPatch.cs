using Anubis.LC.ExtraDays.Extensions;
using Anubis.LC.ExtraDays.Helpers;
using HarmonyLib;
using LethalAPI.LibTerminal.Models;

/// <summary>
/// Patching Terminal
/// </summary>
[HarmonyPatch(typeof(Terminal))]
internal static class TerminalPatch
{
    [HarmonyPatch("BeginUsingTerminal")]
    [HarmonyPostfix]
    public static void BeginUsingTerminal(Terminal __instance)
    {
        if (StartOfRound.Instance.companyBuyingRate <= 0f)
        {
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Company buying rate is lower than zero, recalculating...");
            TimeOfDay.Instance.ReCalculateBuyingRateForCompany();
        } else
        {
            TimeOfDay.Instance.ReCalculateBuyingRateForCompany(true);
        }
    }

    [HarmonyPatch("QuitTerminal")]
    [HarmonyPostfix]
    public static void QuitTerminal(Terminal __instance)
    {
        CommandHandler.HandleCommandInput("help", __instance);
    }
}