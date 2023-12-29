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
            ExtraDaysToDeadlinePlugin.LogSource.LogInfo("Player input DENY and deadline did not change");
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
            if (!terminal.IsExtraDaysPurchasable(TimeOfDay.Instance))
            {
                ExtraDaysToDeadlinePlugin.LogSource.LogInfo("The player have insufficient credits to purchase an extra day");
                builder.AppendLine();
                builder.AppendLine("YOU do not have enough credits to buy an extra day.");
                builder.AppendLine();
                builder.AppendLine("Cancelled order.");
                builder.AppendLine();
                return builder.ToString();
            }

            terminal.SetDaysToDeadline(TimeOfDay.Instance);
            builder.AppendLine();
            builder.AppendLine("Extra day has been added to your deadline. Don't waste it!");
            builder.AppendLine();
            return builder.ToString();
        }

        [TerminalCommand("Buydays", clearText: true), CommandInfo("To ask the Company for an extra days to reach quota")]
        public ITerminalInteraction BuyDaysCommand()
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine(string.Format("You are about to ask the Company for an extra ONE day to reach quota. It will cost you {0} credits", TimeOfDay.Instance.GetExtraDaysPrice()));

            var terminalNode = new TerminalNode()
            {
                displayText = builder.ToString(),
                clearPreviousText = true,
                name = "Buydays"
            };

            //ExtraDaysToDeadlinePlugin.IsInProcess = false;
            var terminalInteraction = new ConfirmInteraction(terminalNode, ConfirmBuyExtraDays, DenyBuyExtraDays);

            return terminalInteraction;
        }
    }
}
