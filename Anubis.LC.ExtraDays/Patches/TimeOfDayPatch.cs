﻿using HarmonyLib;
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
    }
}
