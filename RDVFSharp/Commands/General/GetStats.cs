using FChatSharpLib.Entities.Plugin;
using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class GetStats : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Displays someone else's stats.";

        public override async Task ExecuteCommand(string character ,IEnumerable<string> args, string channel)
        {
            if(!Plugin.FChatClient.IsUserAdmin(character, channel))
            {
                Plugin.FChatClient.SendMessageInChannel("You do not have access to this command", channel);
            }

            var cmd = new Stats()
            {
                Plugin = Plugin
            };

            cmd.ExecuteCommand(string.Join(" ", args), null, channel);
        }
    }
}
