using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anubis.LC.ExtraDays.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace Anubis.LC.ExtraDays.Helpers
{
    public static class SaveHelper
    {
        public static readonly string filePath = Application.persistentDataPath + $"/{PluginInfo.PLUGIN_NAME}_{GameNetworkManager.Instance.currentSaveFileName}.json";

        public static void WriteSettings(Settings data)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);

                File.WriteAllText(filePath, jsonString);
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Settings file created/updated");
            }
            catch
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogError("Could not update settings file for currrent game");
            }
        }

        public static Settings ReadSettings()
        {
            try
            {
                string jsonString = File.ReadAllText(filePath);

                Settings data = JsonConvert.DeserializeObject<Settings>(jsonString);

                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Settings file loaded");
                return data;
            }
            catch
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogError("Could not read settings file. Using defaults");
                return new Settings()
                {
                    DeadlineDaysAmount = ExtraDaysToDeadlineStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS
                };
            }
        }

        public static bool IsSaveFileExists()
        {
            return File.Exists(filePath);
        }

        public static void DeleteSettings()
        {
            if (!IsSaveFileExists())
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Settings file does not exists. Nothing to delete");
                return;
            }

            try
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Settings file has been deleted");
                File.Delete(filePath);
            }
            catch
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogError("Could not delete settings file");
            }
        }

        public static void ResetSettings()
        {
            WriteSettings(new Settings()
            {
                DeadlineDaysAmount = ExtraDaysToDeadlineStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS
            });
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Reset value in saved file for deadlineDaysAmount, deadlineDaysAmount: {ExtraDaysToDeadlineStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS}");
        }
    }
}
