using FChatSharpLib.Entities.Plugin.Commands;
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
            await Task.Delay(4000);

            if (!string.IsNullOrEmpty(characterCalling))
            {
                messages.Add($"[icon]{characterCalling}[/icon] is no longer looking for a fight!!");
                Looking.LookingInformation.Remove(characterCalling);
            }
            else
            {
                messages.Add("The bot couldn't remove your looking status. If you are not removed from the looking list (Which you can see by typing !look), please contact [user]Mayank[/user].");
            }

            return messages;
        }

        public async new void ExecutePrivateCommand(string characterCalling, IEnumerable<string> args)
        {
            var result = await Execute(characterCalling, args);
            foreach (var message in result)
            {
                Plugin.FChatClient.SendPrivateMessage(message, characterCalling);
            }
        }

        public override async Task ExecuteCommand(string characterCalling, IEnumerable<string> args, string channel)
        {
            if (channel == "ADH-a823a4e998a2b3d31794")

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


