using FChatSharpLib.Entities.Plugin;
using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDVFSharp.Commands
{
    public class Start : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Starts the fight.";

        public override void ExecuteCommand(string character ,IEnumerable<string> args, string channel)
        {
            if (Plugin.CurrentBattlefield.IsInProgress)
            {
                throw new FightInProgress();
            }
            else if (!Plugin.CurrentBattlefield.Fighters.Any(x => x.Name == character))
            {
                throw new FighterNotFound(character);
            }

            if (!Plugin.CurrentBattlefield.IsInProgress && Plugin.CurrentBattlefield.Fighters.Count >= 2)
            {
                Plugin.FChatClient.SendMessageInChannel($"Let's get it on!", channel);
                Plugin.FChatClient.SendMessageInChannel(Constants.VCAdvertisement, channel);
                Plugin.CurrentBattlefield.InitialSetup();
            }
        }
    }
}
