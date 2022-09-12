using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Status : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Gets the status of an ongoing fight.";

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (channel == Constants.RDVFBar)
            {
                var argsList = args.ToList();
                var NamedChannel = string.Join(" ", args);

                if (NamedChannel.ToLower() == "arena")
                {
                    Plugin.FChatClient.SendPrivateMessage(Plugin.GetCurrentBattlefield(Constants.RDVFArena).OutputController.LastMessageSent, character);
                }

                else if (NamedChannel.ToLower() == "venue")
                {
                    Plugin.FChatClient.SendPrivateMessage(Plugin.GetCurrentBattlefield(Constants.RDVFVenue).OutputController.LastMessageSent, character);
                }
                
                else
                {
                    if (Plugin.GetCurrentBattlefield(Constants.RDVFArena).IsInProgress && Plugin.GetCurrentBattlefield(Constants.RDVFVenue).IsInProgress)
                    {
                        Plugin.FChatClient.SendMessageInChannel("There are matches happening in both the arena and the venue! If you want to see the progress of either of those matches, type '!status Arena' or '!status Venue' in the bar!", channel);
                    }

                    else if (Plugin.GetCurrentBattlefield(Constants.RDVFArena).IsInProgress && !Plugin.GetCurrentBattlefield(Constants.RDVFVenue).IsInProgress)
                    {
                        Plugin.FChatClient.SendMessageInChannel("There is a match happening in the arena! If you want to see the progress of that match, type '!status Arena' in the bar!", channel);
                    }

                    else if (!Plugin.GetCurrentBattlefield(Constants.RDVFArena).IsInProgress && Plugin.GetCurrentBattlefield(Constants.RDVFVenue).IsInProgress)
                    {
                        Plugin.FChatClient.SendMessageInChannel("There is a match happening in the venue! If you want to see the progress of that match, type '!status Venue' in the bar!", channel);
                    }

                    else
                    {
                        Plugin.FChatClient.SendMessageInChannel("There are no matches going on right now in the venue or the arena.", channel);
                    }
                }
            }

            else
            {
                if (Plugin.GetCurrentBattlefield(channel).IsInProgress)
                {
                    Plugin.FChatClient.SendPrivateMessage(Plugin.GetCurrentBattlefield(channel).OutputController.LastMessageSent, character);
                }
                else
                {
                    Plugin.FChatClient.SendPrivateMessage("There's no match going on right now.", character);
                }
            }
        }
    }
}
