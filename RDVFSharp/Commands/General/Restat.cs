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

        public async Task Execute(string character, IEnumerable<string> args, string channel = "")
        {
            BaseFighter fighter;

            fighter = await Plugin.DataContext.Fighters.FindAsync(character);

            if (fighter == null)
            {
                Plugin.FChatClient.SendMessageInChannel("You are not registered. Please register with the bot first using the !register command. Example: !register 5 8 8 1 2", channel);
                return;
            }

            int[] statsArray;
            try
            {
                statsArray = Array.ConvertAll(args.ToArray(), int.Parse);
            }
            catch (Exception)
            {
                Plugin.FChatClient.SendMessageInChannel("Invalid arguments. All stats must be numbers. Example: !restat 5 8 8 1 2", channel);
                return;
            }

            fighter.Strength = statsArray[0];
            fighter.Dexterity = statsArray[1];
            fighter.Resilience = statsArray[2];
            fighter.Spellpower = statsArray[3];
            fighter.Willpower = statsArray[4];

            if (fighter.AreStatsValid)
            {
                Plugin.DataContext.Fighters.Update(fighter);
                Plugin.DataContext.SaveChanges();
                if (channel == "")
                {
                    Plugin.FChatClient.SendPrivateMessage($"You've successfully moved points among your stats, {character}.", character);
                    Plugin.FChatClient.SendPrivateMessage(fighter.Stats, character);
                }
                else
                {
                    Plugin.FChatClient.SendMessageInChannel($"You've successfully moved points among your stats, {character}.", channel);
                    Plugin.FChatClient.SendPrivateMessage(fighter.Stats, character);
                }

            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("Your stats are invalid. Please ensure they add up to 24 in total, and none exceed the range for each stat (0-10)", channel);
            }
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
