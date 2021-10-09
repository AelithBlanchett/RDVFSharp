using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Curse : Action
    {
        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (!Plugin.CurrentBattlefield.InGrabRange)
            {
                base.ExecuteCommand(character, args, channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You have already used Curse once in this match!", Plugin.Channel);
            }
        }
    }
}
