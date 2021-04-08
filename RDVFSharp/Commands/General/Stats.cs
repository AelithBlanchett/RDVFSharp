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
            using (var context = Plugin.Context)
            {
                var fighter = context.Fighters.Find(character);
                if (fighter == null) { throw new FighterNotRegistered(character); }

                if (channel.ToLower().StartsWith("adh-"))
                {
                    channel = character;
                }

                Plugin.FChatClient.SendPrivateMessage(fighter.Stats, channel);
            }
        }
    }
}
