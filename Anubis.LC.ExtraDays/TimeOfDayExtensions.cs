﻿using BepInEx;
using BepInEx.Bootstrap;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Anubis.LC.ExtraDays
{
    public static class TimeOfDayExtensions
    {
        private static int extraDayPrice = 0;

        public static void AddXDaysToDeadline(this TimeOfDay timeOfDay, float days = 1f)
        {
            timeOfDay.timeUntilDeadline += timeOfDay.totalTime * days;
            timeOfDay.ReCalculateBuyingRateForCompany();
            timeOfDay.SyncTimeAndDeadline();

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Added 1 day to deadline.");
        }

        public static int CalculateDeadlineDaysAmount(this TimeOfDay timeOfDay)
        {
            int days = (int)Mathf.Floor(timeOfDay.timeUntilDeadline / timeOfDay.totalTime);
            return (int)days;
        }

        public static void SetExtraDaysPrice(this TimeOfDay timeOfDay)
        {
            int profitQuota = timeOfDay.profitQuota;
            float baseIncrease = 0.15f * profitQuota;
            float minIncrease = 0.5f * profitQuota;
            float randommizer = (timeOfDay.quotaVariables.randomizerCurve.Evaluate(UnityEngine.Random.Range(0f, 1f)) * timeOfDay.quotaVariables.randomizerMultiplier + 1f);
            float priceXRandom = baseIncrease * randommizer;

            int price = (int)Mathf.Clamp(priceXRandom, minIncrease, 1E+09f);

            extraDayPrice = price;
        }

        public static int GetExtraDaysPrice(this TimeOfDay timeOfDay)
        {
            return extraDayPrice;
        }

        public static void ResetDeadline(this TimeOfDay timeOfDay, bool isShipReset = false)
        {
            if (isShipReset || !ExtraDaysToDeadlineStaticHelper.IsDynamicDeadlinesModInstalled())
            {
                timeOfDay.timeUntilDeadline = timeOfDay.totalTime * 3f;

                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Deadline reset to defaults");
            }

            timeOfDay.ReCalculateBuyingRateForCompany();
            timeOfDay.SyncTimeAndDeadline();
        }

        public static void ReCalculateBuyingRateForCompany(this TimeOfDay timeOfDay)
        {
            timeOfDay.SetDeadlineDaysAmount();
            timeOfDay.SetBuyingRateForDay();
            StartOfRound.Instance.SyncCompanyBuyingRateClientRpc(StartOfRound.Instance.companyBuyingRate);
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Buying rate recalculated");
        }

        public static void SetDeadlineDaysAmount(this TimeOfDay timeOfDay)
        {
            // To calculate buying rate for the company
            timeOfDay.quotaVariables.deadlineDaysAmount = timeOfDay.CalculateDeadlineDaysAmount();

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Deadline days amount: {timeOfDay.CalculateDeadlineDaysAmount()}");
        }

        public static void SyncTimeAndDeadline(this TimeOfDay timeOfDay)
        {
            timeOfDay.UpdateProfitQuotaCurrentTime();
            timeOfDay.SyncTimeClientRpc(timeOfDay.globalTime, (int)timeOfDay.timeUntilDeadline);

            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Deadline sync");
        }
    }
}
