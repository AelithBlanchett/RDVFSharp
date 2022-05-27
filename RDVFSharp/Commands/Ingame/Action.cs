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
            var attacker = Plugin.CurrentBattlefield.GetActor();
            var target = Plugin.CurrentBattlefield.GetTarget();
            
            if ((attacker.IsRestrained == true && !target.IsGrappling(attacker)) || (attacker.IsRestraining > 0 && !attacker.IsGrappling(target)))
            {
                Plugin.FChatClient.SendMessageInChannel("You must be targetting the one that is grappling you, or that you are grappling.", Plugin.Channel);
            }
            
            else if ((Plugin.CurrentBattlefield.IsAbleToAttack(character)) && !((Plugin.CurrentBattlefield.GetActor().IsRestrained == true) && (Plugin.CurrentBattlefield.GetTarget().IsRestraining == 0)))
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
