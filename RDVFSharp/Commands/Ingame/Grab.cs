namespace RDVFSharp.Commands
{
    public class Grab : Action
    {
        public override void ExecuteCommand(string character, string[] args, string channel)
        {
            if (Plugin.CurrentBattlefield.GetTarget().IsRestrained && (Plugin.CurrentBattlefield.InGrabRange || Plugin.CurrentBattlefield.GetTarget().IsExposed > 0))
            {
                base.ExecuteCommand(character, args, channel);
            }
        }
    }
}
