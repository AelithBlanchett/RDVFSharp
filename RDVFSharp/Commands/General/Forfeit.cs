using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Forfeit : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Forfeits an ongoing fight.";

        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.CurrentBattlefield.IsActive)
            {
                var activeFighter = Plugin.CurrentBattlefield.GetFighter(character);
                if (activeFighter != null)
                {
                    Plugin.FChatClient.SendMessageInChannel($"{activeFighter.Name} has forfeited the match.", channel);
                    Plugin.CurrentBattlefield.EndFight(Plugin.CurrentBattlefield.GetFighterTarget(character), activeFighter);
                }
                else
                {
                    throw new FightInProgress();
                }
            }
        }
    }
}
