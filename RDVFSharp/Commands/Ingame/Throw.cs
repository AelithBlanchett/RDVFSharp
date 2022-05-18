using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Throw : Action
    {
        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            var attacker = Plugin.CurrentBattlefield.GetActor();
            var target = Plugin.CurrentBattlefield.GetTarget();

            if (attacker.IsGrappling(target) || target.IsGrappling(attacker))
            {
                base.ExecuteCommand(character, args, channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You can only use Throw if you are grappling.", Plugin.Channel);
            }
        }
    }
}
