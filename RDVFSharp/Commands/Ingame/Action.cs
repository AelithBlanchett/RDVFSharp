using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public abstract class Action : BaseCommand<RDVFPlugin>
    {
        public override string Description => $"{GetType().Name} attack";

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            var attacker = Plugin.GetCurrentBattlefield(channel).GetActor();
            var target = Plugin.GetCurrentBattlefield(channel).GetTarget();
            
            if ((attacker.IsRestrained == true && !target.IsGrappling(attacker)) || (attacker.IsRestraining > 0 && !attacker.IsGrappling(target)))
            {
                Plugin.FChatClient.SendMessageInChannel("You must be targetting the one that is grappling you, or that you are grappling.", channel);
            }
            
            else if ((Plugin.GetCurrentBattlefield(channel).IsAbleToAttack(character)) && !((Plugin.GetCurrentBattlefield(channel).GetActor().IsRestrained == true) && (Plugin.GetCurrentBattlefield(channel).GetTarget().IsRestraining == 0)))
            {
                Plugin.GetCurrentBattlefield(channel).TakeAction(GetType().Name);
            }

            else
            {
                Plugin.FChatClient.SendMessageInChannel("This is not your turn.", channel);
            }

        }
    }
}
