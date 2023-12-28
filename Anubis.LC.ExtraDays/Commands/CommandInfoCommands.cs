using System.Linq;
using System.Text;
using GameNetcodeStuff;
using Anubis.LC.ExtraDays.Attributes;
using Anubis.LC.ExtraDays.Interactions;
using Anubis.LC.ExtraDays.Models;
using System;

namespace Anubis.LC.ExtraDays.Commands
{
    /// <summary>
    /// Rewriting the "Other" menu and add to it "Buydays" menu item
    /// </summary>
    public class CommandInfoCommands
    {
        [TerminalCommand("Other", clearText: true)]
        public string CommandList()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Other commands:");
            builder.AppendLine();
            builder.AppendLine(">VIEW MONITOR");
            builder.AppendLine("To toggle on/off the main monitor's map cam");
            builder.AppendLine();
            builder.AppendLine(">SWITCH {RADAR}");
            builder.AppendLine("To switch the player view on the main monitor");
            builder.AppendLine();
            builder.AppendLine(">PING [Radar booster name]");
            builder.AppendLine("To switch the player view on the main monitor");
            builder.AppendLine();
            builder.AppendLine(">SCAN");
            builder.AppendLine("To scan for the number of items left on the current planet");
            builder.AppendLine();
            builder.AppendLine(">TRANSMIT [message]");
            builder.AppendLine("Transmit a message with the signal translator");
            builder.AppendLine();

            foreach (var command in TerminalRegistry.EnumerateCommands())
            {
                if (command.Description == null)
                {
                    continue;
                }

                if (!command.CheckAllowed())
                {
                    continue;
                }

                builder.AppendLine($">{command.Name.ToUpper()} {command.Syntax?.ToUpper()}");
                builder.AppendLine(command.Description);
                builder.AppendLine();

            }

            return builder.ToString();
        }

        [TerminalCommand("deny_bea", clearText: false)]
        public string DenyBuyExtraDays()
        {
            ExtraDaysToDeadlinePlugin.LogSource.LogInfo("Player input DENY and deadline did not change");
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("Cancelled order.");
            builder.AppendLine();
            return builder.ToString();
        }

        [TerminalCommand("confirm_bea", clearText: false)]
        public string ConfirmBuyExtraDays()
        {
            TimeOfDayStaticHelper.AddXDaysToDeadline(1);
            ExtraDaysToDeadlinePlugin.LogSource.LogInfo("Player input CONFIRM and 1 day to deadline has been added");
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine("Extra day has been added to your deadline. Don't waste it!");
            builder.AppendLine();
            return builder.ToString();
        }

        [TerminalCommand("Buydays", clearText: true), CommandInfo("To ask the Company for an extra days to reach quota")]
        public ConfirmInteraction BuyDaysCommand(Terminal caller)
        {
            var timeOfDay = TimeOfDayStaticHelper.TimeOfDay;

            float creditsFormula = 0.1f * timeOfDay.profitQuota;

            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine(string.Format("You are about to ask the Company for an extra ONE day to reach quota. It will cost you {0} credits", creditsFormula));

            var terminalNode = new TerminalNode()
            {
                displayText = builder.ToString(),
                clearPreviousText = true,
                name = "Buydays"
            };

            var terminalInteraction = new ConfirmInteraction();
            terminalInteraction
                .WithPrompt(terminalNode)
                .Confirm(() =>
                {
                }).Deny(() =>
                {
                });
            return terminalInteraction;
        }
    }
}
