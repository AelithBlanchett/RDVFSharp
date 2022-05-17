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
    public class Ready : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Sets a player as ready.";

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.CurrentBattlefield.IsInProgress)
            {
                throw new FightInProgress();
            }
            else if (Plugin.CurrentBattlefield.Fighters.Any(x => x.Name == character))
            {
                throw new FighterAlreadyExists(character);
            }

            BaseFighter fighter = null;

            fighter = await Plugin.DataContext.Fighters.FindAsync(character);

            if (fighter == null)
            {
                throw new FighterNotRegistered(character);
            }

            var teamInputText = string.Join(" ", args);

            var teamColor = "";
            if (string.IsNullOrEmpty(teamInputText.Trim()))
            {
                teamColor = Plugin.CurrentBattlefield.Fighters.Count % 2 == 0 ? "red" : "blue";
            }
            else if (teamInputText.ToLower().Contains("red"))
            {
                teamColor = "red";
            }
            else if (teamInputText.ToLower().Contains("blue"))
            {
                teamColor = "blue";
            }
            else if (teamInputText.ToLower().Contains("yellow"))
            {
                teamColor = "yellow";
            }
            else if (teamInputText.ToLower().Contains("purple"))
            {
                teamColor = "purple";
            }
            else
            {
                throw new Exception("Invalid team color.");
            }

            if (!Plugin.CurrentBattlefield.Fighters.Any(x => x.Name == fighter.Name))
            {
                Plugin.CurrentBattlefield.AddFighter(fighter, teamColor);
                Plugin.FChatClient.SendMessageInChannel($"{fighter.Name} joined the fight for team [color={teamColor}]{teamColor.ToUpper()}[/color]!", channel);
            }
        }
    }
}
