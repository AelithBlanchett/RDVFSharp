using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Grab : Action
    {
        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (!Plugin.CurrentBattlefield.GetTarget().IsRestrained && (Plugin.CurrentBattlefield.InGrabRange || Plugin.CurrentBattlefield.GetTarget().IsExposed > 0))
            {
                base.ExecuteCommand(character, args, channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You can only use Grab if you are not already grappling your opponent and if either you are in grappling range or your opponent is Exposed.", Plugin.Channel);
            }
        }
    }
}
