using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Status : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Gets the status of an ongoing fight.";

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.GetCurrentBattlefield(channel).IsInProgress)
            {
                Plugin.FChatClient.SendPrivateMessage(Plugin.GetCurrentBattlefield(channel).OutputController.LastMessageSent, character);
            }
            else
            {
                Plugin.FChatClient.SendPrivateMessage("There's no match going on right now.", character);
            }
        }
    }
}
