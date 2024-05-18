using FChatSharpLib.Entities.Plugin;
using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class SeeStats : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Displays someone else's stats.";

        public override async Task ExecuteCommand(string character ,IEnumerable<string> args, string channel)
        {
            if (!(character == Constants.MayankAdmin || character == Constants.EliseAdmin || character == Constants.AelithAdmin))
            {
                Plugin.FChatClient.SendMessageInChannel("You do not have access to this command", channel);
            }


            var characterName = string.Join(' ', args);


            var fighter = await Plugin.DataContext.Fighters.FindAsync(characterName);

            if (fighter == null)
            {
                Plugin.FChatClient.SendMessageInChannel("This character is not registered. Please register with the bot first using the !register command. Example: !register 5 8 8 1 2", channel);
                return;
            }

            Plugin.FChatClient.SendPrivateMessage(fighter.Stats, character);
        }
    }
}
