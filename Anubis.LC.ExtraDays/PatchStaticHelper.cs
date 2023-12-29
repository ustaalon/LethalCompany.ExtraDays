using Anubis.LC.ExtraDays.Patches;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Anubis.LC.ExtraDays
{
    public static class PatchStaticHelper
    {
        public static TimeOfDay TimeOfDay { get; set; }
        public static Terminal Terminal { get; set; }
        public static float lengthOfHours = 100f;
        public static int numberOfHours = 7;

        public static float GetTotalTime()
        {
            return lengthOfHours * (float)numberOfHours;
        }

        public static void SetNewCredits(int credits)
        {
            Terminal.groupCredits = credits;
        }

        public static void AddXDaysToDeadline(float days = 1f)
        {
            if (TimeOfDay == null)
            {
                ExtraDaysToDeadlinePlugin.LogSource.LogError("TimeOfDay object is null!");
                return;
            };

            TimeOfDay.timeUntilDeadline += TimeOfDay.totalTime * days;
            TimeOfDay.quotaVariables.deadlineDaysAmount += (int)days;

            TimeOfDay.UpdateProfitQuotaCurrentTime();

            ExtraDaysToDeadlinePlugin.LogSource.LogInfo("Added 1 day to deadline.");
        }

        public static int GetExtraDaysPrice()
        {
            var profitQuota = TimeOfDay.profitQuota;
            var baseIncrease = 0.15f * profitQuota;
            var minIncrease = 0.5f * profitQuota;
            var randommizer = (TimeOfDay.quotaVariables.randomizerCurve.Evaluate(UnityEngine.Random.Range(0f, 1f)) * TimeOfDay.quotaVariables.randomizerMultiplier + 1f);
            var priceXRandom = baseIncrease * randommizer;

            var price = (int)Mathf.Clamp((float)priceXRandom, minIncrease, 1E+09f);

            return price;
        }

        public static bool IsExtraDaysPurchasable()
        {
            if (ExtraDaysToDeadlinePlugin.IsInProcess) return true;
            return Terminal.groupCredits >= GetExtraDaysPrice();
        }

        public static void SetDaysToDeadline()
        {
            if (ExtraDaysToDeadlinePlugin.IsInProcess) return;

            ExtraDaysToDeadlinePlugin.IsInProcess = true;
            ExtraDaysToDeadlinePlugin.LogSource.LogInfo("Player input CONFIRM and 1 day to deadline has been added");
            AddXDaysToDeadline(1);
            float creditsFormula = GetExtraDaysPrice();
            var newCredits = Terminal.groupCredits -= (int)creditsFormula;
            SetNewCredits(newCredits);
        }
    }
}
