using FChatSharpLib.Entities.Plugin.Commands;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using RDVFSharp.Helpers;
using System.Timers;

namespace RDVFSharp.Commands
{
    public class Looking : BaseCommand<RDVFPlugin>
    {
        public Timer LookingTimer = new Timer(7200000);
        public static Dictionary<string, DateTime> CharacterCooldowns = new Dictionary<string, DateTime>();
        public static List<string> LookingInformation = new List<string>();

        public class LookingInfo
        {
            public string LookingId { get; set; }
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

            await Task.Delay(4000);

            if (!string.IsNullOrEmpty(characterCalling))
            {
                messages.Add($"[icon]{characterCalling}[/icon] " + LookingSelect.SelectRandom() + " (!look to see all available fighters!)");
                Plugin.FChatClient.SendPrivateMessage("You are now on the !look list of fighters looking for a fight, you will be automatically removed after 2 hours, or when you use the !stoplooking command", characterCalling);
                LookingInformation.AddIfNotContains(characterCalling);

                LookingTimer.Start();
                LookingTimer.Elapsed += Lookingover;
                LookingTimer.AutoReset = false;
                void Lookingover(Object source, System.Timers.ElapsedEventArgs e)
                {
                    LookingInformation.Remove(characterCalling);
                    Plugin.FChatClient.SendPrivateMessage("Your looking status has expired. If you want to renew it, please type !looking in the room once again!", characterCalling);
                }
            }
            else
            {
                messages.Add("The bot couldn't set you to looking. If you are not already on the looking list (Which you can see by typing !look), please contact [user]Mayank[/user].");
            }

            return messages;
        }


        public async new void ExecutePrivateCommand(string characterCalling, IEnumerable<string> args)
        {
            var result = await Execute(characterCalling, args);
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