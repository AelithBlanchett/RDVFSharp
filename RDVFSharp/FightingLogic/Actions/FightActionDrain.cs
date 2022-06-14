using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionDrain : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();
            var requiredMana = 10;
            var damage = 0;
            var difficulty = 8; //Base difficulty, rolls greater than this amount will hit.
            var others = battlefield.Fighters.Where(x => x.Name != attacker.Name).OrderBy(x => new Random().Next()).ToList();



            foreach (var fighter in others)
            {
                if ((fighter.CurrentTarget == attacker.CurrentTarget) && (fighter.IsDead == false)) difficulty += 2;
            }
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
                target.IsEvading = 0;
            }
            if (attacker.IsAggressive > 0)
            {//Apply attack bonus from move/teleport then reset it.
                difficulty -= attacker.IsAggressive;
                damage += attacker.IsAggressive;
                attacker.IsAggressive = 0;
            }

            if (attacker.Mana < requiredMana)
            {   //Not enough stamina-- reduced effect
                difficulty += (int)Math.Ceiling((double)((requiredMana - attacker.Mana) / requiredMana) * (20 - difficulty)); // Too tired? You're going to fail.
                battlefield.OutputController.Hint.Add(attacker.Name + " didn't have enough Mana and took a penalty to the attempt.");
            }

            attacker.HitMana(requiredMana); //Now that mana has been checked, reduce the attacker's mana by the appopriate amount.

            var attackTable = attacker.BuildActionTable(difficulty, target.Dexterity, attacker.Dexterity, target.Mana, target.ManaCap);
            //If target can dodge the atatcker has to roll higher than the dodge value. Otherwise they need to roll higher than the miss value. We display the relevant value in the output.
            battlefield.OutputController.Info.Add("Dice Roll Required: " + (attackTable.miss + 1));


            if (roll <= attackTable.miss)
            {   //Miss-- no effect.
                battlefield.OutputController.Hit.Add(" FAILED!");
                return false; //Failed attack, if we ever need to check that.
            }

            if (roll >= attackTable.crit)
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

            //The total mobility bonus generated. This will be split bewteen attack and defense.
            var totalBonus = Utils.RollDice(new List<int>() { 5, 5 }) - 1 + attacker.Spellpower;

            {
                attacker.IsGrabbable = 0;
                target.IsGrabbable = 0;
                target.HPDOT = (int)Math.Ceiling((double)totalBonus / 2);
                target.HPBurn = 4;
                target.ManaDOT = (int)Math.Ceiling((double)totalBonus / 2);
                target.ManaDamage = 4;
                target.StaminaDamage = 0;
                target.HitHp(damage);
                target.HitMana(damage);
                battlefield.OutputController.Hit.Add(attacker.Name + " landed a strike against " + target.Name + " that will do damage over time for 3 turns!");
            }



            return true; //Successful attack, if we ever need to check that.
        }
    }
}
