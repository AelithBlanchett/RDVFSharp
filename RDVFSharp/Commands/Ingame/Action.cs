using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public abstract class Action : BaseCommand<RendezvousFighting>
    {
        public override string Description => $"{GetType().Name} attack";

        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.CurrentBattlefield.IsAbleToAttack(character))
            {
                Plugin.CurrentBattlefield.TakeAction(GetType().Name);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("This is not your turn.", Plugin.Channel);
            }
        }
    }
}
