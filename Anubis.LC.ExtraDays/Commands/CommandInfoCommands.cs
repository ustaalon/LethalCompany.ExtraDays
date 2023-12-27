using System.Linq;
using System.Text;
using GameNetcodeStuff;
using Anubis.LC.ExtraDays.Attributes;
using Anubis.LC.ExtraDays.Interactions;
using Anubis.LC.ExtraDays.Models;

namespace Anubis.LC.ExtraDays.Commands
{
    /// <summary>
    /// Contains Terminal Command definitions for the built-in help commands
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

        [TerminalCommand("Buydays", clearText: true), CommandInfo("To ask the Company for an extra days to reach quota")]
        public ConfirmInteraction BuyDaysCommand(Terminal caller)
        {
            var builder = new StringBuilder();

            var timeOfDay = TimeOfDayStaticHelper.TimeOfDay;

            float creditsFormula = 0.1f * timeOfDay.profitQuota;

            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine(string.Format("You are about to ask the Company for an extra ONE day to reach quota. It will cost you {0} credits", creditsFormula));
            builder.AppendLine();

            var confirmOrDeny = new ConfirmInteraction(builder.ToString(), () =>
            {
                var builder = new StringBuilder();
                if (caller.groupCredits <= creditsFormula)
                {
                    builder.AppendLine();
                    builder.AppendLine();
                    builder.AppendLine("You don't have enough credits to buy an extra day");
                    builder.AppendLine();
                    return builder.ToString();
                }
                caller.groupCredits = caller.groupCredits - (int)creditsFormula;

                TimeOfDayStaticHelper.AddXDaysToDeadline(1);

                builder.AppendLine("Added ONE extra day to reach deadline. Don't waste it!");
                return builder.ToString();

            }, () => { });

            return confirmOrDeny;
        }
    }
}
