using Anubis.LC.ExtraDays.Helpers;
using Anubis.LC.ExtraDays.Models;
using UnityEngine;

namespace Anubis.LC.ExtraDays.Extensions
{
    public static class TimeOfDayExtensions
    {
        private static int extraDayPrice = 0;

        public static void AddXDaysToDeadline(this TimeOfDay timeOfDay, float days = 1f)
        {
            timeOfDay.timeUntilDeadline += timeOfDay.totalTime * days;
            timeOfDay.ReCalculateBuyingRateForCompany();
            timeOfDay.SyncTimeAndDeadline();

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Added {ExtraDaysToDeadlineStaticHelper.DAYS_TO_INCREASE} day to deadline.");
        }

        public static int CalculateDeadlineDaysAmount(this TimeOfDay timeOfDay)
        {
            int days = (int)Mathf.Floor(timeOfDay.timeUntilDeadline / timeOfDay.totalTime);

            // if amount of days is below the default, keep it as default amount of days
            if (days <= ExtraDaysToDeadlineStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS)
            {
                return ExtraDaysToDeadlineStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS;
            }

            return days;
        }

        public static void SetExtraDaysPrice(this TimeOfDay timeOfDay)
        {
            ModSettings settings = ModSettingsHelper.ReadSettings();
            if (settings.IsCorrelatedCalculation)
            {
                int profitQuota = timeOfDay.profitQuota;
                float baseIncrease = 0.15f * profitQuota;
                float minPrice = 0.5f * profitQuota;
                float randommizer = (timeOfDay.quotaVariables.randomizerCurve.Evaluate(UnityEngine.Random.Range(0f, 1f)) * timeOfDay.quotaVariables.randomizerMultiplier + 1f);
                float priceXRandom = baseIncrease * randommizer;

                int price = (int)Mathf.Clamp(priceXRandom, minPrice, 1E+09f);

                extraDayPrice = price;
            }

            extraDayPrice = ExtraDaysToDeadlineStaticHelper.CONSTANT_PRICE;
        }

        public static int GetExtraDaysPrice(this TimeOfDay timeOfDay)
        {
            return extraDayPrice;
        }

        public static void ResetDeadline(this TimeOfDay timeOfDay, bool isShipReset = false)
        {
            if (isShipReset || !ExtraDaysToDeadlineStaticHelper.IsDynamicDeadlinesModInstalled())
            {
                timeOfDay.timeUntilDeadline = timeOfDay.totalTime * ExtraDaysToDeadlineStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS;

                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Deadline reset to defaults");
            }

            timeOfDay.ReCalculateBuyingRateForCompany();
            timeOfDay.SyncTimeAndDeadline();
        }

        public static void ReCalculateBuyingRateForCompany(this TimeOfDay timeOfDay, bool tryGetFromDisk = false)
        {
            timeOfDay.SetDeadlineDaysAmount(tryGetFromDisk);
            timeOfDay.SetBuyingRateForDay();
            StartOfRound.Instance.SyncCompanyBuyingRateServerRpc();
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Buying rate recalculated");
        }

        public static void SetDeadlineDaysAmount(this TimeOfDay timeOfDay, bool tryGetFromDisk = false)
        {
            var deadlineDaysAmount = !tryGetFromDisk ? timeOfDay.CalculateDeadlineDaysAmount() : timeOfDay.GetDeadlineDaysAmountFromDisk();

            timeOfDay.quotaVariables.deadlineDaysAmount = deadlineDaysAmount;

            if (SaveHelper.IsHost())
            {
                SaveHelper.WriteSettings(new Settings()
                {
                    DeadlineDaysAmount = deadlineDaysAmount
                });
            }

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Deadline days amount: {deadlineDaysAmount} (To calculate buying rate)");
        }

        public static int GetDeadlineDaysAmountFromDisk(this TimeOfDay timeOfDay)
        {
            if (!SaveHelper.IsHost()) return timeOfDay.CalculateDeadlineDaysAmount();

            int deadlineDaysAmount;
            if (SaveHelper.IsSaveFileExists())
            {
                deadlineDaysAmount = SaveHelper.ReadSettings().DeadlineDaysAmount;
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Loaded deadlineDaysAmount from disk, deadlineDaysAmount: {deadlineDaysAmount}");
            }
            else
            {
                SaveHelper.WriteSettings(new Settings()
                {
                    DeadlineDaysAmount = timeOfDay.CalculateDeadlineDaysAmount()
                });
                deadlineDaysAmount = SaveHelper.ReadSettings().DeadlineDaysAmount;
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Saved deadlineDaysAmount to disk and then loaded, deadlineDaysAmount: {deadlineDaysAmount}");
            }

            return deadlineDaysAmount;
        }

        public static void SyncTimeAndDeadline(this TimeOfDay timeOfDay)
        {
            timeOfDay.UpdateProfitQuotaCurrentTime();
            timeOfDay.SyncTimeClientRpc(timeOfDay.globalTime, (int)timeOfDay.timeUntilDeadline);

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Deadline sync");
        }
    }
}
