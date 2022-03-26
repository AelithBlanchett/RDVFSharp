using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Throw : Action
    {
        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.CurrentBattlefield.GetActor().IsRestraining && Plugin.CurrentBattlefield.GetTarget().IsRestrained)
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
