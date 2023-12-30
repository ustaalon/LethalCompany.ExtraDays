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
            SaveHelper.DeleteSettings();
        }
    }
}
