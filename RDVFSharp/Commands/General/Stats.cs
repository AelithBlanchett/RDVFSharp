using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Stats : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Displays your own stats.";

        public void Execute(string character, IEnumerable<string> args, string channel = "")
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

        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            this.Execute(character, args, channel);
        }

        public override void ExecutePrivateCommand(string characterCalling, IEnumerable<string> args)
        {
            this.Execute(characterCalling, args);
        }
    }
}
