using FChatSharpLib.Entities.Plugin.Commands;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class EnterRoom : BaseCommand<RDVFPlugin>
    {
        public static Dictionary<string, DateTime> CharacterCooldowns = new Dictionary<string, DateTime>();

        public async Task<List<string>> Execute(string characterCalling, IEnumerable<string> args)
        {
            var messages = new List<string>();


            var argsList = args.ToList();
            var ChannelCode = argsList[0];

            if (characterCalling == Constants.MayankAdmin)
            {
                try
                {
                    Plugin.FChatClient.JoinChannel($"adh-{ChannelCode}");
                    Plugin.AddHandledChannel($"adh-{ChannelCode}");
                }
                catch (Exception ex)
                {
                    messages.Add($"There was an error. Try again, and if it doesn't work, notify Elise Pariat by note with this as the message: {ex.Message}");
                    return messages;
                }
            }
            else
            {
                messages.Add("There was an error processing the room ID. Make sure to use the right syntax.");
            }

            return messages;
        }

        public override async Task ExecuteCommand(string characterCalling, IEnumerable<string> args, string channel)
        {
            if (characterCalling == Constants.MayankAdmin || characterCalling == Constants.EliseAdmin || characterCalling == Constants.AelithAdmin)
            {
                var result = await Execute(characterCalling, args);
                foreach (var message in result)
                {
                    Plugin.FChatClient.SendPrivateMessage($"{message}", characterCalling);
                }
            }

            else
            {
                Plugin.FChatClient.SendMessageInChannel("You cannot do that", channel);
            }
        }
    }
}