using Anubis.LC.ExtraDays;
using HarmonyLib;
using LethalAPI.TerminalCommands.Models;
using System.Collections.Generic;


/// <summary>
/// Patching Terminal
/// </summary>
[HarmonyPatch(typeof(Terminal))]
internal static class TerminalPatch
{
    [HarmonyPatch("QuitTerminal")]
    [HarmonyPostfix]
    private static void QuitTerminal(Terminal __instance)
    {
        CommandHandler.HandleCommandInput("help", __instance);
    }
}