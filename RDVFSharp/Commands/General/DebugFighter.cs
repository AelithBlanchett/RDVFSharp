using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RDVFSharp.Commands
{
    public class DebugFighter : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Sets a property to a certain value for a fighter in an ongoing fight.";

        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            if (Plugin.FChatClient.IsUserAdmin(character, channel) && Plugin.CurrentBattlefield.IsActive)
            {
                if(args.Count() < 3)
                {
                    return;
                }
                var argsList = args.ToList();

                var propertyName = argsList[0];
                var propertyValue = argsList[1];
                var characterName = string.Join(' ', argsList.Skip(2));

                var activeFighter = Plugin.CurrentBattlefield.GetFighter(characterName);
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
                    throw new FighterNotFound(args.FirstOrDefault());
                }
            }
        }
    }
}
