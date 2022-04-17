using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionThrow : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            var attacker = initiatingActor;
            var target = battlefield.GetTarget();
            var damage = Utils.RollDice(new List<int>() { 5, 5 }) - 1 + attacker.Strength;
            damage *= 2;
            var requiredStam = 10;
            var difficulty = 8; //Base difficulty, rolls greater than this amount will hit.

            if (attacker.IsRestrained) difficulty += Math.Max(0, 12 + (int)Math.Floor((double)(target.Strength - attacker.Strength) / 2)); //When grappled, up the difficulty based on the relative strength of the combatants. Minimum of +4 difficulty, maximum of +12.
            if (attacker.IsRestrained) difficulty -= attacker.IsEscaping; //Then reduce difficulty based on how much effort we've put into escaping so far.
            if (target.IsRestrained) difficulty -= 4; //Lower the difficulty considerably if the target is restrained.
            if (target.IsExposed > 0) difficulty -= 2; // If opponent left themself wide open after a failed strong attack, they'll be easier to hit.
            if (target.HPBurn > 1) difficulty -= 1;

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

            var attackTable = attacker.BuildActionTable(difficulty, target.Dexterity, attacker.Dexterity, target.Stamina, target.StaminaCap);
            //If target can dodge the atatcker has to roll higher than the dodge value. Otherwise they need to roll higher than the miss value. We display the relevant value in the output.
            battlefield.OutputController.Info.Add("Dice Roll Required: " + (attackTable.miss + 1));

            if (roll <= attackTable.miss)
            {   //Miss-- no effect.
                battlefield.OutputController.Hit.Add(" FAILED!");
                if (attacker.IsRestrained) attacker.IsEscaping += 4;//If we fail to escape, it'll be easier next time.
                attacker.IsExposed += 2; //If the fighter misses a big attack, it leaves them open and they have to recover balance which gives the opponent a chance to strike.
                battlefield.OutputController.Hint.Add(attacker.Name + " was left wide open by the failed attack and is now Exposed! " + target.Name + " has -2 difficulty to hit and can use Grab even if fighters are not in grappling range!");
                //If opponent fumbled on their previous action they should become stunned. Tackle is a special case because it stuns anyway if it hits, so we only do this on a miss.
                if (target.Fumbled)
                {
                    target.IsDazed = true;
                    target.Fumbled = false;
                }
                return false; //Failed attack, if we ever need to check that.
            }

            if (roll >= attackTable.crit && critCheck == true)
            { //Critical Hit-- increased damage/effect, typically 3x damage if there are no other bonuses.
                battlefield.OutputController.Hint.Add("Critical Hit! " + attacker.Name + " really drove that one home!");
                damage += 10;
            }

            if (attacker.IsGrappling(target))
            {
                target.RemoveGrappler(attacker);
                battlefield.InGrabRange = false;//A throw will put the fighters out of grappling range.
                if (target.IsGrappling(attacker))
                {
                    attacker.RemoveGrappler(target);
                    battlefield.OutputController.Hit.Add(attacker.Name + " gained the upper hand and THREW " + target.Name + "! " + attacker.Name + " can make another move! " + attacker.Name + " is no longer at a penalty from being grappled!");
                }
                else
                {
                    damage += Math.Max(0, 10 - target.IsEscaping);
                    battlefield.OutputController.Hit.Add(attacker.Name + " THREW " + target.Name + " and dealt bonus damage!");
                }
                //battlefield.OutputController.Hint.Add(target.Name + ", you are no longer grappled. You should make your post, but you should only emote being hit, do not try to perform any other actions.");
            }
            else if (target.IsGrappling(attacker))
            {
                attacker.RemoveGrappler(target);
                battlefield.InGrabRange = false;//A throw will put the fighters out of grappling range.
                battlefield.OutputController.Hit.Add(attacker.Name + " found a hold and THREW " + target.Name + " off! " + attacker.Name + " is no longer at a penalty from being grappled!");
                //battlefield.OutputController.Hint.Add(target.Name + ", you should make your post, but you should only emote being hit, do not try to perform any other actions.");
            }
            else
            {
                battlefield.InGrabRange = true;//A regular tackle will put you close enough to your opponent to initiate a grab.
                battlefield.OutputController.Hit.Add(attacker.Name + " TACKLED " + target.Name + ". " + attacker.Name + " can take another action while their opponent is stunned!");
                //battlefield.OutputController.Hint.Add(target.Name + ", you should make your post, but you should only emote being hit, do not try to perform any other actions.");
            }

            //Deal all the actual damage/effects here.

            damage = Math.Max(damage, 1);
            target.HitHp(damage);
            //target.IsStunned = true;
            return true; //Successful attack, if we ever need to check that.
        }
    }
}
