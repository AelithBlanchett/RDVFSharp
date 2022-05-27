using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionCurse : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();
            var damage = 11 + attacker.Spellpower - attacker.Strength;
            damage *= 2;
            var requiredMana = 20;
            var difficulty = 10; //Base difficulty, rolls greater than this amount will hit.
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

            if (attacker.IsRestrained) difficulty += 2; //Math.Max(2, 4 + (int)Math.Floor((double)(target.Strength - attacker.Strength) / 2)); //When grappled, up the difficulty based on the relative strength of the combatants. Minimum of +2 difficulty, maximum of +8.
            if (target.IsRestrained) difficulty -= 4; //Ranged attacks during grapple are hard.
            if (attacker.IsFocused > 0) difficulty -= (int)Math.Ceiling((double)attacker.IsFocused / 10); //Lower the difficulty if the attacker is focused
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
            {   //Miss-- no effect. Happens during grappling.
                battlefield.OutputController.Hit.Add(" CURSE FAILED! Curse may not be used again by the attacker!");
                attacker.CurseUsed += 10;
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
                battlefield.OutputController.Hit.Add("CURSE HIT! Curse may not be used again by the attacker!");
            }

            //Deal all the actual damage/effects here.

            foreach (var opponent in battlefield.Fighters.Where(x => x.TeamColor != attacker.TeamColor))
            {
                if (attacker.IsGrabbable > 0 && opponent.IsGrabbable == attacker.IsGrabbable && !attacker.IsGrappling(target) && !target.IsGrappling(attacker))
                {
                    battlefield.OutputController.Hint.Add(attacker.Name + " managed to put some distance between them and " + opponent.Name + " and is now out of grabbing range.");
                }
            }


            //If you're being grappled and you hit the opponent that will make it a little easier to escape later on.
            if (attacker.IsRestrained) attacker.IsEscaping += (int)Math.Floor((double)damage / 5);

            attacker.IsGrabbable = 0;
            target.IsGrabbable = 0;
            damage = Math.Max(damage, 1);
            target.HitHp(damage);
            attacker.CurseUsed += 10;
            return true; //Successful attack, if we ever need to check that.
        }
    }
}
