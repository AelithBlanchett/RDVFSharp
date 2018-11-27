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
    public class Stats : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Displays your own stats.";

        public override void ExecuteCommand(string character ,string[] args, string channel)
        {
            var fighter = Plugin.Context.Fighters.Find(character);
            if(fighter == null){throw new FighterNotFound(character);}

            Plugin.FChatClient.SendMessageInChannel(fighter.Stats, channel);
        }
    }
}
