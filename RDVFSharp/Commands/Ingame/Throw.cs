using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Throw : Action
    {
        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.CurrentBattlefield.GetActor().IsRestrained || Plugin.CurrentBattlefield.GetTarget().IsRestrained)
            {
                base.ExecuteCommand(character, args, channel);
            }
        }
    }
}
