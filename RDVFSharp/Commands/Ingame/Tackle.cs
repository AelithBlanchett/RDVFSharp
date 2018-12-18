using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Tackle : Action
    {
        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (!Plugin.CurrentBattlefield.InGrabRange)
            {
                base.ExecuteCommand(character, args, channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You can't use Tackle when you already are in grappling range.", Plugin.Channel);
            }
        }
    }
}
