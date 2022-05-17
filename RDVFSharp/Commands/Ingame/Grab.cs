using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Grab : Action
    {
        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            var attacker = Plugin.CurrentBattlefield.GetActor();
            var target = Plugin.CurrentBattlefield.GetTarget();

            if (!Plugin.CurrentBattlefield.GetTarget().IsRestrained && (((Plugin.CurrentBattlefield.GetTarget().IsGrabbable==Plugin.CurrentBattlefield.GetActor().IsGrabbable) && (0 < Plugin.CurrentBattlefield.GetActor().IsGrabbable)) || Plugin.CurrentBattlefield.GetTarget().IsExposed > 0 || target.IsGrappling(attacker)))
            {
                base.ExecuteCommand(character, args, channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You can only use Grab if your opponent is not already grappled and if either you are in grappling range or your opponent is Exposed.", Plugin.Channel);
            }
        }
    }
}
