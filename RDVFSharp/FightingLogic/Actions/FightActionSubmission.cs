using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionSubmission : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();
            var damage = 30;
            var requiredStam = 25;

            var difficulty = 10; //Base difficulty, rolls greater than this amount will hit.
            difficulty += (int)Math.Floor((double)(target.Strength - attacker.Strength) / 2); //Up the difficulty of submission moves based on the relative strength of the combatants.
            //difficulty *= (int)Math.Ceiling((double)2 * target.HP / target.MaxHP);//Multiply difficulty with percentage of opponent's health and 2, so that 50% health yields normal difficulty.

            if (target.HP * 100 / target.MaxHP > 50) // If target is above 50% HP this is a bad move.
            {
                damage /= 2;
                difficulty *= 2;
            }

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
            if (attacker.Stamina < requiredStam)
            {   //Not enough stamina-- reduced effect
                critCheck = false;
                damage *= attacker.Stamina / requiredStam;
                difficulty += (int)Math.Ceiling((double)((requiredStam - attacker.Stamina) / requiredStam) * (20 - difficulty)); // Too tired? You're likely to miss.
                battlefield.OutputController.Hint.Add(attacker.Name + " did not have enough stamina, and took penalties to the attack.");
            }

            attacker.HitStamina(requiredStam); //Now that stamina has been checked, reduce the attacker's stamina by the appopriate amount. (We'll hit the attacker up for the rest on a miss or a dodge).

            //If opponent fumbled on their previous action they should become stunned.
            // We put it down here for Grab so it doesn't interfere with the stun from a crit on moving into range.
            if (target.Fumbled)
            {
                target.IsDazed = true;
                target.Fumbled = false;
            }

            var attackTable = attacker.BuildActionTable(difficulty, target.Dexterity, attacker.Dexterity, target.Stamina, target.StaminaCap);
            //If target can dodge the atatcker has to roll higher than the dodge value. Otherwise they need to roll higher than the miss value. We display the relevant value in the output.
            battlefield.OutputController.Info.Add("Dice Roll Required: " + (attackTable.miss + 1));

            if (roll <= attackTable.miss)
            {   //Miss-- no effect.
                battlefield.OutputController.Hit.Add(" FAILED! ");
                battlefield.OutputController.Hint.Add(attacker.Name + " failed to establish a hold!");
                return false; //Failed attack, if we ever need to check that.
            }

            if (roll >= attackTable.crit && critCheck)
            { //Critical Hit-- increased damage/effect, typically 3x damage if there are no other bonuses.
                battlefield.OutputController.Hit.Add(" CRITICAL HIT! ");
                battlefield.OutputController.Hint.Add("Critical! " + attacker.Name + " found a particularly good hold!");
                damage += 10;
            }

            battlefield.OutputController.Hit.Add(" SUBMISSION ");
            target.IsEscaping -= 5; //Submission moves make it harder to escape.
            if (target.IsGrappling(attacker))
            {
                attacker.RemoveGrappler(target);
                battlefield.OutputController.Hint.Add(target.Name + " is in a SUBMISSION hold. " + attacker.Name + " is also no longer at a penalty from being grappled!");
            }
            else
            {
                battlefield.OutputController.Hint.Add(target.Name + " is in a SUBMISSION hold.");
            }

            //If we managed to make a submission without being in grab range, we are certainly in grabe range afterwards.
            if (!battlefield.InGrabRange) battlefield.InGrabRange = true;

            damage = Math.Max(damage, 1);
            target.HitHp(damage);
            return true; //Successful attack, if we ever need to check that.
        }
    }
}
