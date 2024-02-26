using Anubis.LC.ExtraDays.Helpers;
using Anubis.LC.ExtraDays.ModNetwork;
using BepInEx.Configuration;
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
            Networking.Instance.SyncTimeServerRpc();

            ModStaticHelper.Logger.LogInfo($"Added {ModStaticHelper.DAYS_TO_INCREASE} day to deadline.");
        }

        public static int CalculateDeadlineDaysAmount(this TimeOfDay timeOfDay)
        {
            int days = (int)Mathf.Floor(timeOfDay.timeUntilDeadline / timeOfDay.totalTime);

            // if amount of days is below the default, keep it as default amount of days
            if (days <= ModStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS)
            {
                return ModStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS;
            }

            return days;
        }

        public static void SetExtraDaysPrice(this TimeOfDay timeOfDay)
        {
            if (Networking.Instance.isCorrelatedPrice)
            {
                int profitQuota = timeOfDay.profitQuota;
                float baseIncrease = 0.15f * profitQuota;
                float minPrice = 0.5f * profitQuota;
                float randommizer = (timeOfDay.quotaVariables.randomizerCurve.Evaluate(UnityEngine.Random.Range(0f, 1f)) * timeOfDay.quotaVariables.randomizerMultiplier + 1f);
                float priceXRandom = baseIncrease * randommizer;

                int price = (int)Mathf.Clamp(priceXRandom, minPrice, 1E+09f);

                extraDayPrice = price;
            }
            else
            {
                if (LethalConfigHelper.GetConfigForSaveFile().TryGetValue("extraDayPrice", out var _extraDayPrice))
                {
                    extraDayPrice = ((ConfigEntry<int>)_extraDayPrice).Value;
                }
                else
                {
                    extraDayPrice = ModStaticHelper.CONSTANT_PRICE;
                }
            }
        }

        public static int GetExtraDaysPrice(this TimeOfDay timeOfDay)
        {
            return Networking.Instance.isCorrelatedPrice ? extraDayPrice : Networking.Instance.extraDaysPrice;
        }

        public static void ResetDeadline(this TimeOfDay timeOfDay, bool isShipReset = false)
        {
            if (isShipReset && ModStaticHelper.IsThisModInstalled("LethalExpansion"))
            {
                timeOfDay.timeUntilDeadline = timeOfDay.totalTime * TimeOfDay.Instance.quotaVariables.deadlineDaysAmount;

                ModStaticHelper.Logger.LogInfo("Deadline reset to defaults (Compatibility for Lethal Expansion)");
            }
            else if (isShipReset
                || !ModStaticHelper.IsThisModInstalled("Haha.DynamicDeadline")
                || !ModStaticHelper.IsThisModInstalled("BrutalCompany")
                || !ModStaticHelper.IsThisModInstalled("BrutalCompanyPlus")
                || !ModStaticHelper.IsThisModInstalled("LethalOrg.ProgressiveDeadline")
                || !ModStaticHelper.IsThisModInstalled("LethalExpansion"))
            {
                timeOfDay.timeUntilDeadline = timeOfDay.totalTime * ModStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS;

                ModStaticHelper.Logger.LogInfo("Deadline reset to defaults");
            }

            timeOfDay.ReCalculateBuyingRateForCompany();
            timeOfDay.SyncTimeAndDeadline();
        }

        public static void ReCalculateBuyingRateForCompany(this TimeOfDay timeOfDay, bool tryGetFromDisk = false)
        {
            timeOfDay.SetDeadlineDaysAmount(tryGetFromDisk);
            timeOfDay.SetBuyingRateForDay();
            timeOfDay.SetReduceBuyingRate();
            StartOfRound.Instance.SyncCompanyBuyingRateServerRpc();
            ModStaticHelper.Logger.LogInfo("Buying rate recalculated");
        }

        public static void SetReduceBuyingRate(this TimeOfDay timeOfDay)
        {
            if (Networking.Instance.isReduceBuyingRate)
            {
                ModStaticHelper.Logger.LogInfo("Reduced buying rate is ON");
                var reduceAmount = 0.01f * timeOfDay.quotaVariables.deadlineDaysAmount;
                var currentRate = StartOfRound.Instance.companyBuyingRate;
                var nextAmount = StartOfRound.Instance.companyBuyingRate - reduceAmount;

                ModStaticHelper.Logger.LogInfo($"Current rate: {currentRate}, Next rate: {nextAmount}");
                if (currentRate >= 0.9f && nextAmount > 0.7f)
                {
                    ModStaticHelper.Logger.LogInfo($"Buying rate set to: {nextAmount}");
                    StartOfRound.Instance.companyBuyingRate = nextAmount;
                }
                else
                {
                    ModStaticHelper.Logger.LogInfo($"Buying rate did not change due to chance of very low rate");
                }
            }
        }

        public static void SetDeadlineDaysAmount(this TimeOfDay timeOfDay, bool tryGetFromDisk = false)
        {
            if (!RoundManager.Instance.NetworkManager.IsHost) return;

            var deadlineDaysAmount = !tryGetFromDisk ? timeOfDay.CalculateDeadlineDaysAmount() : timeOfDay.GetDeadlineDaysAmountFromDisk();

            if (!ModStaticHelper.IsThisModInstalled("LethalExpansion"))
            {
                timeOfDay.quotaVariables.deadlineDaysAmount = deadlineDaysAmount;
            }
            else
            {
                deadlineDaysAmount = timeOfDay.quotaVariables.deadlineDaysAmount;
            }

            string currentSaveFile = GameNetworkManager.Instance.currentSaveFileName;

            ES3.Save<int>(ModStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS_SAVE_KEY, deadlineDaysAmount, currentSaveFile);

            ModStaticHelper.Logger.LogInfo($"Deadline days amount: {deadlineDaysAmount} (To calculate buying rate)");
        }

        public static int GetDeadlineDaysAmountFromDisk(this TimeOfDay timeOfDay)
        {
            string currentSaveFile = GameNetworkManager.Instance.currentSaveFileName;
            int deadlineDaysAmount;

            if (!ES3.KeyExists(ModStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS_SAVE_KEY, currentSaveFile))
            {
                ES3.Save<int>(ModStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS_SAVE_KEY, timeOfDay.CalculateDeadlineDaysAmount(), currentSaveFile);
                deadlineDaysAmount = ES3.Load<int>(ModStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS_SAVE_KEY, currentSaveFile);
                ModStaticHelper.Logger.LogInfo($"Saved deadlineDaysAmount to disk and then loaded, deadlineDaysAmount: {deadlineDaysAmount}");
            }
            else
            {
                deadlineDaysAmount = ES3.Load<int>(ModStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS_SAVE_KEY, currentSaveFile);
                ModStaticHelper.Logger.LogInfo($"Loaded deadlineDaysAmount from disk, deadlineDaysAmount: {deadlineDaysAmount}");
            }

            return deadlineDaysAmount;
        }

        public static void SyncTimeAndDeadline(this TimeOfDay timeOfDay)
        {
            timeOfDay.SyncTimeClientRpc(timeOfDay.globalTime, (int)timeOfDay.timeUntilDeadline);
            timeOfDay.UpdateProfitQuotaCurrentTime();

            ModStaticHelper.Logger.LogInfo("Deadline sync");
        }
    }
}
