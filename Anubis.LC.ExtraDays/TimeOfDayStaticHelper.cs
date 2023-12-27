using Anubis.LC.ExtraDays.Patches;
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

        public static float GetTotalTime()
        {
            return lengthOfHours * (float)numberOfHours;
        }

        public static void AddXDaysToDeadline(float days = 1f)
        {
            if (TimeOfDay == null) return;

            TimeOfDay.daysUntilDeadline += (int)days;
            TimeOfDay.timeUntilDeadline += GetTotalTime() * days;
            TimeOfDay.UpdateProfitQuotaCurrentTime();
        }
    }
}
