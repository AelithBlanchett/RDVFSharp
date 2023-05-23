using FChatSharpLib.Entities.Plugin;
using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Register : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Registers a player in the game.";

        public async Task<string> Execute(string character, IEnumerable<string> args, string channel = "")
        {
            var fighter = await Plugin.DataContext.Fighters.FindAsync(character);
            if (fighter != null)
            {
                return "You are already registered!";
            }

            int[] statsArray;

            try
            {
                statsArray = Array.ConvertAll(args.ToArray(), int.Parse);

                if (statsArray.Length != 5)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                return "Invalid arguments. All stats must be numbers. Example: !register 5 8 8 1 2";
            }

            var createdFighter = new BaseFighter()
            {
                Name = character,
                Strength = statsArray[0],
                Dexterity = statsArray[1],
                Resilience = statsArray[2],
                Spellpower = statsArray[3],
                Willpower = statsArray[4]
            };

            if (string.IsNullOrEmpty(createdFighter.StatErrors))
            {
                Plugin.DataContext.Fighters.Add(createdFighter);
                Plugin.DataContext.SaveChanges();
                return $"Welcome among us, {character}!\n{createdFighter.Stats}";
            }
            else
            {
                return $"There was an error registering your character. Please check that you have used 24 points: And that each stat is assigned a number from 0-10";
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
