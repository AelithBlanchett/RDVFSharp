﻿using FChatSharpLib.Entities.Plugin.Commands;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using RDVFSharp.Helpers;

namespace RDVFSharp.Commands
{
    public class Look : BaseCommand<RDVFPlugin>
    {

        public async Task<List<string>> Execute(string characterCalling, IEnumerable<string> args)
        {
            var messages = new List<string>();

            if (Looking.LookingInformation != null && Looking.LookingInformation.Any())
            {
                var output = $"Here are all the people that are looking for a fight! ({Looking.LookingInformation.Count}):\n\n";

                foreach (var fighter in Looking.LookingInformation)
                {
                    output += $"[user]{fighter.CharacterId}[/user], ";
                }

                messages.Add(output);
            }
            else
            {
                messages.Add("Nobody has been set to looking yet, but you can by typing: '!looking'.");
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
