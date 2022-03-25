using RDVFSharp.FightingLogic.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic
{
    class FightActionFactory
    {
        public static BaseFightAction Create(string actionName)
        {
            switch (actionName)
            {
                case "Channel":
                    return new FightActionChannel();
                case "Focus":
                    return new FightActionFocus();
                case "Fumble":
                    return new FightActionFumble();
                case "Grab":
                    return new FightActionGrab();
                case "Heavy":
                    return new FightActionHeavy();
                case "Hex":
                    return new FightActionHex();
                case "Light":
                    return new FightActionLight();
                case "Magic":
                    return new FightActionMagic();
                case "Mana":
                    return new FightActionMana();
                case "Move":
                    return new FightActionMove();
                case "Ranged":
                    return new FightActionRanged();
                case "Rest":
                    return new FightActionRest();
                case "Skip":
                    return new FightActionSkip();
                case "Spell":
                    return new FightActionSpell();
                case "Submission":
                    return new FightActionSubmission();
                case "Tackle":
                    return new FightActionTackle();
                case "Teleport":
                    return new FightActionTeleport();
                case "Throw":
                    return new FightActionThrow();
                default:
                    return new FightActionEmpty();
            }
        }
    }
}
