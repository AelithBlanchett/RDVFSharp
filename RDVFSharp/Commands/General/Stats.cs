using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Stats : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Displays your own stats.";

        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            var fighter = Plugin.Context.Fighters.Find(character);
            if (fighter == null) { throw new FighterNotRegistered(character); }

            Plugin.FChatClient.SendMessageInChannel(fighter.Stats, channel);
        }
    }
}
