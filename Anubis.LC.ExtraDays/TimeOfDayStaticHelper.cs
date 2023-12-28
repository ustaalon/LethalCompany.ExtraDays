using Anubis.LC.ExtraDays.Patches;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anubis.LC.ExtraDays
{
    public static class TimeOfDayStaticHelper
    {
        public static TimeOfDay TimeOfDay { get; set; }
        public static float lengthOfHours = 100f;
        public static int numberOfHours = 7;

        public static bool IsInProcess = false;

        public static float GetTotalTime()
        {
            return lengthOfHours * (float)numberOfHours;
        }

        public static void AddXDaysToDeadline(float days = 1f)
        {
            if (IsInProcess) return;
            if (TimeOfDay == null)
            {
                ExtraDaysToDeadlinePlugin.LogSource.LogError("TimeOfDay object is null!");
                return;
            };
            IsInProcess = true;

            TimeOfDay.timeUntilDeadline += TimeOfDay.totalTime * days;
            //TimeOfDay.totalTime += TimeOfDay.totalTime * days;
            TimeOfDay.UpdateProfitQuotaCurrentTime();

            ExtraDaysToDeadlinePlugin.LogSource.LogInfo("Added 1 day to deadline.");
            IsInProcess = false;
        }
    }
}
