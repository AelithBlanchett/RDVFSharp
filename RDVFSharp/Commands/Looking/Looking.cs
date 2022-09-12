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
        public static Dictionary<string, DateTime> CharacterCooldowns = new Dictionary<string, DateTime>();
        public static List<LookingInfo> LookingInformation = new List<LookingInfo>();

        public class LookingInfo
        {
            public string CharacterId { get; set; }
            public Timer ExpirationTimer { get; set; }
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

            if (!string.IsNullOrEmpty(characterCalling))
            {
                messages.Add($"[icon]{characterCalling}[/icon] " + LookingSelect.SelectRandom() + " (!look to see all available fighters!)");
                Plugin.FChatClient.SendPrivateMessage("You are now on the !look list of fighters looking for a fight, you will be automatically removed after 2 hours, or when you use the !stoplooking command", characterCalling);

                if (!LookingInformation.Any(x => x.CharacterId == characterCalling))
                {
                    var lookingInfo = new LookingInfo()
                    {
                        CharacterId = characterCalling,
                        ExpirationTimer = new Timer(7200000)
                    };

                    lookingInfo.ExpirationTimer.Start();
                    lookingInfo.ExpirationTimer.Elapsed += (sender, e) => { LookingOver(sender, e, lookingInfo); };
                    lookingInfo.ExpirationTimer.AutoReset = false;

                    LookingInformation.Add(lookingInfo);
                }
                else
                {

                }
            }
            else
            {
                messages.Add("The bot couldn't set you to looking. If you are not already on the looking list (Which you can see by typing !look), please contact [user]Mayank[/user].");
            }

            return messages;
        }

        private void LookingOver(Object source, System.Timers.ElapsedEventArgs e, LookingInfo lookingInfo)
        {
            Plugin.FChatClient.SendPrivateMessage("Your looking status has expired. If you want to renew it, please type !looking in the room once again!", lookingInfo.CharacterId);
            LookingInformation.RemoveAll(x => x.CharacterId == lookingInfo.CharacterId);
        }

        public async new void ExecutePrivateCommand(string characterCalling, IEnumerable<string> args)
        {
            var result = await Execute(characterCalling, args);
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
