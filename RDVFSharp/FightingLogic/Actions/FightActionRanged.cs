using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionRanged : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();
            var damage = Utils.RollDice(new List<int>() { 5, 5 }) - 1;
            damage *= 2;
            damage += (attacker.Strength + attacker.Dexterity);
            damage += Math.Min(attacker.Strength, attacker.Spellpower);
            var requiredStam = 10;
            var difficulty = 10; //Base difficulty, rolls greater than this amount will hit.

            //If opponent fumbled on their previous action they should become stunned.
            if (target.Fumbled)
            {
                target.IsDazed = true;
                target.Fumbled = false;
            }

            if (target.HPBurn > 1) difficulty -= 1;
            if (attacker.IsRestrained) difficulty += 4; //Up the difficulty considerably if the attacker is restrained.
            if (target.IsRestrained) difficulty += 4; //Ranged attacks during grapple are hard.
            if (target.IsRestrained) difficulty -= 2; //Lower the difficulty slightly if the target is restrained.
            if (attacker.IsFocused > 0) difficulty -= (int)Math.Ceiling((double)attacker.IsFocused / 10); //Lower the difficulty considerably if the attacker is focused

            if (attacker.IsFocused > 0) damage += (int)Math.Ceiling((double)attacker.IsFocused / 10); //Focus gives bonus damage.

            if (target.IsEvading > 0)
            {//Evasion bonus from move/teleport. Only applies to one attack, then is reset to 0.
                difficulty += (int)Math.Ceiling((double)target.IsEvading / 2);//Half effect on ranged attacks.
                damage -= (int)Math.Ceiling((double)target.IsEvading / 2);
                target.IsEvading = 0;
            }
            if (attacker.IsAggressive > 0)
            {//Apply attack bonus from move/teleport then reset it.
                difficulty -= attacker.IsAggressive;
                damage += attacker.IsAggressive;
                attacker.IsAggressive = 0;
            }

            var critCheck = true;
            if (attacker.Stamina < requiredStam)
            {   //Not enough stamina-- reduced effect
                critCheck = false;
                damage *= attacker.Stamina / requiredStam;
                difficulty += (int)Math.Ceiling((double)((requiredStam - attacker.Stamina) / requiredStam) * (20 - difficulty)); // Too tired? You're likely to miss.
                battlefield.OutputController.Hint.Add(attacker.Name + " did not have enough stamina, and took penalties to the attack.");
            }

            attacker.HitStamina(requiredStam); //Now that stamina has been checked, reduce the attacker's stamina by the appopriate amount.

            var attackTable = attacker.BuildActionTable(difficulty, target.Dexterity, attacker.Dexterity, target.Stamina, target.StaminaCap);
            //If target can dodge the atatcker has to roll higher than the dodge value. Otherwise they need to roll higher than the miss value. We display the relevant value in the output.
            battlefield.OutputController.Info.Add("Dice Roll Required: " + (attackTable.miss + 1));

            if (roll <= attackTable.miss)
            {   //Miss-- no effect.
                battlefield.OutputController.Hit.Add(" FAILED!");
                return false; //Failed attack, if we ever need to check that.
            }

            if (roll >= attackTable.crit && critCheck == true)
            { //Critical Hit-- increased damage/effect, typically 3x damage if there are no other bonuses.
                battlefield.OutputController.Hit.Add(" CRITICAL HIT! ");
                battlefield.OutputController.Hint.Add(attacker.Name + " hit somewhere that really hurts!");
                damage += 10;
            }
            else
            { //Normal hit.
                battlefield.OutputController.Hit.Add(" HIT! ");
            }

            //Deal all the actual damage/effects here.

            if (battlefield.InGrabRange)
            {// Succesful attacks will beat back the grabber before they can grab you, but not if you're already grappling.
                if (!attacker.IsRestrained && !target.IsRestrained)
                {
                    battlefield.InGrabRange = false;
                    battlefield.OutputController.Hit.Add(attacker.Name + " distracted " + target.Name + " with the attack and was able to move out of grappling range!");
                }
            }

            //If you're being grappled and you hit the opponent that will make it a little easier to escape later on.
            if (attacker.IsRestrained) attacker.IsEscaping += (int)Math.Floor((double)damage / 5);

            damage = Math.Max(damage, 1);
            target.HitHp(damage);
            return true; //Successful attack, if we ever need to check that.
        }
    }
}
