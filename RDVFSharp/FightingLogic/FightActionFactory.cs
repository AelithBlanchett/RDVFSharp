using RDVFSharp.FightingLogic.Actions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic
{
    class FightActionFactory
    {
        public static BaseFightAction Create(string actionName, bool isTeamFight = false)
        {
            switch (actionName)
            {
                case "Burn":
                    return new FightActionBurn();
                case "Channel":
                    return new FightActionChannel();
                case "Cleave":
                    ThrowIfNotTeamFight(isTeamFight);
                    return new FightActionCleave();
                case "Curse":
                    return new FightActionCleave();
                case "Focus":
                    return new FightActionFocus();
                case "Fumble":
                    return new FightActionFumble();
                case "Grab":
                    return new FightActionGrab();
                case "Guard":
                    ThrowIfNotTeamFight(isTeamFight);
                    return new FightActionGuard();
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
                case "Manastorm":
                    ThrowIfNotTeamFight(isTeamFight);
                    return new FightActionManastorm();
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
                case "Stab":
                    return new FightActionStab();
                case "Submission":
                    return new FightActionSubmission();
                case "Tackle":
                    return new FightActionTackle();
                case "Target":
                    ThrowIfNotTeamFight(isTeamFight);
                    return new FightActionTarget();
                case "Teleport":
                    return new FightActionTeleport();
                case "Throw":
                    return new FightActionThrow();
                default:
                    throw new Exception("This move doesn't exist.");
            }
        }

        private static void ThrowIfNotTeamFight(bool isTeamfight)
        {
            if (isTeamfight == false)
            {
                throw new Exception("This move isn't available.");
            }
        }
    }
}
