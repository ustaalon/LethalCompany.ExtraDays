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
            string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);

            File.WriteAllText(filePath, jsonString);
        }

        public static Settings ReadSettings()
        {
            string jsonString = File.ReadAllText(filePath);

            Settings data = JsonConvert.DeserializeObject<Settings>(jsonString);

            return data;
        }

        public static bool IsSaveFileExists() { return File.Exists(filePath); }

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
