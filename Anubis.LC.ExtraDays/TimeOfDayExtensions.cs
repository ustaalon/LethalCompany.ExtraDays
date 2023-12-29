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
            timeOfDay.quotaVariables.deadlineDaysAmount = timeOfDay.CalculateDeadlineDaysAmount(); // To calculate buying rate for the company
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"DeadlineDaysAmount: {timeOfDay.CalculateDeadlineDaysAmount()}");
            timeOfDay.SyncTimeAndDeadline();

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Added 1 day to deadline.");
        }

        public static int CalculateDeadlineDaysAmount(this TimeOfDay timeOfDay)
        {
            int days = (int)Mathf.Floor(timeOfDay.timeUntilDeadline / timeOfDay.totalTime);
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
                timeOfDay.timeUntilDeadline = timeOfDay.totalTime * 3f;

                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Deadline reset to defaults");
            }

            timeOfDay.quotaVariables.deadlineDaysAmount = timeOfDay.CalculateDeadlineDaysAmount();

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Buying rate recalculated");

            timeOfDay.SyncTimeAndDeadline();
        }

        public static void SyncTimeAndDeadline(this TimeOfDay timeOfDay)
        {
            timeOfDay.UpdateProfitQuotaCurrentTime();
            timeOfDay.SyncTimeClientRpc(timeOfDay.globalTime, (int)timeOfDay.timeUntilDeadline);
            timeOfDay.SetBuyingRateForDay();
            StartOfRound.Instance.SyncCompanyBuyingRateClientRpc(StartOfRound.Instance.companyBuyingRate);

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Deadline & Buying rate SYNC");
        }
    }
}
