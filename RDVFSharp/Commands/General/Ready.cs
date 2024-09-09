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
using System.Timers;

namespace RDVFSharp.Commands
{
    public class Ready : BaseCommand<RDVFPlugin>
    {
        public static Timer ReadyTimer = new Timer(300000);
        public override string Description => "Sets a player as ready.";

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (channel == Constants.RDVFBar)
            {
                Plugin.FChatClient.SendMessageInChannel($"[eicon]Invertyank[/eicon][eicon]flipnewshyperbap[/eicon][icon]{character}[/icon] [i]\"No fighting in the bar!\"[/i]", channel);
            }

            else
            { 
                if (Plugin.GetCurrentBattlefield(channel).IsInProgress)
                {
                    Plugin.FChatClient.SendMessageInChannel("A fight that you are not participating in is already in progress", channel);
                    return;
                }
                else if (Plugin.GetCurrentBattlefield(channel).Fighters.Any(x => x.Name == character))
                {
                    Plugin.FChatClient.SendMessageInChannel("You have already readied up!", channel);
                    return;
                }

                BaseFighter fighter = null;

                fighter = await Plugin.DataContext.Fighters.FindAsync(character);

                if (fighter == null)
                {
                    Plugin.FChatClient.SendMessageInChannel("You are not registered. Please register with the bot first using the !register command. Example: !register 5 8 8 1 2", channel);
                    return;
                }
                else if ((fighter.Name == Constants.LeonDuChain) || (fighter.Name == Constants.CantTouchThis))
                {
                    Plugin.FChatClient.SendMessageInChannel("You have been banned: And as such, are no longer able to participate in fights with this bot.", channel);
                    return;
                }

                var teamInputText = string.Join(" ", args);

                var teamColor = "";
                if (string.IsNullOrEmpty(teamInputText.Trim()))
                {
                    if (Plugin.GetCurrentBattlefield(channel).TeamRed.Count == 0)
                    {
                        teamColor = "red";
                    }

                    else if (Plugin.GetCurrentBattlefield(channel).TeamBlue.Count == 0)
                    {
                        teamColor = "blue";
                    }

                    else if (Plugin.GetCurrentBattlefield(channel).TeamYellow.Count == 0)
                    {
                        teamColor = "yellow";
                    }

                    else if (Plugin.GetCurrentBattlefield(channel).TeamPurple.Count == 0)
                    {
                        teamColor = "purple";
                    }
                    
                    else
                    {
                        teamColor = Plugin.GetCurrentBattlefield(channel).Fighters.Count % 2 == 0 ? "red" : "blue";
                    }
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
                    Plugin.FChatClient.SendMessageInChannel("Invalid team color. Please pick between red/blue/yellow/purple", channel);
                    return;
                }

                if (!Plugin.GetCurrentBattlefield(channel).Fighters.Any(x => x.Name == fighter.Name))
                {
                    Plugin.GetCurrentBattlefield(channel).AddFighter(fighter, teamColor);
                    Plugin.FChatClient.SendMessageInChannel($"{fighter.Name} joined the fight for team [color={teamColor}]{teamColor.ToUpper()}[/color]!", channel);
                    Plugin.GetCurrentBattlefield(channel).JoinTeams();
                    ReadyTimer.Start();
                    ReadyTimer.Elapsed += Readyover;
                    ReadyTimer.AutoReset = false;
                    void Readyover(Object source, System.Timers.ElapsedEventArgs e)
                    {
                        if (!Plugin.GetCurrentBattlefield(channel).IsInProgress && Plugin.GetCurrentBattlefield(channel).Fighters.Any(x => x.Name == fighter.Name))
                        {
                            Plugin.GetCurrentBattlefield(channel).Fighters.RemoveAll(x => x.Name == character);
                            Plugin.FChatClient.SendMessageInChannel($"This fight has been reset. (You can only ready for up to 5 minutes before starting a fight. Please do not ready if you don't have an opponent!)", channel);   
                        }
                    }

                }
            }
        }
    }
}
