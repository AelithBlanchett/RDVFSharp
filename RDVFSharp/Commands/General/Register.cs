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
    public class Register : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Registers a player in the game.";

        public override void ExecuteCommand(string character ,IEnumerable<string> args, string channel)
        {
            var fighter = Plugin.Context.Fighters.Find(character);
            if(fighter != null)
            {
                throw new FighterAlreadyExists(character);
            }

            int[] statsArray;

            try
            {
                statsArray = Array.ConvertAll(args.ToArray(), int.Parse);

                if(statsArray.Length != 5)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid arguments. All stats must be numbers. Example: !register 5,8,8,1,2");
            }

            var createdFighter = new BaseFighter()
            {
                Name = character,
                Strength = statsArray[0],
                Dexterity = statsArray[1],
                Resilience = statsArray[2],
                Endurance = statsArray[3],
                Special = statsArray[4]
            };

            if (createdFighter.AreStatsValid)
            {
                Plugin.Context.Fighters.Add(createdFighter);
                Plugin.Context.SaveChanges();
                Plugin.FChatClient.SendMessageInChannel($"Welcome among us, {character}!", channel);
            }
            else
            {
                throw new Exception(string.Join(", ", createdFighter.GetStatsErrors()));
            }
        }
    }
}
