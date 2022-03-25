using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionFocus : BaseFightAction
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
                battlefield.WindowController.Hint.Add(attacker.Name + " was too disoriented or distracted to focus.");
                return false; //Failed action, if we ever need to check that.
            }

            if (roll == 20)
            {
                battlefield.WindowController.Hit.Add("CRITICAL SUCCESS! ");
                battlefield.WindowController.Hint.Add(attacker.Name + " can perform another action!");
                target.IsStunned = true;
                if (target.IsDisoriented > 0) target.IsDisoriented += 2;
                if (target.IsExposed > 0) target.IsExposed += 2;
            }

            //If opponent fumbled on their previous action they should become stunned, unless they're already stunned by us rolling a 20.
            if (target.Fumbled & !target.IsStunned)
            {
                target.IsStunned = true;
                target.Fumbled = false;
            }

            battlefield.WindowController.Info.Add("Dice Roll Required: " + Math.Max(2, (difficulty + 1)));
            battlefield.WindowController.Hit.Add(attacker.Name + " FOCUSES!");
            attacker.IsFocused += Utils.RollDice(new List<int>() { 6, 6, 6, 6 }) + 10 + attacker.Willpower * 4;
            return true;
        }
    }
}
