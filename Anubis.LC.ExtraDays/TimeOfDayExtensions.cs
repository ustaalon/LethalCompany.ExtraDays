using Anubis.LC.ExtraDays.Helpers;
using Anubis.LC.ExtraDays.Models;
using BepInEx;
using BepInEx.Bootstrap;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Anubis.LC.ExtraDays
{
    public static class TimeOfDayExtensions
    {
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

            return (int)days;
        }

        public static int GetExtraDaysPrice(this TimeOfDay timeOfDay)
        {
            int profitQuota = timeOfDay.profitQuota;
            float baseIncrease = 0.15f * profitQuota;
            float minIncrease = 0.5f * profitQuota;
            float randommizer = (timeOfDay.quotaVariables.randomizerCurve.Evaluate(UnityEngine.Random.Range(0f, 1f)) * timeOfDay.quotaVariables.randomizerMultiplier + 1f);
            float priceXRandom = baseIncrease * randommizer;

            int price = (int)Mathf.Clamp(priceXRandom, minIncrease, 1E+09f);
            return price;
        }

        public static void ResetDeadline(this TimeOfDay timeOfDay, bool isShipReset = false)
        {
            if (isShipReset || !ExtraDaysToDeadlineStaticHelper.IsDynamicDeadlinesModInstalled())
            {
                timeOfDay.timeUntilDeadline = timeOfDay.totalTime * (float)ExtraDaysToDeadlineStaticHelper.DEFAULT_AMOUNT_OF_DEADLINE_DAYS;

                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Deadline reset to defaults");
            }

            timeOfDay.ReCalculateBuyingRateForCompany();
            timeOfDay.SyncTimeAndDeadline();
        }

        public static void ReCalculateBuyingRateForCompany(this TimeOfDay timeOfDay, bool tryGetFromDisk = false)
        {
            timeOfDay.SetDeadlineDaysAmount(tryGetFromDisk);
            timeOfDay.SetBuyingRateForDay();
            StartOfRound.Instance.SyncCompanyBuyingRateClientRpc(StartOfRound.Instance.companyBuyingRate);
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Buying rate recalculated");
        }

        public static void SetDeadlineDaysAmount(this TimeOfDay timeOfDay, bool tryGetFromDisk = false)
        {
            var deadlineDaysAmount = !tryGetFromDisk ? timeOfDay.CalculateDeadlineDaysAmount() : timeOfDay.GetDeadlineDaysAmountFromDisk();

            timeOfDay.quotaVariables.deadlineDaysAmount = deadlineDaysAmount;
            SaveHelper.WriteSettings(new Settings()
            {
                DeadlineDaysAmount = deadlineDaysAmount
            });

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Deadline days amount: {deadlineDaysAmount}");
        }

        public static int GetDeadlineDaysAmountFromDisk(this TimeOfDay timeOfDay)
        {
            bool isHost = RoundManager.Instance.NetworkManager.IsHost;
            if (!isHost) return timeOfDay.CalculateDeadlineDaysAmount();

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
