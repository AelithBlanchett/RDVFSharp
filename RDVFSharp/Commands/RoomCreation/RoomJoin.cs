using FChatSharpLib.Entities.Plugin.Commands;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class RoomJoin : BaseCommand<RDVFPlugin>
    {
        public static Dictionary<string, DateTime> CharacterCooldowns = new Dictionary<string, DateTime>();

        public async Task<List<string>> Execute(string characterCalling, IEnumerable<string> args)
        {
            var messages = new List<string>();


            int roomId = 0;

            if (args.Any() && int.TryParse(args.First(), out roomId) && RoomCreate.CharacterRoomsIds.Any(x => x.Id == roomId))
            {
                var room = RoomCreate.CharacterRoomsIds.First(x => x.Id == roomId);

                try
                {
                    Plugin.FChatClient.SendPrivateMessage($"[session={room.ChannelName}]{room.Channel}[/session]", characterCalling);
                }
                catch (Exception ex)
                {
                    messages.Add($"There was an error. Try again, and if it doesn't work, notify Elise Pariat by note with this as the message: {ex.Message}");
                    return messages;
                }
            }
            else
            {
                messages.Add("There was an error processing the room ID. Make sure to use the right syntax: Example: '!roomjoin 123'.");
            }

            return messages;
        }

        public async new void ExecutePrivateCommand(string characterCalling, IEnumerable<string> args)
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
                Plugin.FChatClient.SendPrivateMessage("You cannot do that", characterCalling);
            }
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
