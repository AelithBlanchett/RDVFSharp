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
    public class Restat : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Restats a player in the game.";

        public async Task<string> Execute(string character, IEnumerable<string> args, string channel = "")
        {
            BaseFighter fighter;

            fighter = await Plugin.DataContext.Fighters.FindAsync(character);

            if (fighter == null)
            {
                return "You are not registered. Please register with the bot first using the !register command. Example: !register 5 8 8 1 2";
            }

            int[] statsArray;
            try
            {
                statsArray = Array.ConvertAll(args.ToArray(), int.Parse);
            }
            catch (Exception)
            {
                return "Invalid arguments. All stats must be numbers. Example: !restat 5 8 8 1 2";
            }

            var statErrors = BaseFighter.GetStatsErrors(statsArray[0], statsArray[1], statsArray[2], statsArray[3], statsArray[4]).JoinAsString("\n");
            if (string.IsNullOrEmpty(statErrors))
            {
                fighter.Strength = statsArray[0];
                fighter.Dexterity = statsArray[1];
                fighter.Resilience = statsArray[2];
                fighter.Spellpower = statsArray[3];
                fighter.Willpower = statsArray[4];

                Plugin.DataContext.Fighters.Update(fighter);
                Plugin.DataContext.SaveChanges();
                return $"Welcome among us, {character}!\n{fighter.Stats}";
            }
            else
            {
                return "Your stats are invalid. Please ensure they add up to 24 in total, and none exceed the range for each stat (0-10)";
            }
        }

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            var message = await this.Execute(character, args, channel);
            Plugin.FChatClient.SendMessageInChannel(message, channel);
        }

        public override async Task ExecutePrivateCommand(string characterCalling, IEnumerable<string> args)
        {
            var message = await this.Execute(characterCalling, args);
            Plugin.FChatClient.SendPrivateMessage(message, characterCalling);
        }
    }
}
