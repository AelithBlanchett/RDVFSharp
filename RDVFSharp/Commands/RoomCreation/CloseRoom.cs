using FChatSharpLib.Entities.Plugin.Commands;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class CloseRoom : BaseCommand<RDVFPlugin>
    {
        public async Task<List<string>> Execute(string characterCalling, IEnumerable<string> args)
        {
            var messages = new List<string>();


            int roomId = 0;

            if (args.Any() && int.TryParse(args.First(), out roomId) && CreateRoom.CharacterRoomsIds.Any(x => x.Id == roomId))
            {
                var room = CreateRoom.CharacterRoomsIds.First(x => x.Id == roomId);

                if ((room.CreatorId == characterCalling) || (Constants.MayankAdmin == characterCalling))
                {
                    Plugin.RemoveHandledChannel(room.Channel);
                    Plugin.FChatClient.LeaveChannel(room.Channel);
                    CreateRoom.CharacterRoomsIds.RemoveAll(x => x.Id == roomId);
                    messages.Add($"The room {roomId} has been successfully closed.");
                }
                else
                {
                    messages.Add("There was an error deleting the room with that room ID. This room doesn't exist.");
                }
            }
            else
            {
                messages.Add("There was an error deleting the room with that room ID. Make sure to use the right syntax: Example: '!closeroom 123'.");
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
                Plugin.FChatClient.SendPrivateMessage($"{message}", characterCalling);
            }
        }
    }
}