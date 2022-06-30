﻿using FChatSharpLib.Entities.Plugin;
using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RDVFSharp.Commands
{
    public class Ready : BaseCommand<RDVFPlugin>
    {
        public Timer ReadyTimer = new Timer(30000);
        public override string Description => "Sets a player as ready.";

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (channel == "ADH-a823a4e998a2b3d31794")
            {
                Plugin.FChatClient.SendMessageInChannel($"[icon]Mayank[/icon][eicon]flipnewshyperbap[/eicon][icon]{character}[/icon] [i]\"No fighting in the bar!\"[/i]", channel);
            }

            else
            { 
                if (Plugin.GetCurrentBattlefield(channel).IsInProgress)
                {
                    throw new FightInProgress();
                }
                else if (Plugin.GetCurrentBattlefield(channel).Fighters.Any(x => x.Name == character))
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
                    teamColor = Plugin.GetCurrentBattlefield(channel).Fighters.Count % 2 == 0 ? "red" : "blue";
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

                if (!Plugin.GetCurrentBattlefield(channel).Fighters.Any(x => x.Name == fighter.Name))
                {
                    Plugin.GetCurrentBattlefield(channel).AddFighter(fighter, teamColor);
                    Plugin.FChatClient.SendMessageInChannel($"{fighter.Name} joined the fight for team [color={teamColor}]{teamColor.ToUpper()}[/color]!", channel);
                    ReadyTimer.Start();
                    ReadyTimer.Elapsed += Readyover;
                    ReadyTimer.AutoReset = false;
                    void Readyover(Object source, System.Timers.ElapsedEventArgs e)
                    {
                        var activeFighter = Plugin.GetCurrentBattlefield(channel).GetFighter(character);
                        if (!Plugin.GetCurrentBattlefield(channel).IsInProgress)
                        {
                            Plugin.GetCurrentBattlefield(channel).Fighters.RemoveAll(x => x.Name == character);
                            Plugin.FChatClient.SendMessageInChannel($"{character} has been removed from the upcoming fight. If you have an opponent, please ready again!", channel);   
                        }
                    }

                }
            }
        }
    }
}
