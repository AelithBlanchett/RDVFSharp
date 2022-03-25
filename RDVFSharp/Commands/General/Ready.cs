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
    public class Ready : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Sets a player as ready.";

        public override void ExecuteCommand(string character ,IEnumerable<string> args, string channel)
        {
            if (Plugin.CurrentBattlefield.IsInProgress)
            {
                throw new FightInProgress();
            }
            else if (Plugin.CurrentBattlefield.Fighters.Any(x => x.Name == character))
            {
                throw new FighterAlreadyExists(character);
            }

            BaseFighter fighter = null;

            using (var context = Plugin.Context)
            {
                fighter = context.Fighters.Find(character);
            }

            if (fighter == null)
            {
                throw new FighterNotRegistered(character);
            }

            var actualFighter = new Fighter(fighter, Plugin.CurrentBattlefield);
            var optionalAlly = string.Join(" ", args);

            //TODO process optionalAlly to construct teams
            if(!Plugin.CurrentBattlefield.Fighters.Any(x => x.Name == actualFighter.Name))
            {
                Plugin.CurrentBattlefield.Fighters.Add(actualFighter);
                Plugin.FChatClient.SendMessageInChannel($"{actualFighter.Name} joined the fight!", channel);
            }

            if (!Plugin.CurrentBattlefield.IsInProgress && Plugin.CurrentBattlefield.Fighters.Count >= 2)
            {
                Plugin.FChatClient.SendMessageInChannel($"{actualFighter.Name} accepted the challenge! Let's get it on!", channel);
                Plugin.FChatClient.SendMessageInChannel(Constants.VCAdvertisement, channel);
                Plugin.CurrentBattlefield.InitialSetup(Plugin.CurrentBattlefield.FirstFighter, Plugin.CurrentBattlefield.SecondFighter);
            }
        }
    }
}
