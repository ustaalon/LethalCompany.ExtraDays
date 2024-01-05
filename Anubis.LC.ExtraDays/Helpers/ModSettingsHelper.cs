using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Anubis.LC.ExtraDays.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace Anubis.LC.ExtraDays.Helpers
{
    public static class ModSettingsHelper
    {
        public static readonly string CurrentModSettingsFile = Application.persistentDataPath + $"/{ExtraDaysToDeadlineStaticHelper.modGUID}_ModSettings.json";

        public static ModSettings GetDefaults()
        {
            return new ModSettings()
            {
                IsCorrelatedCalculation = true,
            };
        }

        public static void WriteSettings(bool isCorrelatedCalculation)
        {
            try
            {
                ModSettings settings = GetDefaults();
                settings.IsCorrelatedCalculation = isCorrelatedCalculation;
                string jsonString = JsonConvert.SerializeObject(settings, Formatting.Indented);

                File.WriteAllText(CurrentModSettingsFile, jsonString);
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Mod settings file created/updated");
            }
            catch
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogError("Could not update mod settings file for currrent game");
            }
        }

        public static ModSettings ReadSettings()
        {
            try
            {
                string jsonString = File.ReadAllText(CurrentModSettingsFile);

                ModSettings settings = JsonConvert.DeserializeObject<ModSettings>(jsonString);

                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Mod settings file loaded");
                return settings;
            }
            catch
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogError("Could not read mod settings file. Using defaults");
                return GetDefaults();
            }
        }

        public static bool IsSaveFileExists()
        {
            return File.Exists(CurrentModSettingsFile);
        }

        public static void DeleteSettings()
        {
            if (!IsSaveFileExists())
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Mod settings file does not exists. Nothing to delete");
                return;
            }

            try
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Mod settings file has been deleted");
                File.Delete(CurrentModSettingsFile);
            }
            catch
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogError("Could not delete mod settings file");
            }
        }
    }
}
