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
    public class Leave : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Leaves an ongoing fight.";

        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.CurrentBattlefield.IsInProgress)
            {
                var activeFighter = Plugin.CurrentBattlefield.GetFighter(character);
                if (activeFighter != null)
                {
                    activeFighter.WantsToLeave = true;
                    Plugin.FChatClient.SendMessageInChannel($"The fight will end if your opponent types !exit too.", channel);
                }
                else
                {
                    throw new FightInProgress();
                }

                if (Plugin.CurrentBattlefield.Fighters.TrueForAll(x => x.WantsToLeave))
                {
                    Plugin.ResetFight();
                    Plugin.FChatClient.SendMessageInChannel($"The fight has been reset.", channel);
                }
            }
            else
            {
                var removed = false;
                int result = Plugin.CurrentBattlefield.Fighters.RemoveAll(x => x.Name == character);
                removed = result > 0;

                if (removed)
                {
                    Plugin.FChatClient.SendMessageInChannel($"You've successfully been removed from the upcoming fight.", channel);
                }
            }
            

        }
    }
}
