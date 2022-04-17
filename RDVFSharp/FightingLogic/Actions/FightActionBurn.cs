using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionBurn : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();
            var requiredMana = 10;
            var difficulty = 8; //Base difficulty, rolls greater than this amount will hit.

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
            var totalBonus = Utils.RollDice(new List<int>() { 5, 5 }) - 1 + attacker.Spellpower;

            {
                target.HPDOT = (int)Math.Ceiling((double)totalBonus / 2);
                target.HPBurn = 4;
                target.ManaDOT = (int)Math.Ceiling((double)totalBonus / 2);
                target.ManaDamage = 4;
                target.StaminaDamage = 0;
                battlefield.OutputController.Hit.Add(attacker.Name + " landed a strike against " + target.Name + " that will do damage over time for 3 turns!");
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
