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

        public void Execute(string character ,IEnumerable<string> args, string channel = "")
        {
            using (var context = Plugin.Context)
            {
                var fighter = context.Fighters.Find(character);
                if (fighter != null)
                {
                    throw new FighterAlreadyExists(character);
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
                    throw new ArgumentException("Invalid arguments. All stats must be numbers. Example: !register 5 8 8 1 2");
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
                    context.Fighters.Add(createdFighter);
                    context.SaveChanges();
                    if(channel == "")
                    {
                        Plugin.FChatClient.SendPrivateMessage($"Welcome among us, {character}!", character);
                    }
                    else
                    {
                        Plugin.FChatClient.SendMessageInChannel($"Welcome among us, {character}!", channel);
                    }
                    
                }
                else
                {
                    throw new Exception(string.Join(", ", createdFighter.GetStatsErrors()));
                }
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
