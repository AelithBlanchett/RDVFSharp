using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionMana : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();
            var damage = Utils.RollDice(new List<int>() { 5, 5 }) - 1 + attacker.Spellpower;
            damage *= 2;
            damage += Math.Min(attacker.Strength, attacker.Spellpower);
            var requiredMana = 10;
            var difficulty = 8; //Base difficulty, rolls greater than this amount will hit.

            //If opponent fumbled on their previous action they should become stunned.
            if (target.Fumbled)
            {
                target.IsDazed = true;
                target.Fumbled = false;
            }

            if (attacker.IsRestrained) difficulty += 2; //Math.Max(2, 4 + (int)Math.Floor((double)(target.Strength - attacker.Strength) / 2)); //When grappled, up the difficulty based on the relative strength of the combatants. Minimum of +2 difficulty, maximum of +8.
            if (target.IsRestrained) difficulty -= 4; //Lower the difficulty considerably if the target is restrained.
            if (target.IsExposed > 0) difficulty -= 2; // If opponent left themself wide open after a failed strong attack, they'll be easier to hit.
            if (target.HPBurn > 1) difficulty -= 1;

            if (target.IsEvading > 0)
            {//Evasion bonus from move/teleport. Only applies to one attack, then is reset to 0.
                difficulty += target.IsEvading;
                damage -= target.IsEvading;
                target.IsEvading = 0;
            }
            if (attacker.IsAggressive > 0)
            {//Apply attack bonus from move/teleport then reset it.
                difficulty -= attacker.IsAggressive;
                damage += attacker.IsAggressive;
                attacker.IsAggressive = 0;
            }

            var critCheck = true;
            if (attacker.Mana < requiredMana)
            {   //Not enough mana-- reduced effect
                critCheck = false;
                damage *= attacker.Mana / requiredMana;
                difficulty += (int)Math.Ceiling((double)((requiredMana - attacker.Mana) / requiredMana) * (20 - difficulty)); // Too tired? You're likely to have your spell fizzle.
                battlefield.OutputController.Hint.Add(attacker.Name + " did not have enough mana, and took penalties to the attack.");
            }

            attacker.HitMana(requiredMana); //Now that required mana has been checked, reduce the attacker's mana by the appopriate amount.

            var attackTable = attacker.BuildActionTable(difficulty, target.Dexterity, attacker.Dexterity, target.Mana, target.ManaCap);
            //If target can dodge the atatcker has to roll higher than the dodge value. Otherwise they need to roll higher than the miss value. We display the relevant value in the output.
            battlefield.OutputController.Info.Add("Dice Roll Required: " + (attackTable.miss + 1));

            if (roll <= attackTable.miss)
            {   //Miss-- no effect.
                battlefield.OutputController.Hit.Add(" FAILED!");
                attacker.IsExposed += 2; //If the fighter misses a big attack, it leaves them open and they have to recover balance which gives the opponent a chance to strike.
                battlefield.OutputController.Hint.Add(attacker.Name + " was left wide open by the failed attack and is now Exposed! " + target.Name + " has -2 difficulty to hit and can use Grab even if fighters are not in grappling range!");
                return false; //Failed attack, if we ever need to check that.
            }

            if (roll >= attackTable.crit)
            { //Critical Hit-- increased damage/effect, typically 3x damage if there are no other bonuses.
                battlefield.OutputController.Hit.Add(" CRITICAL HIT! ");
                battlefield.OutputController.Hint.Add(attacker.Name + " landed a particularly vicious blow!");
                damage += 10;
                battlefield.OutputController.Hint.Add("Critical Hit! " + attacker.Name + "'s magic worked abnormally well! " + target.Name + " is dazed and disoriented.");
            }
            else
            { //Normal hit.
                battlefield.OutputController.Hit.Add("MAGIC HIT! ");
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
