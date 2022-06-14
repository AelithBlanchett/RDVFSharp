using FChatSharpLib.Entities.Plugin;
using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Reset : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Resets the current fight.";

        public override async Task ExecuteCommand(string character ,IEnumerable<string> args, string channel)
        {
            if(Plugin.FChatClient.IsUserAdmin(character, channel))
            {
                Plugin.ResetFight(channel);
                Plugin.FChatClient.SendMessageInChannel($"The fight has been reset.", channel);
            }
        }
    }
}
