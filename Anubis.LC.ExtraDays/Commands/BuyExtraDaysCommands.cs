﻿using System.Text;
using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Models;
using LethalAPI.LibTerminal.Interfaces;
using LethalAPI.LibTerminal.Interactions;
using Anubis.LC.ExtraDays.Helpers;
using Anubis.LC.ExtraDays.Extensions;
using Anubis.LC.ExtraDays.ModNetwork;

namespace Anubis.LC.ExtraDays.Commands
{
    /// <summary>
    /// Buy an extra days commands
    /// </summary>
    public class BuyExtraDaysCommands
    {
        [RequireInterface(typeof(ConfirmInteraction))]
        [TerminalCommand("deny", clearText: false)]
        public string DenyBuyExtraDays(Terminal terminal)
        {
            terminal.terminalAudio.PlayOneShot(terminal.syncedAudios[1]);
            ModStaticHelper.Logger.LogInfo("Player denied so the deadline didn't change!");
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
                terminal.terminalAudio.PlayOneShot(terminal.syncedAudios[1]);
                ModStaticHelper.Logger.LogInfo("Player has insufficient credits to purchase an extra day");
                builder.AppendLine();
                builder.AppendLine("You don't have enough credits to buy an extra day.");
                builder.AppendLine();
                builder.AppendLine("Cancelled order.");
                builder.AppendLine();
                return builder.ToString();
            }

            terminal.terminalAudio.PlayOneShot(terminal.syncedAudios[0]);
            Networking.Instance.BuyExtraDayServerRpc();
            builder.AppendLine();
            builder.AppendLine("An extra day has been added to your deadline. Don't waste it!");
            builder.AppendLine();
            builder.AppendLine($"({TimeOfDay.Instance.daysUntilDeadline} day(s) remaining)");
            builder.AppendLine();
            return builder.ToString();
        }

        [TerminalCommand("buyday", clearText: true), CommandInfo("Ask the Company for an extra day to reach the quota")]
        public ITerminalInteraction BuyDaysCommand()
        {
            var builder = new StringBuilder();

            var terminalNode = new TerminalNode()
            {
                displayText = string.Format("You're about to ask the Company for ONE extra day to reach the profit quota. It will cost you {0} credits ({1} day(s) remaining).", TimeOfDay.Instance.GetExtraDaysPrice(), TimeOfDay.Instance.daysUntilDeadline),
                clearPreviousText = true,
                name = "buyday"
            };

            return new ConfirmInteraction(terminalNode, ConfirmBuyExtraDays, DenyBuyExtraDays);
        }
    }
}
