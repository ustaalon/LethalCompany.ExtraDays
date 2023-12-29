using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Anubis.LC.ExtraDays
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
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Player input CONFIRM and 1 day to deadline has been added");
            TimeOfDay.Instance.AddXDaysToDeadline(5);

            float creditsFormula = TimeOfDay.Instance.GetExtraDaysPrice();
            int newCredits = terminal.groupCredits -= (int)creditsFormula;

            terminal.SetNewCredits(newCredits);
        }
    }
}
