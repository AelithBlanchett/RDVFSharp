using FChatSharpLib.Entities.Plugin;
using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.Commands
{
    public class Ready : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Sets a player as ready.";

        public override void ExecuteCommand(string character ,IEnumerable<string> args, string channel)
        {
            if (Plugin.CurrentBattlefield.IsActive || (Plugin.FirstFighter != null && Plugin.SecondFighter != null))
            {
                throw new FightInProgress();
            }
            else if (Plugin.FirstFighter?.Name == character || Plugin.SecondFighter?.Name == character)
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

            if(Plugin.FirstFighter == null && Plugin.SecondFighter != null)
            {
                Plugin.FirstFighter = Plugin.SecondFighter;
                Plugin.SecondFighter = null;
            }

            if (Plugin.FirstFighter == null)
            {
                Plugin.FirstFighter = actualFighter;
                Plugin.FChatClient.SendMessageInChannel($"{actualFighter.Name} joined the fight!", channel);
            }
            else
            {
                Plugin.SecondFighter = actualFighter;

                if (!Plugin.CurrentBattlefield.IsActive && (Plugin.FirstFighter != null && Plugin.SecondFighter != null))
                {
                    Plugin.FChatClient.SendMessageInChannel($"{actualFighter.Name} accepted the challenge! Let's get it on!", channel);
                    Plugin.FChatClient.SendMessageInChannel(Constants.VCAdvertisement, channel);
                    Plugin.CurrentBattlefield.InitialSetup(Plugin.FirstFighter, Plugin.SecondFighter);
                }
            }

        }
    }
}
