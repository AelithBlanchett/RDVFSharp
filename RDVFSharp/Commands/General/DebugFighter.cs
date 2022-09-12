using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class DebugFighter : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Sets a property to a certain value for a fighter in an ongoing fight.";

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.FChatClient.IsUserAdmin(character, channel) && Plugin.GetCurrentBattlefield(channel).IsInProgress)
            {
                if(args.Count() < 3)
                {
                    return;
                }
                var argsList = args.ToList();

                var propertyName = argsList[0];
                var propertyValue = argsList[1];
                var characterName = string.Join(' ', argsList.Skip(2));

                var activeFighter = Plugin.GetCurrentBattlefield(channel).GetFighter(characterName);
                if (activeFighter != null)
                {
                    PropertyInfo prop = activeFighter.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != prop && prop.CanWrite)
                    {
                        var convertedVal = Convert.ChangeType(propertyValue, prop.PropertyType);
                        prop.SetValue(activeFighter, convertedVal, null);
                        Plugin.FChatClient.SendMessageInChannel($"Value successfully set to {prop.GetValue(activeFighter)} for {activeFighter.Name}.", channel);
                    }
                    else
                    {
                        Plugin.FChatClient.SendMessageInChannel($"The value couldn't be changed: the input value isn't of the right type or the value couldn't be written.", channel);
                    }
                }
                else
                {
                    Plugin.FChatClient.SendMessageInChannel("This fighter was not found. Please check the spelling of the fighter's name!", channel);
                }
            }
            else if (Plugin.GetCurrentBattlefield(channel).IsInProgress && !Plugin.FChatClient.IsUserAdmin(character, channel))
            {
                Plugin.FChatClient.SendMessageInChannel("You do not have access to this command", channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("There is no match going on right now", channel);
            }
        }
    }
}
