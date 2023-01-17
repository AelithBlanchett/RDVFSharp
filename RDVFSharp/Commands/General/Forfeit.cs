using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Forfeit : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Forfeits an ongoing fight.";

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.GetCurrentBattlefield(channel).IsInProgress)
            {
                var battlefield = Plugin.GetCurrentBattlefield(channel);
                var activeFighter = battlefield.GetFighter(character);
                if (activeFighter != null)
                {
                    activeFighter.HP = 0;
                    activeFighter.IsDead = true;
                    activeFighter.IsStunned = 56374621;

                    if (battlefield.TurnOrder[battlefield.currentFighter] == activeFighter)
                    {
                        battlefield.NextFighter();
                    }    

                    Plugin.FChatClient.SendMessageInChannel($"{activeFighter.Name} has forfeited the match.", channel);
                    battlefield.CheckIfFightIsOver();

                    if (battlefield.RemainingTeams > 1)
                    {
                        battlefield.CheckTargetCoherenceAndReassign();
                        battlefield.OutputFighterStatuses();
                        battlefield.OutputController.Action.Add("Forfeit");
                        battlefield.OutputController.Hit.Add($"{activeFighter.Name} is out of commission!");
                        battlefield.OutputController.Broadcast(battlefield); 
                    }
                }
                else
                {
                    Plugin.FChatClient.SendMessageInChannel("A fight that you are not participating in is already in progress", channel);
                }
            }



            else
            {
                Plugin.FChatClient.SendMessageInChannel("There is no match going on right now", channel);
            }
        }
    }
}
