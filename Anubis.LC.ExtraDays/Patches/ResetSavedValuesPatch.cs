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
            string currentSaveFile = GameNetworkManager.Instance.currentSaveFileName;
            ES3.Save<int>(ModStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS_SAVE_KEY, ModStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS, currentSaveFile);
        }
    }

}
