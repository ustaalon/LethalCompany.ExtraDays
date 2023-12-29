using System.Linq;
using System.Text;
using GameNetcodeStuff;
using LethalAPI.TerminalCommands.Attributes;
using LethalAPI.TerminalCommands.Models;
using System;
using LethalAPI.TerminalCommands.Interfaces;
using LethalAPI.TerminalCommands.Interactions;

namespace Anubis.LC.ExtraDays.Commands
{
    /// <summary>
    /// Buy an extra days commands
    /// </summary>
    public class BuyExtraDaysCommands
    {
        [RequireInterface(typeof(ConfirmInteraction))]
        [TerminalCommand("deny", clearText: false)]
        public string DenyBuyExtraDays()
        {
            ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Player denied so the deadline didn't change!");
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("Cancelled order.");
            builder.AppendLine();
            return builder.ToString();
        }

        [RequireInterface(typeof(ConfirmInteraction))]
        [TerminalCommand("confirm", clearText: false)]
        public string ConfirmBuyExtraDays(Terminal terminal)
        {
            var builder = new StringBuilder();
            if (!terminal.IsExtraDaysPurchasable())
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Player has insufficient credits to purchase an extra day");
                builder.AppendLine();
                builder.AppendLine("You don't have enough credits to buy an extra day.");
                builder.AppendLine();
                builder.AppendLine("Cancelled order.");
                builder.AppendLine();
                return builder.ToString();
            }

            terminal.SetDaysToDeadline();
            builder.AppendLine();
            builder.AppendLine("An extra day has been added to your deadline. Don't waste it!");
            builder.AppendLine();
            return builder.ToString();
        }

        [TerminalCommand("buyday", clearText: true), CommandInfo("Ask the Company for an extra day to reach the quota")]
        public ITerminalInteraction BuyDaysCommand()
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine(string.Format("You're about to ask the Company for ONE extra day to reach the profit quota. It will cost you {0} credits.", TimeOfDay.Instance.GetExtraDaysPrice()));

            var terminalNode = new TerminalNode()
            {
                displayText = builder.ToString(),
                clearPreviousText = true,
                name = "buyday"
            };

            return new ConfirmInteraction(terminalNode, ConfirmBuyExtraDays, DenyBuyExtraDays);
        }
    }
}
