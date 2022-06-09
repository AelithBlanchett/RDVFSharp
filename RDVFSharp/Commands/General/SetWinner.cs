using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class SetWinner : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Sets the winner of an ongoing fight.";

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.FChatClient.IsUserAdmin(character, channel) && Plugin.GetCurrentBattlefield(channel).IsInProgress)
            {
                var activeFighter = Plugin.GetCurrentBattlefield(channel).GetFighter(string.Join(' ', args));
                if (activeFighter != null)
                {
                    Plugin.FChatClient.SendMessageInChannel($"{activeFighter.Name} has won the match.", channel);
                    Plugin.GetCurrentBattlefield(channel).EndFight(activeFighter, Plugin.GetCurrentBattlefield(channel).GetFighterTarget(activeFighter.Name));
                }
                else
                {
                    throw new FighterNotFound(args.FirstOrDefault());
                }
            }
        }
    }
}
