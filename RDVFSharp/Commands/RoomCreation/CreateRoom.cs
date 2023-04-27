using FChatSharpLib.Entities.Plugin.Commands;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using RDVFSharp.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Channels;

namespace RDVFSharp.Commands
{
    public class CreateRoom : BaseCommand<RDVFPlugin>
    {
        public static Dictionary<string, DateTime> CharacterCooldowns = new Dictionary<string, DateTime>();
        public static List<ChannelInfo> CharacterRoomsIds = new List<ChannelInfo>();
        private static int _counter = 1;

        public class ChannelInfo
        {
            public int Id { get; set; }
            public string Channel { get; set; }
            public int EntryPrice { get; set; }
            public DateTime CreationTime { get; set; }
            public string ChannelName { get; set; }
            public string CreatorId { get; set; }
        }

        public async Task<List<string>> Execute(string characterCalling, IEnumerable<string> args)
        {
            var messages = new List<string>();

            if (CharacterCooldowns.ContainsKey(characterCalling))
            {
                if (CharacterCooldowns[characterCalling] > DateTime.Now)
                {
                    var message = $"Please wait before creating another room. (Cooldown: {(CharacterCooldowns[characterCalling] - DateTime.Now).FormatTimeSpan()} left)";
                    messages.Add($"{message}");
                    return messages;
                }
            }

            
            var argsList = args.ToList();
            var NamedChannel = string.Join(" ", args);
            var room = new ChannelInfo()
            {
                CreationTime = DateTime.Now,
                ChannelName = $"RDVF - {NamedChannel}",
                CreatorId = characterCalling
            };

            Plugin.FChatClient.BotCreatedChannel += FChatClient_BotCreatedChannel;
            Plugin.FChatClient.CreateChannel(room.ChannelName);

            var delay = 100; //in ms
            var totalDelay = 0;

            for (int i = 0; i < 100; i++)
            {
                if (!string.IsNullOrEmpty(_newChannelId))
                {
                    break;
                }
                await Task.Delay(delay);
                totalDelay += delay;
            }

            if (!string.IsNullOrEmpty(_newChannelId))
            {
                room.Id = _counter;
                _counter++;
                room.Channel = _newChannelId;

                CharacterRoomsIds.Add(room);

                messages.Add($"A new channel titled '{room.ChannelName}' has been created for you ({room.Channel}).\n" +
                    $"An invite will be sent to you right away.\n" +
                    $"Please invite whoever you want to by using the '/invite name' function within the room!\n" + 
                    $"Please read the room description once you enter!");

                Plugin.FChatClient.InviteUserToChannel(characterCalling, room.Channel);
                Plugin.FChatClient.ModUser(characterCalling, room.Channel);
                Plugin.FChatClient.ChangeChannelDescription($"[b]To close this room, {room.CreatorId} must type '!closeroom {room.Id}', or please ask [user]Mayank[/user] to close the room for you![/b]\n" + 
                "What happens in this room is outside the scope of the main room admins. Please keep that in mind when using this private room feature!", room.Channel);
                Plugin.FChatClient.ModUser(Constants.EliseAdmin, room.Channel);
                Plugin.FChatClient.ModUser(Constants.AelithAdmin, room.Channel);
                Plugin.FChatClient.ModUser(Constants.VCBot, room.Channel);
                Plugin.FChatClient.ChangeChannelOwner(Constants.MayankAdmin, room.Channel);
                Plugin.AddHandledChannel(room.Channel);
            }
            else
            {
                messages.Add("The bot couldn't create the channel. Please try again later.");
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
            if (channel == Constants.RDVFVenue || channel == Constants.RDVFArena)

            {
                Plugin.FChatClient.SendMessageInChannel("You cannot do that in here", channel);  
            }

            else
            {
                var result = await Execute(characterCalling, args);
                foreach (var message in result)
                {
                    Plugin.FChatClient.SendMessageInChannel($"{message}", channel);
                }
            }
        }


        private string _newChannelId = "";

        private void FChatClient_BotCreatedChannel(object sender, FChatSharpLib.Entities.Events.Server.JoinChannel e)
        {
            _newChannelId = e.channel;
            Console.WriteLine("New channel created.");
        }
    }
}
