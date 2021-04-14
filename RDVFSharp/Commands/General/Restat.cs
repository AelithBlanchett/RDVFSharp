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
    public class Restat : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Restats a player in the game.";

        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            BaseFighter fighter;

            using (var context = Plugin.Context)
            {
                fighter = context.Fighters.Find(character);


                if (fighter == null)
                {
                    throw new FighterNotRegistered(character);
                }

                int[] statsArray;

                try
                {
                    statsArray = Array.ConvertAll(args.ToArray(), int.Parse);
                }
                catch (Exception)
                {
                    throw new ArgumentException("Invalid arguments. All stats must be numbers. Example: !restat 5 8 8 1 2");
                }

                fighter.Strength = statsArray[0];
                fighter.Dexterity = statsArray[1];
                fighter.Resilience = statsArray[2];
                fighter.Endurance = statsArray[3];
                fighter.Special = statsArray[4];

                if (fighter.AreStatsValid)
                {
                    context.Fighters.Update(fighter);
                    context.SaveChanges();
                    Plugin.FChatClient.SendMessageInChannel($"You've successfully moved points among your stats, {character}.", channel);
                }
                else
                {
                    throw new Exception(string.Join(", ", fighter.GetStatsErrors()));
                }
            }
        }
    }
}
