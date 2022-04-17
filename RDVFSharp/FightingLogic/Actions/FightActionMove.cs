using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionMove : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();
            var requiredStam = 5;
            var difficulty = 1; //Base difficulty, rolls greater than this amount will hit.

            //If opponent fumbled on their previous action they should become stunned.
            if (target.Fumbled)
            {
                target.IsDazed = true;
                target.Fumbled = false;
            }


            if (attacker.IsRestrained) difficulty += (11 + (int)Math.Floor((double)(target.Strength - attacker.Strength) / 2)); //When grappled, up the difficulty based on the relative strength of the combatants.
            if (attacker.IsRestrained) difficulty -= attacker.IsEscaping; //Then reduce difficulty based on how much effort we've put into escaping so far.
            if (target.IsRestrained) difficulty -= 4; //Lower the difficulty considerably if the target is restrained.

            if (target.IsEvading > 0)
            {//Evasion bonus from move/teleport. Only applies to one attack, then is reset to 0.
             //Not affected by opponent's evasion bonus.
                target.IsEvading = 0;
            }
            if (attacker.IsAggressive > 0)
            {//Apply attack bonus from move/teleport then reset it.
                difficulty -= attacker.IsAggressive;
                attacker.IsAggressive = 0;
            }

            if (attacker.Stamina < requiredStam)
            {   //Not enough stamina-- reduced effect
                difficulty += (int)Math.Ceiling((double)((requiredStam - attacker.Stamina) / requiredStam) * (20 - difficulty)); // Too tired? You're going to fail.
                battlefield.OutputController.Hint.Add(attacker.Name + " didn't have enough Stamina and took a penalty to the escape attempt.");
            }

            attacker.HitStamina(requiredStam); //Now that stamina has been checked, reduce the attacker's stamina by the appopriate amount.

            var attackTable = attacker.BuildActionTable(difficulty, 0, 0, target.Stamina, target.StaminaCap);
            //If target can dodge the atatcker has to roll higher than the dodge value. Otherwise they need to roll higher than the miss value. We display the relevant value in the output.
            battlefield.OutputController.Info.Add("Dice Roll Required: " + (attackTable.miss + 1));

            var tempGrappleFlag = true;
            if (attacker.IsGrappling(target))
            { //If you're grappling someone they are freed, regardless of the outcome.
                battlefield.OutputController.Hint.Add(attacker.Name + " used ESCAPE. " + target.Name + " is no longer being grappled. ");
                target.RemoveGrappler(attacker);
                tempGrappleFlag = false;
            }

            if (roll <= attackTable.miss)
            {   //Miss-- no effect.
                battlefield.OutputController.Hit.Add(" FAILED!");
                if (attacker.IsRestrained) attacker.IsEscaping += 4;//If we fail to escape, it'll be easier next time.
                return false; //Failed attack, if we ever need to check that.
            }

            if (roll >= attackTable.crit)
            { //Critical Hit-- increased damage/effect, typically 3x damage if there are no other bonuses.
                battlefield.OutputController.Hit.Add(" CRITICAL SUCCESS! ");
                battlefield.OutputController.Hint.Add(attacker.Name + " can perform another action!");
                // The only way the target can be stunned is if we set it to stunned with the action we're processing right now.
                // That in turn is only possible if target had fumbled. So we restore the fumbled status, but keep the stun.
                // That way we properly get a third action.
                if (target.IsDazed) target.Fumbled = true;
                foreach (var opposingFighter in battlefield.Fighters.Where(x => x.TeamColor != attacker.TeamColor))
                {
                    opposingFighter.IsDazed = true;
                }
                if (target.IsDisoriented > 0) target.IsDisoriented += 2;
                if (target.IsExposed > 0) target.IsExposed += 2;
            }

            //The total mobility bonus generated. This will be split bewteen attack and defense.
            var totalBonus = Utils.RollDice(new List<int>() { 5, 5 }) - 1 + attacker.Dexterity;

            if (target.IsGrappling(attacker))
            { //If you were being grappled, you get free.
                battlefield.OutputController.Hint.Add(attacker.Name + " escaped " + target.Name + "'s hold! ");
                attacker.RemoveGrappler(target);
                tempGrappleFlag = false;
                attacker.IsEvading = (int)Math.Floor((double)totalBonus / 2);
            }
            else
            {
                attacker.IsAggressive = (int)Math.Ceiling((double)totalBonus / 2);
                battlefield.OutputController.Hit.Add(attacker.Name + " gained mobility bonuses against " + target.Name + " for one turn!");
            }

            if (battlefield.InGrabRange)
            {
                battlefield.OutputController.Hit.Add(attacker.Name + " moved away!");
                battlefield.InGrabRange = false;
                battlefield.OutputController.Hint.Add(attacker.Name + " managed to put some distance between them and " + target.Name + " and is now out of grabbing range.");
            }
            return true; //Successful attack, if we ever need to check that.
        }
    }
}
