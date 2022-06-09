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
    public class Start : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Starts the fight.";

        public override async Task ExecuteCommand(string character ,IEnumerable<string> args, string channel)
        {
            if (Plugin.GetCurrentBattlefield(channel).IsInProgress)
            {
                throw new FightInProgress();
            }
            else if (!Plugin.GetCurrentBattlefield(channel).Fighters.Any(x => x.Name == character))
            {
                throw new FighterNotFound(character);
            }

            if (!Plugin.GetCurrentBattlefield(channel).IsInProgress && Plugin.GetCurrentBattlefield(channel).Fighters.Count >= 2)
            {
                Plugin.FChatClient.SendMessageInChannel($"Let's get it on!", channel);
                Plugin.FChatClient.SendMessageInChannel(Constants.VCAdvertisement, channel);
                Plugin.GetCurrentBattlefield(channel).InitialSetup();
            }
        }
    }
}
