using System.IO;
using Anubis.LC.ExtraDays.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace Anubis.LC.ExtraDays.Helpers
{
    public class SaveGameHelper
    {
        public static readonly string CurrentGameSaveFile = Application.persistentDataPath + $"/{ExtraDaysToDeadlineStaticHelper.modGUID}_{GameNetworkManager.Instance.currentSaveFileName}.json";

        public int deadlineDaysAmount = GetDefaults().DeadlineDaysAmount;
        public bool isCorrelatedPriceCalculation = GetDefaults().IsCorrelatedPriceCalculation;

        public static Settings GetDefaults()
        {
            return new Settings()
            {
                DeadlineDaysAmount = ExtraDaysToDeadlineStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS,
                IsCorrelatedPriceCalculation = LethalConfigHelper.IsCorrelatedPriceCalculation.Value
            };
        }

        public SaveGameHelper SetDeadlineDaysAmount(int value)
        {
            deadlineDaysAmount = value;
            return this;
        }

        public SaveGameHelper SetIsCorrelatedPriceCalculation(bool value)
        {
            isCorrelatedPriceCalculation = value;
            return this;
        }

        public void Save()
        {
            WriteSettings(new Settings()
            {
                DeadlineDaysAmount = this.deadlineDaysAmount,
                IsCorrelatedPriceCalculation = this.isCorrelatedPriceCalculation
            });
        }

        public static void WriteSettings(Settings data)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);

                File.WriteAllText(CurrentGameSaveFile, jsonString);
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
                string jsonString = File.ReadAllText(CurrentGameSaveFile);

                Settings data = JsonConvert.DeserializeObject<Settings>(jsonString);

                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Settings file loaded");
                return data;
            }
            catch
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogError("Could not read settings file. Using defaults");
                return GetDefaults();
            }
        }

        public static bool IsSaveFileExists()
        {
            return File.Exists(CurrentGameSaveFile);
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
                File.Delete(CurrentGameSaveFile);
            }
            catch
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogError("Could not delete settings file");
            }
        }

        public static void ResetSettings()
        {
            WriteSettings(GetDefaults());
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Reset value in saved file for deadlineDaysAmount, deadlineDaysAmount: {ExtraDaysToDeadlineStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS}");
        }
    }
}
