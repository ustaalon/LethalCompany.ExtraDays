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
            timeOfDay.quotaVariables.deadlineDaysAmount += (int)days;
            timeOfDay.UpdateProfitQuotaCurrentTime();

            ExtraDaysToDeadlinePlugin.LogSource.LogInfo("Added 1 day to deadline.");
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
    }
}
