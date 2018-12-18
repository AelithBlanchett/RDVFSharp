using FChatSharpLib.Entities.Plugin;
using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.Commands
{
    public class Reset : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Resets the current fight.";

        public override void ExecuteCommand(string character ,IEnumerable<string> args, string channel)
        {
            if(Plugin.FChatClient.IsUserAdmin(character, channel))
            {
                Plugin.ResetFight();
                Plugin.FChatClient.SendMessageInChannel($"The fight has been reset.", channel);
            }
        }
    }
}
