using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Throw : Action
    {
        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            var attacker = Plugin.GetCurrentBattlefield(channel).GetActor();
            var target = Plugin.GetCurrentBattlefield(channel).GetTarget();

            if (attacker.IsGrappling(target))
            {
                await base.ExecuteCommand(character, args, channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You can only use Throw if you are grappling the opponent.", channel);
            }
        }
    }
}
