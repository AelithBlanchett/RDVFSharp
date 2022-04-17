using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionRest : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();
            var difficulty = 1; //Base difficulty, rolls greater than this amount will succeed.

            //if (attacker.IsDisoriented) difficulty += 2; //Up the difficulty if you are dizzy.
            if (attacker.IsRestrained) difficulty += 9; //Up the difficulty considerably if you are restrained.

            if (target.IsEvading > 0)
            {//Evasion bonus from move/teleport. Lasts 1 turn. We didn't make an attack and now it resets to 0.
                target.IsEvading = 0;
            }
            if (attacker.IsAggressive > 0)
            {//Apply bonus to our action from move/teleport then reset it.
                difficulty -= attacker.IsAggressive;
                attacker.IsAggressive = 0;
            }

            if (roll <= difficulty)
            {   //Failed!
                battlefield.OutputController.Hint.Add(attacker.Name + " was too disoriented or distracted to get any benefit from resting.");
                return false; //Failed action, if we ever need to check that.
            }

            if (roll == 20)
            {
                battlefield.OutputController.Hit.Add("CRITICAL SUCCESS! ");
                battlefield.OutputController.Hint.Add(attacker.Name + " can perform another action!");
                foreach (var opposingFighter in battlefield.Fighters.Where(x => x.TeamColor != attacker.TeamColor))
                {
                    opposingFighter.IsDazed = true;
                }
                if (target.IsDisoriented > 0) target.IsDisoriented += 2;
                if (target.IsExposed > 0) target.IsExposed += 2;
            }

            //If opponent fumbled on their previous action they should become stunned, unless they're already stunned by us rolling a 20.
            if (target.Fumbled & !target.IsDazed)
            {
                target.IsDazed = true;
                target.Fumbled = false;
            }

            battlefield.OutputController.Info.Add("Dice Roll Required: " + Math.Max(2, (difficulty + 1)));
            var staminaShift = 12 + (attacker.Willpower * 2);
            //staminaShift = Math.Min(staminaShift, attacker.mana);

            //attacker.StaminaCap = Math.Max(attacker.StaminaCap, attacker.Stamina + staminaShift);
            //attacker.HitMana(staminaShift);
            attacker.AddStamina(staminaShift);
            battlefield.OutputController.Hit.Add(attacker.Name + " REGENERATES STAMINA!"); //Removed Stamina cost.
            battlefield.OutputController.Hint.Add(attacker.Name + " recovered " + staminaShift + " stamina!");
            return true;
        }
    }
}
