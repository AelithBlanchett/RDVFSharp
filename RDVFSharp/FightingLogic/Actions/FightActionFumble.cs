using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionFumble : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();

            if (attacker.IsEvading > 0)
            {//Apply attack bonus from move/teleport then reset it.
                attacker.IsEvading = 0;
            }
            if (attacker.IsAggressive > 0)
            {//Only applies to 1 action, so we reset it now.
                attacker.IsAggressive = 0;
            }
            if (attacker.IsGuarding > 0)
            {//Apply attack bonus from move/teleport then reset it.
                attacker.IsGuarding = 0;
            }

            attacker.IsExposed += 2;//Fumbling exposes you.

            battlefield.OutputController.Hit.Add(" FUMBLE! ");

            // Fumbles make you lose a turn, unless your opponent fumbled on their previous one in which case nobody should lose a turn and we just clear the fumbled status on them.
            // Reminder: if fumbled is true for you, your opponent's next normal action will stun you.
            if (!target.Fumbled)
            {
                attacker.Fumbled = true;
                battlefield.OutputController.Hint.Add(attacker.Name + " loses the next action and is Exposed!");
            }
            else
            {
                target.Fumbled = false;
                battlefield.OutputController.Hint.Add("Both fighter fumbled and lost an action so it evens out, but you should still emote the fumble.");
            }

            return true;
        }
    }
}
