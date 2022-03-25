using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Status : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Gets the status of an ongoing fight.";

        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.CurrentBattlefield.IsInProgress)
            {
                Plugin.FChatClient.SendPrivateMessage(Plugin.CurrentBattlefield.OutputController.LastMessageSent, character);
            }
            else
            {
                Plugin.FChatClient.SendPrivateMessage("There's no match going on right now.", character);
            }
        }
    }
}
