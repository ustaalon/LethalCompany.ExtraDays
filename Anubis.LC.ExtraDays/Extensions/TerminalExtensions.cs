using System;
using System.Collections.Generic;
using System.Text;
using Anubis.LC.ExtraDays.Helpers;
using UnityEngine;

namespace Anubis.LC.ExtraDays.Extensions
{
    public static class TerminalExtensions
    {
        public static bool IsExtraDaysPurchasable(this Terminal terminal)
        {
            return terminal.groupCredits >= TimeOfDay.Instance.GetExtraDaysPrice();
        }

        public static void SetDaysToDeadline(this Terminal terminal)
        {
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo($"Player input CONFIRM and {ExtraDaysToDeadlineStaticHelper.DAYS_TO_INCREASE} day to deadline has been added");
            float creditsFormula = TimeOfDay.Instance.GetExtraDaysPrice();
            terminal.groupCredits -= (int)creditsFormula;
            terminal.SyncGroupCreditsServerRpc(terminal.groupCredits, terminal.numberOfItemsInDropship);
            TimeOfDay.Instance.AddXDaysToDeadline(ExtraDaysToDeadlineStaticHelper.DAYS_TO_INCREASE);
        }
    }
}
