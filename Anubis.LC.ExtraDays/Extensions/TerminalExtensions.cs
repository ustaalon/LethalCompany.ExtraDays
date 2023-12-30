using System;
using System.Collections.Generic;
using System.Text;
using Anubis.LC.ExtraDays.Helpers;
using UnityEngine;

namespace Anubis.LC.ExtraDays.Extensions
{
    public static class TerminalExtensions
    {
        public static void SetNewCredits(this Terminal terminal, int credits)
        {
            terminal.groupCredits = credits;
        }

        public static bool IsExtraDaysPurchasable(this Terminal terminal)
        {
            return terminal.groupCredits >= TimeOfDay.Instance.GetExtraDaysPrice();
        }

        public static void SetDaysToDeadline(this Terminal terminal)
        {
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Player input CONFIRM and {ExtraDaysToDeadlineStaticHelper.DAYS_TO_INCREASE} day to deadline has been added");
            TimeOfDay.Instance.AddXDaysToDeadline(ExtraDaysToDeadlineStaticHelper.DAYS_TO_INCREASE);

            float creditsFormula = TimeOfDay.Instance.GetExtraDaysPrice();
            int newCredits = terminal.groupCredits -= (int)creditsFormula;

            terminal.SetNewCredits(newCredits);
        }
    }
}
