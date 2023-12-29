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

        public static bool IsExtraDaysPurchasable(this Terminal terminal, TimeOfDay timeOfDay)
        {
            return terminal.groupCredits >= timeOfDay.GetExtraDaysPrice();
        }

        public static void SetDaysToDeadline(this Terminal terminal, TimeOfDay timeOfDay)
        {
            ExtraDaysToDeadlinePlugin.LogSource.LogInfo("Player input CONFIRM and 1 day to deadline has been added");
            timeOfDay.AddXDaysToDeadline(1);

            float creditsFormula = timeOfDay.GetExtraDaysPrice();
            int newCredits = terminal.groupCredits -= (int)creditsFormula;

            terminal.SetNewCredits(newCredits);
        }
    }
}
