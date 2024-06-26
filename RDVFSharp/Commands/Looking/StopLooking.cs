﻿using FChatSharpLib.Entities.Plugin.Commands;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using RDVFSharp.Helpers;
using System.Timers;

namespace RDVFSharp.Commands
{
    public class StopLooking : BaseCommand<RDVFPlugin>
    {
        public async Task<List<string>> Execute(string characterCalling, IEnumerable<string> args)
        {
            var messages = new List<string>();

            if (!string.IsNullOrEmpty(characterCalling))
            {
                messages.Add($"[icon]{characterCalling}[/icon] is no longer looking for a fight!!");
                Looking.LookingInformation.RemoveAll(x => x.CharacterId == characterCalling);
            }
            else
            {
                messages.Add("The bot couldn't remove your looking status. If you are not removed from the looking list (Which you can see by typing !look), please contact [user]Mayank[/user].");
            }

            return messages;
        }

        public override async Task ExecutePrivateCommand(string characterCalling, IEnumerable<string> args)
        {
            var result = await Execute(characterCalling, args);
            foreach (var message in result)
            {
                Plugin.FChatClient.SendPrivateMessage(message, characterCalling);
            }
        }

        public override async Task ExecuteCommand(string characterCalling, IEnumerable<string> args, string channel)
        {
            if (channel == Constants.RDVFBar)

            {
                var result = await Execute(characterCalling, args);
                foreach (var message in result)
                {
                    Plugin.FChatClient.SendMessageInChannel($"{message}", channel);
                }
            }

            else
            {
                Plugin.FChatClient.SendMessageInChannel("You cannot do that in here", channel);
            }
        }
    }
}


