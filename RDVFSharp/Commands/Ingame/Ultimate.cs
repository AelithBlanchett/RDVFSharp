
namespace RDVFSharp.Commands
{
    public class Ultimate : Action
    {
        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (!Plugin.CurrentBattlefield.InGrabRange)
            {
                base.ExecuteCommand(character, args, channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You have already used Ultimate once in this match!", Plugin.Channel);
            }
        }
    }
}
