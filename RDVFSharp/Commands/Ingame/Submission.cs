namespace RDVFSharp.Commands
{
    public class Submission : Action
    {
        public override void ExecuteCommand(string character, string[] args, string channel)
        {
            if (Plugin.CurrentBattlefield.GetTarget().IsRestrained)
            {
                base.ExecuteCommand(character, args, channel);
            }
        }
    }
}
