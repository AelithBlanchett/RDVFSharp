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
                var activeFighter = Plugin.GetCurrentBattlefield(channel).GetFighter(character);
                if (activeFighter != null)
                {
                    Plugin.FChatClient.SendMessageInChannel($"{activeFighter.Name} has forfeited the match.", channel);
                    Plugin.GetCurrentBattlefield(channel).EndFight(Plugin.GetCurrentBattlefield(channel).GetFighterTarget(character), activeFighter);
                }
                else
                {
                    throw new FightInProgress();
                }
            }
        }
    }
}
