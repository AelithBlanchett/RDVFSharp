using FChatSharpLib.Entities.Plugin;
using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDVFSharp.Commands
{
    public class GetStats : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Displays someone else's stats.";

        public override void ExecuteCommand(string character ,IEnumerable<string> args, string channel)
        {
            if(!Plugin.FChatClient.IsUserAdmin(character, channel))
            {
                throw new UnauthorizedAccessException($"You don't have access to the {nameof(GetStats)} command.");
            }

            var cmd = new Stats()
            {
                Plugin = Plugin
            };

            cmd.ExecuteCommand(string.Join(" ", args), null, channel);
        }
    }
}
