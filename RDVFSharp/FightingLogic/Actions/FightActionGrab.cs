using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionGrab : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();
            var damage = Utils.RollDice(new List<int>() { 6, 6 }) - 1 + attacker.Strength;
            damage /= 2;
            var requiredStam = 5;

            var difficulty = 6; //Base difficulty, rolls greater than this amount will hit.
            var others = battlefield.Fighters.Where(x => x.Name != attacker.Name).OrderBy(x => new Random().Next()).ToList();



            foreach (var fighter in others)
            {
                if ((fighter.CurrentTarget == attacker.CurrentTarget) && (fighter.IsDead == false)) difficulty += 2;
            }
            if (target.IsExposed > 0) difficulty -= 2; // If opponent left themself wide open after a failed strong attack, they'll be easier to hit.

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

            //grab can only be used when you are not grappling the target, so we no longer need the old check.
            battlefield.OutputController.Hit.Add(attacker.Name + " GRABBED " + target.Name + "! ");
            battlefield.OutputController.Hint.Add(target.Name + " is being grappled! " + attacker.Name + " has reduced difficulty to use melee attacks and can also use the special attacks Throw and Submission.");
            battlefield.OutputController.Hint.Add(target.Name + " can try to escape the grapple by using Move, Throw, or Teleport.");
            target.IsGrappledBy.Add(attacker.Name);


            damage = Math.Max(damage, 1);
            target.HitHp(damage);
            attacker.IsGrabbable = 40;
            target.IsGrabbable = 40;
            attacker.IsRestraining = 2;
            target.IsRestrained = true;
            return true; //Successful attack, if we ever need to check that.
        }
    }
}
