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
    public class Register : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Registers a player in the game.";

        public async Task Execute(string character, IEnumerable<string> args, string channel = "")
        {
            var fighter = await Plugin.DataContext.Fighters.FindAsync(character);
            if (fighter != null)
            {
                Plugin.FChatClient.SendMessageInChannel("You are already registered!", channel);
                return;
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
                Plugin.FChatClient.SendMessageInChannel("Invalid arguments. All stats must be numbers. Example: !register 5 8 8 1 2", channel);
                return;
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

            if (createdFighter.AreStatsValid)
            {
                Plugin.DataContext.Fighters.Add(createdFighter);
                Plugin.DataContext.SaveChanges();

                if (channel == "")
                {
                    Plugin.FChatClient.SendPrivateMessage($"Welcome among us, {character}!", character);
                    Plugin.FChatClient.SendPrivateMessage(createdFighter.Stats, character);
                }
                else
                {
                    Plugin.FChatClient.SendMessageInChannel($"Welcome among us, {character}!", channel);
                    Plugin.FChatClient.SendPrivateMessage(createdFighter.Stats, character);
                }

            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel($"There was an error registering your character. Please check that you have used 24 points: And that each stat is assigned a number from 0-10", channel);
            }
        }

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            await this.Execute(character, args, channel);
        }
    }
}
