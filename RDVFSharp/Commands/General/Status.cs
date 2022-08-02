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
            if (channel == "ADH-a823a4e998a2b3d31794")
            {
                var argsList = args.ToList();
                var NamedChannel = argsList[0];

                if (NamedChannel == "Arena" || NamedChannel == "arena")
                {
                    Plugin.FChatClient.SendPrivateMessage(Plugin.GetCurrentBattlefield("ADH-b3c88050e9c580631c70").OutputController.LastMessageSent, character);
                }

                else if (NamedChannel == "Venue" || NamedChannel == "venue")
                {
                    Plugin.FChatClient.SendPrivateMessage(Plugin.GetCurrentBattlefield("ADH-51710b5ac8cce7e99f19").OutputController.LastMessageSent, character);
                }
                
                else
                {
                    if (Plugin.GetCurrentBattlefield("ADH-b3c88050e9c580631c70").IsInProgress && Plugin.GetCurrentBattlefield("ADH-51710b5ac8cce7e99f19").IsInProgress)
                    {
                        Plugin.FChatClient.SendMessageInChannel("There are matches happening in both the arena and the venue!", channel);
                    }

                    else if (Plugin.GetCurrentBattlefield("ADH-b3c88050e9c580631c70").IsInProgress && !Plugin.GetCurrentBattlefield("ADH-51710b5ac8cce7e99f19").IsInProgress)
                    {
                        Plugin.FChatClient.SendMessageInChannel("There is a match happening in the arena!", channel);
                    }

                    else if (!Plugin.GetCurrentBattlefield("ADH-b3c88050e9c580631c70").IsInProgress && Plugin.GetCurrentBattlefield("ADH-51710b5ac8cce7e99f19").IsInProgress)
                    {
                        Plugin.FChatClient.SendMessageInChannel("There is a match happening in the venue!", channel);
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
