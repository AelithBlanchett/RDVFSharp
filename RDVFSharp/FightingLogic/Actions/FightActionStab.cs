﻿using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionStab : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();
            var requiredStam = 10;
            var damage = 0;
            var difficulty = 8; //Base difficulty, rolls greater than this amount will hit.
            var others = battlefield.Fighters.Where(x => x.Name != attacker.Name).OrderBy(x => new Random().Next()).ToList();
            var othersdeadcheck = others.Where(x => x.IsDead == false).OrderBy(x => new Random().Next()).ToList();
            var sametarget = othersdeadcheck.Where(x => x.CurrentTarget == attacker.CurrentTarget).OrderBy(x => new Random().Next()).ToList();


            difficulty += 2 * sametarget.Count;
            
            if (attacker.IsRestrained) difficulty += 2; //Up the difficulty if the attacker is restrained.
            if (target.IsRestrained) difficulty -= 4; //Lower it if the target is restrained.
            if (target.IsExposed > 0) difficulty -= 2; // If opponent left themself wide open after a failed strong attack, they'll be easier to hit.


            //If opponent fumbled on their previous action they should become stunned.
            if (target.Fumbled)
            {
                target.IsDazed = true;
                target.Fumbled = false;
            }

            if (target.IsEvading > 0)
            {//Evasion bonus from move/teleport. Only applies to one attack, then is reset to 0.
             //Not affected by opponent's evasion bonus.
                difficulty += target.IsEvading;
            }
            if (attacker.IsAggressive > 0)
            {//Apply attack bonus from move/teleport then reset it.
                difficulty -= attacker.IsAggressive;
                damage += attacker.IsAggressive;
                attacker.IsAggressive = 0;
            }
            if (attacker.IsEvading > 0)
            {//Apply attack bonus from move/teleport then reset it.
                attacker.IsEvading = 0;
            }

            var totalBonus = Utils.RollDice(new List<int>() { 5, 5 }) - 1 + attacker.Dexterity;
            var critCheck = true;
            if (attacker.Stamina < requiredStam)
            {   //Not enough stamina-- reduced effect
                critCheck = false;
                damage /= 2;
                totalBonus /= 2;
                attacker.HitHp(requiredStam - attacker.Stamina);
                difficulty += (int)Math.Ceiling((double)((requiredStam - attacker.Stamina) / requiredStam) * (20 - difficulty)); // Too tired? You're likely to miss.
                battlefield.OutputController.Hint.Add(attacker.Name + " did not have enough stamina, and took penalties to the attack.");
            }

            attacker.HitStamina(requiredStam); //Now that mana has been checked, reduce the attacker's mana by the appopriate amount.

            var attackTable = attacker.BuildActionTable(difficulty, target.Dexterity, attacker.Dexterity, target.Stamina, target.StaminaCap);
            //If target can dodge the atatcker has to roll higher than the dodge value. Otherwise they need to roll higher than the miss value. We display the relevant value in the output.
            battlefield.OutputController.Info.Add("Dice Roll Required: " + (attackTable.miss + 1));


            if (roll <= attackTable.miss)
            {   //Miss-- no effect.
                battlefield.OutputController.Hit.Add(" FAILED!");
                return false; //Failed attack, if we ever need to check that.
            }

            if (roll >= attackTable.crit && critCheck == true)
            { //Critical Hit-- increased damage/effect.
                battlefield.OutputController.Hit.Add(" CRITICAL HIT! ");
                battlefield.OutputController.Hint.Add(attacker.Name + " landed a particularly vicious blow!");
                damage += 10;
            }

            foreach (var opponent in battlefield.Fighters.Where(x => x.TeamColor != attacker.TeamColor))
            {
                if (attacker.IsGrabbable > 0 && opponent.IsGrabbable == attacker.IsGrabbable && !attacker.IsGrappling(target) && !target.IsGrappling(attacker))
                {
                    battlefield.OutputController.Hint.Add(attacker.Name + " managed to put some distance between them and " + opponent.Name + " and is now out of grabbing range.");
                }
            }


            {
                attacker.IsGrabbable = 0;
                target.IsGrabbable = 0;
                target.HPDOT = (int)Math.Ceiling((double)totalBonus / 2);
                target.HPBurn = 4;
                target.StaminaDOT = (int)Math.Ceiling((double)totalBonus / 2);
                target.StaminaDamage = 4;
                target.ManaDamage = 0;
                target.HitHp(damage);
                target.HitStamina(damage);
                battlefield.OutputController.Hit.Add(attacker.Name + " landed a strike against " + target.Name + " that will do damage over time for 3 turns!");
            }

            

            return true; //Successful attack, if we ever need to check that.
        }
    }
}
