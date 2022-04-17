using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDVFSharp.Entities
{
    public class Fighter
    {
        public Battlefield Battlefield { get; set; }
        public BaseFighter BaseFighter { get; set; }
        public string Name
        {
            get
            {
                return BaseFighter.Name;
            }
        }

        public int Strength
        {
            get
            {
                var total = BaseFighter.Strength;
                return total;
            }
        }
        public int Dexterity
        {
            get
            {
                var total = BaseFighter.Dexterity;
                return total;
            }
        }
        public int Resilience
        {
            get
            {
                var total = BaseFighter.Resilience;
                return total;
            }
        }
        public int Spellpower
        {
            get
            {
                var total = BaseFighter.Spellpower;
                return total;
            }
        }
        public int Willpower
        {
            get
            {
                var total = BaseFighter.Willpower;
                return total;
            }
        }

        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int HPDOT { get; set; }
        public int ManaDOT { get; set; }
        public int StaminaDOT { get; set; }
        public int ManaDamage { get; set; }
        public int StaminaDamage { get; set; }
        public int Mana { get; set; }
        public int ManaCap { get; set; }
        public int MaxMana { get; set; }
        public int Stamina { get; set; }
        public int StaminaCap { get; set; }
        public int MaxStamina { get; set; }
        public int DizzyValue { get; set; }
        public int HPBurn { get; set; }
        public int ManaBurn { get; set; }
        public int StaminaBurn { get; set; }
        public int DamageEffectMult { get; set; }
        public bool IsUnconscious { get; set; }
        public bool IsDead { get; set; }
        public int CurseUsed { get; set; }
        public bool IsRestrained { get; set; }
        public bool IsRestraining { get; set; }
        public bool IsDazed { get; set; }
        public int IsStunned { get; set; }
        public int IsDisoriented { get; set; }
        public List<string> IsGrappledBy { get; set; }
        public int IsGrabbable { get; set; }
        public int IsFocused { get; set; }
        public int IsEscaping { get; set; }
        public int IsGuarding { get; set; }
        public int IsEvading { get; set; }
        public int IsAggressive { get; set; }
        public int IsExposed { get; set; }
        public bool Fumbled { get; set; }
        public int KoValue { get; set; }
        public int DeathValue { get; set; }
        public int RollTotal { get; set; }
        public int RollsMade { get; set; }
        public List<int> LastRolls { get; set; }
        public bool WantsToLeave { get; set; }
        public int LastKnownHP { get; set; }
        public int LastKnownMana { get; set; }
        public int LastKnownStamina { get; set; }
        public int SetTarget { get; set; }
        public string TeamColor { get; set; }
        public Fighter CurrentTarget { get; set; }

        public Fighter(BaseFighter baseFighter, Battlefield battlefield, string teamColor)
        {
            BaseFighter = baseFighter;
            Battlefield = battlefield;
            TeamColor = teamColor;
            KoValue = Constants.DefaultUnconsciousAt;
            DeathValue = Constants.DefaultDeadAt;

            if (!BaseFighter.AreStatsValid)
            {
                throw new Exception($"{Name} was not created due to invalid settings: {string.Join(", ", BaseFighter.GetStatsErrors())}");
            }

            MaxHP = BaseFighter.BaseMaxHP;
            MaxMana = BaseFighter.BaseMaxMana;
            ManaCap = MaxMana;
            MaxStamina = BaseFighter.BaseMaxStamina;
            StaminaCap = MaxStamina;

            DizzyValue = (int)(MaxHP * Constants.DefaultDizzyHPMultiplier); //You become dizzy at half health and below.

            ManaBurn = 0;
            StaminaBurn = 0;

            DamageEffectMult = Constants.DefaultGameSpeed;

            HP = 0;
            AddHp(MaxHP);

            Mana = 0;
            AddMana(MaxMana);

            Stamina = 0;
            AddStamina(MaxStamina);

            RollTotal = 0; // Two values that we track in order to calculate average roll, which we will call Luck on the output screen.
            RollsMade = 0; // Luck = rollTotal / rollsMade
            LastRolls = new List<int>();

            LastKnownHP = HP;
            LastKnownMana = Mana;
            LastKnownStamina = Stamina;
            IsGrabbable = 0;
            IsUnconscious = false;
            IsDead = false;
            CurseUsed = 0;
            IsRestrained = false;
            IsRestraining = false;
            IsDazed = false;
            IsStunned = 0;
            IsDisoriented = 0;
            IsGrappledBy = new List<string>();
            IsFocused = 0;
            IsEscaping = 0;//A bonus to escape attempts that increases whenever you fail one.
            IsGuarding = 0;
            IsEvading = 0;
            IsAggressive = 0;
            IsExposed = 0;
            Fumbled = false; //A status that gets set when you fumble, so that opponents next action can stun you.
            WantsToLeave = false;
        }

        public void AddHp(int hpToAdd)
        {
            var x = ~~hpToAdd;
            HP += x * DamageEffectMult;
            HP = Utils.Clamp(HP, 0, MaxHP);
        }

        public void AddMana(int manaToAdd)
        {
            var x = ~~manaToAdd;
            Mana += x;
            Mana = Utils.Clamp(Mana, 0, MaxMana);
        }

        public void AddStamina(int staminaToAdd)
        {
            var x = ~~staminaToAdd;
            Stamina += x;
            Stamina = Utils.Clamp(Stamina, 0, MaxStamina);
        }

        public void HitHp(int hpToRemove)
        {
            var x = ~~hpToRemove;
            x *= DamageEffectMult;
            HP -= x;
            HP = Utils.Clamp(HP, 0, MaxHP);
            Battlefield.OutputController.Damage = x;

            if (IsFocused > 0)
            {
                var doubleX = (double)x;
                if (IsRestrained) doubleX *= 1.5;
                if (IsDisoriented > 0) doubleX += IsDisoriented;
                IsFocused = (int)Math.Max(IsFocused - doubleX, 0);
                if (IsFocused == 0) Battlefield.OutputController.Hint.Add(Name + " has lost their focus!");
            }
        }

        public void HitMana(int manaToRemove)
        {
            var x = ~~manaToRemove;
            Mana -= x;
            Mana = Utils.Clamp(Mana, 0, ManaCap);
        }

        public void HitStamina(int staminaToRemove)
        {
            var x = ~~staminaToRemove;
            Stamina -= x;
            Stamina = Utils.Clamp(Stamina, 0, StaminaCap);
        }

        public void Regen()
        {
            if (ManaCap > MaxMana)
            {
                ManaCap = Math.Max(ManaCap - ManaBurn, MaxMana);
                ManaBurn = 10;
            }

            if (ManaCap == MaxMana) ManaBurn = 0;

            if (StaminaCap > MaxStamina)
            {
                StaminaCap = Math.Max(StaminaCap - StaminaBurn, MaxStamina);
                StaminaBurn = 10;
            }

            if (StaminaCap == MaxStamina) StaminaBurn = 0;

            if (HPBurn > 0)
            {
                AddHp(-HPDOT);
            }

            if (StaminaDamage > 0)
            {
                AddStamina(-StaminaDOT);
            }

            if (ManaDamage > 0)
            {
                AddMana(-ManaDOT);
            }

            if (IsUnconscious == false)
            {
                //Disable regeneration.
                //    var stamBonus = 6 + Willpower;
                //    this.addStamina(stamBonus);
                //    var manaBonus = 6 + Willpower;
                //    this.addMana(manaBonus);
            }
            else
            {
                IsDazed = true;
            }
        }

        public string GetStatBlock()
        {
            return "[color=cyan]" + Name + " stats: Strength: " + Strength + " Dexterity: " + Dexterity + " Resilience: " + Resilience + " Spellpower: " + Spellpower + " Willpower: " + Willpower + "[/color]";
        }

        public string GetStatus()
        {
            var hpDelta = HP - LastKnownHP;
            var staminaDelta = Stamina - LastKnownStamina;
            var manaDelta = Mana - LastKnownMana;
            double hpPercent = Math.Ceiling((double)(100 * HP / MaxHP));
            var staminaPercent = Math.Ceiling((double)(100 * Stamina / StaminaCap));
            var manaPercent = Math.Ceiling((double)(100 * Mana / ManaCap));

            var message = $"[color={TeamColor}]" + Name;
            message += "[/color][color=yellow] hit points: ";
            if (HP > DizzyValue)
            {
                message += HP;
            }
            else
            {
                message += "[color=red]" + HP + "[/color]";
            }
            if (hpDelta > 0) message += "[color=cyan] (+" + hpDelta + ")[/color]";
            if (hpDelta < 0) message += "[color=red] (" + hpDelta + ")[/color]";
            message += "|" + this.MaxHP;
            message += " (" + hpPercent + "%)";

            message += "[/color][color=green] stamina: " + Stamina;
            if (staminaDelta > 0) message += "[color=cyan] (+" + staminaDelta + ")[/color]";
            if (staminaDelta < 0) message += "[color=red] (" + staminaDelta + ")[/color]";

            message += "|";
            if (StaminaCap > MaxStamina) message += "[color=cyan]";
            message += StaminaCap;
            if (StaminaCap > MaxStamina) message += "[/color]";
            message += " (" + staminaPercent + "%)";

            message += "[/color][color=blue] mana: " + Mana;
            if (manaDelta > 0) message += "[color=cyan] (+" + manaDelta + ")[/color]";
            if (manaDelta < 0) message += "[color=red] (" + manaDelta + ")[/color]";

            message += "|";
            if (ManaCap > MaxMana) message += "[color=cyan]";
            message += ManaCap;
            if (ManaCap > MaxMana) message += "[/color]";
            message += " (" + manaPercent + "%)[/color]";

            message += "|";
            message += "[color=red] target:[/color] ";
            message += $"[color={CurrentTarget.TeamColor}]" + CurrentTarget.Name + "[/color]";

            LastKnownHP = HP;
            LastKnownMana = Mana;
            LastKnownStamina = Stamina;

            if (IsRestrained) Battlefield.OutputController.Hint.Add(Name + " is Grappled.");
            if (IsFocused > 0) Battlefield.OutputController.Hint.Add(Name + " is Focused (" + IsFocused + " points). Focus is reduced by taking damage.");
            if (IsFocused > 0) Battlefield.OutputController.Hint.Add(Name + "'s Ranged and Spell attacks have a +" + Math.Ceiling((double)IsFocused / 10) + " bonus to attack and damage because of the Focus.");
            Battlefield.DisplayGrabbed = !Battlefield.DisplayGrabbed; //only output it on every two turns
            return message;
        }

        public void UpdateCondition()
        {
            if (IsGrappledBy.Count != 0 && IsRestrained == false) IsRestrained = true;
            if (IsGrappledBy.Count == 0 && IsRestrained == true) IsRestrained = false;

            if (IsEscaping > 0 && !IsRestrained || IsEscaping < 0) IsEscaping = 0;//If you have ane scape bonus, but you're not grappled it should get cancled. And the escape bonus can't be negative either.

            if (IsEscaping > 0)
            {
                Battlefield.OutputController.Hint.Add(Name + " has a +" + IsEscaping + " escape bonus.");
            }

            //if (Stamina < rollDice([20]) && IsFocused > 0) {
            //    Battlefield.WindowController.Hint.Add(Name + " lost their focus/aim because of fatigue!");
            //    IsFocused = 0;
            //}

            if (IsEvading > 0)
            {
                Battlefield.OutputController.Hint.Add(Name + " has a temporary +" + IsEvading + " bonus to evasion and damage reduction.");
            }

            if (IsAggressive > 0)
            {
                Battlefield.OutputController.Hint.Add(Name + " has a temporary +" + IsAggressive + " bonus to accuracy and attack damage.");
            }

            if (StaminaDamage > 1)
            {
                Battlefield.OutputController.Hint.Add(Name + " is taking " + HPDOT + " damage to both Stamina and HP for " + (HPBurn - 1) + " turn(s).");
            }

            if (ManaDamage > 1)
            {
                Battlefield.OutputController.Hint.Add(Name + " is taking " + HPDOT + " damage to both Mana and HP for " + (HPBurn - 1) + " turn(s).");
            }

            if (IsGuarding > 0)
            {
                Battlefield.OutputController.Hint.Add(Name + " has a temporary +" + IsGuarding + " bonus to evasion and damage reduction.");
            }

            if (HP > DizzyValue && IsDisoriented > 0)
            {
                IsDisoriented -= 1;
                if (IsDisoriented == 0) Battlefield.OutputController.Hint.Add(Name + " has recovered and is no longer dizzy!");
            }

            if (IsExposed > 0)
            {
                IsExposed -= 1;
                if (IsExposed == 0) Battlefield.OutputController.Hint.Add(Name + " has recovered from the missed attack and is no longer Exposed!");
            }

            if (HP <= KoValue && IsUnconscious == false)
            {
                IsUnconscious = true;
                //Battlefield.WindowController.Hit.Add(Name + " is permanently Knocked Out (or extremely dizzy, and can not resist)! Feel free to use this opportunity! " + Name + " must not resist! Continue beating them to get a fatality suggestion.");
            }

            if (HP <= DeathValue && IsDead == false)
            {
                IsDead = true;
                IsGrabbable = 0;
                IsStunned = 2147483647;
                CurseUsed = 0;
                IsRestrained = false;
                IsDazed = false;
                IsDisoriented = 0;
                IsGrappledBy = new List<string>();
                IsFocused = 0;
                IsEscaping = 0;//A bonus to escape attempts that increases whenever you fail one.
                IsGuarding = 0;
                IsEvading = 0;
                IsAggressive = 0;
                IsExposed = 0;
                SetTarget = 0;
                HPDOT = 0;
                HPBurn = 0;
                foreach (var enemies in this.Battlefield.Fighters.Where(x => x.TeamColor != this.TeamColor && x.CurrentTarget == this))
                {
                    enemies.IsEscaping = 0;
                    enemies.IsRestrained = false;
                }
                
                Battlefield.OutputController.Hit.Add(Name + " has been knocked out!");
            }
        }

        public (int miss, int crit, int targethit) BuildActionTable(int difficulty, int targetDex, int attackerDex, int targetEnergy, int targetEnergyMax)
        {
            var miss = 0;
            var crit = 0;
            var targethit = 0;
            // Modify difficulty by half the difference in DEX rounded down. Each odd point more gives you +1 attack and each even point more gives you +1 defence.
            miss = difficulty + (int)Math.Floor((double)(targetDex - attackerDex) / 2);
            //Opponents who are low on energy are easier to hit. Will use Stamina for physical and Mana for magical attacks.
            miss = (int)Math.Ceiling((double)miss * targetEnergy / targetEnergyMax);
            miss = Math.Max(1, miss);//A roll of 1 is always a miss.
            miss = Math.Min(miss, 19); //A roll of 20 is always a hit, so maximum difficulty is 19.
            crit = 20;
            targethit = 7;
            return (miss, crit, targethit);
        }

        public bool CanDodge(Fighter attacker)
        {
            return !(IsGrappling(attacker) || IsGrappledBy.Count != 0);
        }

        public bool IsGrappling(Fighter target)
        {
            return target.IsGrappledBy.Contains(Name);
        }

        public void RemoveGrappler(Fighter target)
        {
            IsGrappledBy.Remove(target.Name);
        }
    }

}
