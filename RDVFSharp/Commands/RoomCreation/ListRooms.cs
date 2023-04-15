using FChatSharpLib.Entities.Plugin.Commands;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using RDVFSharp.Helpers;

namespace RDVFSharp.Commands
{
    public class ListRooms : BaseCommand<RDVFPlugin>
    {

        public async Task<List<string>> Execute(string characterCalling, IEnumerable<string> args)
        {
            var messages = new List<string>();

            if (CreateRoom.CharacterRoomsIds != null && CreateRoom.CharacterRoomsIds.Any())
            {
                var output = $"Here are all the available rooms ({CreateRoom.CharacterRoomsIds.Count}):\n\n";

                foreach (var room in CreateRoom.CharacterRoomsIds.OrderByDescending(x => x.CreationTime))
                {
                    output += $"#[b]{room.Id}[/b]: {room.ChannelName}, created by {room.CreatorId} ({room.CreationTime.GetPrettyDateDiffWithToday()})  --- !joinroom {room.Id}\n";
                }

                messages.Add(output);
            }
            else
            {
                messages.Add("No rooms have been opened recently, but you can create yours like that: '!createroom name'.");
            }

            return messages;
        }


        public override async Task ExecutePrivateCommand(string characterCalling, IEnumerable<string> args)
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
                    Plugin.FChatClient.SendMessageInChannel($"{message}", channel);
                }
            }

            else
            {
                Plugin.FChatClient.SendMessageInChannel("You cannot do that", channel);
            }
        }
    }
}
