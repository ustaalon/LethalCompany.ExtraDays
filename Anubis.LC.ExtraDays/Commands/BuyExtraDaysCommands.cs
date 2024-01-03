﻿using System.Linq;
using System.Text;
using GameNetcodeStuff;
using LethalAPI.LibTerminal.Attributes;
using LethalAPI.LibTerminal.Models;
using System;
using LethalAPI.LibTerminal.Interfaces;
using LethalAPI.LibTerminal.Interactions;
using Anubis.LC.ExtraDays.Helpers;
using Anubis.LC.ExtraDays.Extensions;

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
            if(!RoundManager.Instance.NetworkManager.IsHost)
            {
                ExtraDaysToDeadlineStaticHelper.Logger.LogInfo("Player is not the host. Deadline won't be change.");
                builder.AppendLine();
                builder.AppendLine("Only the ship's captain can ask for an extra day.");
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine("Cancelled order.");
                builder.AppendLine();
                return builder.ToString();
            }

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
            TimeOfDay.Instance.SetExtraDaysPrice();
            var builder = new StringBuilder();
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
