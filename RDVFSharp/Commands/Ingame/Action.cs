using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;

namespace RDVFSharp.Commands
{
    public abstract class Action : BaseCommand<RendezvousFighting>
    {
        public override string Description => $"{GetType().Name} attack";

        public override void ExecuteCommand(string character, string[] args, string channel)
        {
            if (Plugin.CurrentBattlefield.IsAbleToAttack(character))
            {
                Plugin.CurrentBattlefield.TakeAction(GetType().Name);
            }
        }
    }
}
