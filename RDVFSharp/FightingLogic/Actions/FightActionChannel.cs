using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionChannel : BaseFightAction
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
                battlefield.OutputController.Hint.Add(attacker.Name + " was too disoriented or distracted to channel mana.");
                return false; //Failed action, if we ever need to check that.
            }

            if (roll == 20)
            {
                battlefield.OutputController.Hit.Add("CRITICAL SUCCESS! ");
                battlefield.OutputController.Hint.Add(attacker.Name + " can perform another action!");
                target.IsDazed = true;
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
            var manaShift = 12 + (attacker.Willpower * 2);
            //manaShift = Math.Min(manaShift, attacker.Stamina); //This also needs to be commented awaay if we want to remove stamina cost.

            //attacker._manaCap = Math.Max(attacker._manaCap, attacker.mana + manaShift);
            //attacker.HitStamina(manaShift);
            attacker.AddMana(manaShift);
            battlefield.OutputController.Hit.Add(attacker.Name + " GENERATES MANA!"); //Removed Stamina cost.
            battlefield.OutputController.Hint.Add(attacker.Name + " recovered " + manaShift + " mana!");
            return true;
        }
    }
}
