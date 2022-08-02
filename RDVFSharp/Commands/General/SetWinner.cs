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
                    Plugin.FChatClient.SendMessageInChannel("This fighter was not found. Please check the spelling of the fighter's name!", channel);
                }
            }
            else if (Plugin.GetCurrentBattlefield(channel).IsInProgress && !Plugin.FChatClient.IsUserAdmin(character, channel))
            {
                Plugin.FChatClient.SendMessageInChannel("You do not have access to this command", channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("There is no match going on right now", channel);
            }
        }
    }
}
