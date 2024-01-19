using Anubis.LC.ExtraDays.Helpers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anubis.LC.ExtraDays.Patches
{
    [HarmonyPatch(typeof(DeleteFileButton))]
    public class DeleteFileButtonPatch
    {
        [HarmonyPatch("DeleteFile")]
        [HarmonyPrefix]
        public static void DeleteFile()
        {
            if (LethalConfigHelper.GetConfigForSaveFile().TryGetValue("correlatedPrice", out var correlatedPrice))
            {
                correlatedPrice.Value = (bool)correlatedPrice.DefaultValue;
            }
            if (LethalConfigHelper.GetConfigForSaveFile().TryGetValue("buyingRate", out var buyingRate))
            {
                buyingRate.Value = (bool)buyingRate.DefaultValue;
            }
        }
    }
}
