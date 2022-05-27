using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;
using System.Linq;

namespace RDVFSharp.Commands
{
    public class SetWinner : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Sets the winner of an ongoing fight.";

        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.FChatClient.IsUserAdmin(character, channel) && Plugin.CurrentBattlefield.IsInProgress)
            {
                var activeFighter = Plugin.CurrentBattlefield.GetFighter(string.Join(' ', args));
                if (activeFighter != null)
                {
                    Plugin.FChatClient.SendMessageInChannel($"{activeFighter.Name} has won the match.", channel);
                    Plugin.CurrentBattlefield.EndFight(activeFighter, Plugin.CurrentBattlefield.GetFighterTarget(activeFighter.Name));
                }
                else
                {
                    throw new FighterNotFound(args.FirstOrDefault());
                }
            }
        }
    }
}
