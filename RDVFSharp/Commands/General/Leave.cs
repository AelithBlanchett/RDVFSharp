using FChatSharpLib.Entities.Plugin;
using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Leave : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Leaves an ongoing fight.";

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.GetCurrentBattlefield(channel).IsInProgress)
            {
                var activeFighter = Plugin.GetCurrentBattlefield(channel).GetFighter(character);
                if (activeFighter == null)
                {
                    Plugin.FChatClient.SendMessageInChannel("A fight that you are not participating in is already in progress", channel);
                    return;
                }
                
                activeFighter.WantsToLeave = true;
                Plugin.FChatClient.SendMessageInChannel($"The fight will end once all the fighters in this match type !leave.", channel);
                if (Plugin.GetCurrentBattlefield(channel).Fighters.TrueForAll(x => x.WantsToLeave))
                {
                    Plugin.ResetFight(channel);
                    Plugin.FChatClient.SendMessageInChannel($"The fight has been reset.", channel);
                }
                
            }
            else
            {
                var removed = false;
                int result = Plugin.GetCurrentBattlefield(channel).Fighters.RemoveAll(x => x.Name == character);
                removed = result > 0;

                if (removed)
                {
                    Plugin.FChatClient.SendMessageInChannel($"You've successfully been removed from the upcoming fight.", channel);
                    Ready.ReadyTimer.Stop();
                }
            }


        }
    }
}