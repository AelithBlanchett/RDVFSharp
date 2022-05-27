using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RDVFSharp.Commands
{
    public class Target : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Sets your target.";

        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            var target = Plugin.CurrentBattlefield.GetTarget();
            if (Plugin.CurrentBattlefield.GetFighter(character).IsGrappling(target))
            {
                Plugin.FChatClient.SendMessageInChannel("You cannot change targets while grappling someone.", channel);
            }

            else if ((Plugin.CurrentBattlefield.IsAbleToAttack(character)))
            {
                if (args.Count() < 1)
                {
                    return;
                }
                var argsList = args.ToList();

                var characterName = string.Join(' ', argsList.Skip(0));

                var activeFighter = Plugin.CurrentBattlefield.GetFighter(character);
                var NewTarget = Plugin.CurrentBattlefield.GetFighter(characterName);
                var battlefield = Plugin.CurrentBattlefield;

                if ((NewTarget != null) && (NewTarget.TeamColor != activeFighter.TeamColor))
                {
                    {
                        activeFighter.CurrentTarget = NewTarget;
                        battlefield.OutputFighterStatuses();
                        battlefield.OutputController.Action.Add("Target");
                        battlefield.OutputController.Hit.Add($"Target successfully set to {NewTarget.Name} for {activeFighter.Name}.");
                        if (activeFighter.IsEvading > 0)
                        {
                            battlefield.OutputController.Hint.Add(activeFighter.Name + " has a temporary +" + activeFighter.IsEvading + " bonus to evasion and damage reduction.");
                        }

                        if (activeFighter.IsAggressive > 0)
                        {
                            battlefield.OutputController.Hint.Add(activeFighter.Name + " has a temporary +" + activeFighter.IsAggressive + " bonus to accuracy and attack damage.");
                        }

                        if (activeFighter.StaminaDamage > 1)
                        {
                            battlefield.OutputController.Hint.Add(activeFighter.Name + " is taking " + activeFighter.HPDOT + " damage to both Stamina and HP for " + (activeFighter.HPBurn - 1) + " turn(s).");
                        }

                        if (activeFighter.ManaDamage > 1)
                        {
                            battlefield.OutputController.Hint.Add(activeFighter.Name + " is taking " + activeFighter.HPDOT + " damage to both Mana and HP for " + (activeFighter.HPBurn - 1) + " turn(s).");
                        }

                        if (activeFighter.IsGuarding > 0)
                        {
                            battlefield.OutputController.Hint.Add(activeFighter.Name + " has a temporary +" + activeFighter.IsGuarding + " bonus to evasion and damage reduction.");
                        }
                        if (activeFighter.IsGrabbable == NewTarget.IsGrabbable && activeFighter.IsGrabbable>0 && activeFighter.IsGrabbable<10)
                        {
                            battlefield.OutputController.Hint.Add(activeFighter.Name + " and " + NewTarget.Name + " are in grappling range.");
                        }
                        if (NewTarget.IsEvading > 0)
                        {
                            battlefield.OutputController.Hint.Add(NewTarget.Name + " has a temporary +" + NewTarget.IsEvading + " bonus to evasion and damage reduction.");
                        }
                        if (NewTarget.IsAggressive > 0)
                        {
                            battlefield.OutputController.Hint.Add(NewTarget.Name + " has a temporary +" + NewTarget.IsAggressive + " bonus to accuracy and attack damage.");
                        }

                        if (NewTarget.StaminaDamage > 1)
                        {
                            battlefield.OutputController.Hint.Add(NewTarget.Name + " is taking " + NewTarget.HPDOT + " damage to both Stamina and HP for " + (NewTarget.HPBurn - 1) + " turn(s).");
                        }

                        if (NewTarget.ManaDamage > 1)
                        {
                            battlefield.OutputController.Hint.Add(NewTarget.Name + " is taking " + NewTarget.HPDOT + " damage to both Mana and HP for " + (NewTarget.HPBurn - 1) + " turn(s).");
                        }

                        if (NewTarget.IsGuarding > 0)
                        {
                            battlefield.OutputController.Hint.Add(NewTarget.Name + " has a temporary +" + NewTarget.IsGuarding + " bonus to evasion and damage reduction.");
                        }
                        if (NewTarget.IsExposed > 0)
                        {
                            battlefield.OutputController.Hint.Add(NewTarget.Name + " is exposed and has a -2 difficulty to be hit");
                        }

                        battlefield.OutputController.Broadcast(battlefield);
                        
                    }
                }

                else if (NewTarget.TeamColor == activeFighter.TeamColor)
                {
                    {
                        Plugin.FChatClient.SendMessageInChannel("You cannot target your own team members.", channel);
                    }
                }

                else
                {
                    throw new FighterNotFound(args.FirstOrDefault());
                }
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You may not change targets right now.", channel);
            }
        }
    }
}
