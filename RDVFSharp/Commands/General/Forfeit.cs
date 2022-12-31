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
                    Plugin.GetCurrentBattlefield(channel).GetFighter(character).HP = 0;
                    Plugin.FChatClient.SendMessageInChannel($"{activeFighter.Name} has forfeited the match.", channel);
                    Plugin.GetCurrentBattlefield(channel).GetFighter(character).UpdateCondition();
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
