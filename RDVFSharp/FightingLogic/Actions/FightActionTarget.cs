using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionTarget : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = battlefield.GetActor();
            var target = battlefield.GetTarget();
            var difficulty = 6; //Base difficulty, rolls greater than this amount will hit.

            //If opponent fumbled on their previous action they should become stunned.

            if (attacker.IsRestrained) difficulty += (4); //When grappled, up the difficulty based on the relative strength of the combatants.

            if (attacker.IsAggressive > 0)
            {//Apply attack bonus from move/teleport then reset it.
                difficulty -= attacker.IsAggressive;
                attacker.IsAggressive = 0;
            }

            if (attacker.IsGuarding > 0)
            {
                //Apply attack bonus from move/teleport then reset it.
                attacker.IsGuarding = 0;
            }

            if (attacker.IsEvading > 0)
            {
                //Apply attack bonus from move/teleport then reset it.
                attacker.IsEvading = 0;
            }

            var attackTable = attacker.BuildActionTable(difficulty, 0, 0, target.Mana, target.ManaCap);
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
                battlefield.Fighters.ForEach(f => f.IsDazed = true);
                attacker.IsDazed = false;
            }

            if (roll >= attackTable.targethit)
            {

                battlefield.OutputController.Hit.Add("Success! Your target has changed!");
                attacker.CurrentTarget = target;

            }




            return true; //Successful attack, if we ever need to check that.
        }
    }
}
