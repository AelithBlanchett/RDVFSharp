using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Stats : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Displays your own stats.";

        public async Task Execute(string character, IEnumerable<string> args, string channel = "")
        {
            var fighter = await Plugin.DataContext.Fighters.FindAsync(character);
            if (fighter == null) { throw new FighterNotRegistered(character); }

            if (channel.ToLower().StartsWith("adh-"))
            {
                channel = character;
            }

            Plugin.FChatClient.SendPrivateMessage(fighter.Stats, channel);
        }

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            await this.Execute(character, args, channel);
        }

        public override async Task ExecutePrivateCommand(string characterCalling, IEnumerable<string> args)
        {
            await this.Execute(characterCalling, args);
        }
    }
}
