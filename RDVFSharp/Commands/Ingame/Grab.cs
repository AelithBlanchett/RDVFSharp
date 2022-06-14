using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Grab : Action
    {
        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            var attacker = Plugin.GetCurrentBattlefield(channel).GetActor();
            var target = Plugin.GetCurrentBattlefield(channel).GetTarget();

            if (!Plugin.GetCurrentBattlefield(channel).GetTarget().IsRestrained && (((Plugin.GetCurrentBattlefield(channel).GetTarget().IsGrabbable==Plugin.GetCurrentBattlefield(channel).GetActor().IsGrabbable) && (0 < Plugin.GetCurrentBattlefield(channel).GetActor().IsGrabbable)) || Plugin.GetCurrentBattlefield(channel).GetTarget().IsExposed > 0 || target.IsGrappling(attacker)))
            {
                base.ExecuteCommand(character, args, channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You can only use Grab if your opponent is not already grappled and if either you are in grappling range or your opponent is Exposed.", channel);
            }
        }
    }
}
