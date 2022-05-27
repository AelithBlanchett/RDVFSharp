using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Curse : Action
    {
        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            var attacker = Plugin.CurrentBattlefield.GetActor();

            if (attacker.CurseUsed == 0)
            {
                base.ExecuteCommand(character, args, channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You can only use Curse once per match.", Plugin.Channel);
            }
        }
    }
}
