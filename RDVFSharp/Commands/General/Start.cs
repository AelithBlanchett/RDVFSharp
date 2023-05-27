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
    public class Start : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Starts the fight.";

        public override async Task ExecuteCommand(string character ,IEnumerable<string> args, string channel)
        {
            if (Plugin.GetCurrentBattlefield(channel).IsInProgress)
            {
                Plugin.FChatClient.SendMessageInChannel("A fight that you are not participating in is already in progress", channel);
                return;
            }
            else if (!Plugin.GetCurrentBattlefield(channel).Fighters.Any(x => x.Name == character))
            {
                Plugin.FChatClient.SendMessageInChannel("You have not readied up!", channel);
                return;
            }

            if (!Plugin.GetCurrentBattlefield(channel).IsInProgress && Plugin.GetCurrentBattlefield(channel).Fighters.Count >= 2)
            {
                var StageInputText = string.Join(" ", args);

                Plugin.FChatClient.SendMessageInChannel($"Let's get it on!", channel);
                Plugin.FChatClient.SendMessageInChannel(Constants.VCAdvertisement, channel);
                if (string.IsNullOrEmpty(StageInputText.Trim()))
                {
                    Plugin.GetCurrentBattlefield(channel).InitialSetup();
                }

                else
                {
                    Plugin.GetCurrentBattlefield(channel).StageSelectedSetup();
                    Plugin.GetCurrentBattlefield(channel).OutputController.Hit.Add("Game started!");
                    Plugin.GetCurrentBattlefield(channel).OutputController.Hit.Add("FIGHTING STAGE: " + StageInputText + " - " + Plugin.GetCurrentBattlefield(channel).GetActor().Name + " goes first!");
                    Plugin.GetCurrentBattlefield(channel).BroadcastStart();
                }
                Ready.ReadyTimer.Stop();

                if (channel == Constants.RDVFArena)
                {
                    Plugin.FChatClient.SendMessageInChannel($"A fight has started in the [session]{Constants.RDVFArena}[/session]!", Constants.RDVFBar);
                }

                else if (channel == Constants.RDVFVenue)
                {
                    Plugin.FChatClient.SendMessageInChannel($"A fight has started in the [session]{Constants.RDVFVenue}[/session]!", Constants.RDVFBar);
                }
            }
        }
    }
}
