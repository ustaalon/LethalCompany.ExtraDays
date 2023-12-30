using Anubis.LC.ExtraDays.Helpers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anubis.LC.ExtraDays.Patches
{
    [HarmonyPatch(typeof(GameNetworkManager))]
    public class ResetSavedValuesPatch
    {
        [HarmonyPatch("ResetSavedGameValues")]
        [HarmonyPostfix]
        public static void ResetSavedValues()
        {
            SaveHelper.ResetSettings();
        }
    }

}
