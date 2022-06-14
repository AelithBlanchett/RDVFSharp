using FChatSharpLib.Entities.Plugin.Commands;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using RDVFSharp.Helpers;

namespace RDVFSharp.Commands
{
    public class RoomList : BaseCommand<RDVFPlugin>
    {

        public async Task<List<string>> Execute(string characterCalling, IEnumerable<string> args)
        {
            var messages = new List<string>();

            if (RoomCreate.CharacterRoomsIds != null && RoomCreate.CharacterRoomsIds.Any())
            {
                var output = $"Here are all the available rooms ({RoomCreate.CharacterRoomsIds.Count}):\n\n";

                foreach (var room in RoomCreate.CharacterRoomsIds.OrderByDescending(x => x.CreationTime))
                {
                    output += $"#[b]{room.Id}[/b]: {room.ChannelName}, created by {room.CreatorId} ({room.CreationTime.GetPrettyDateDiffWithToday()})  --- !roomjoin {room.Id}\n";
                }

                messages.Add(output);
            }
            else
            {
                messages.Add("No rooms have been opened recently, but you can create yours like that: '!roomcreate'.");
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
            var result = await Execute(characterCalling, args);
            foreach (var message in result)
            {
                Plugin.FChatClient.SendMessageInChannel($"{message}", channel);
            }
        }
    }
}
