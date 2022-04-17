using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RDVFSharp.Entities
{
    public class BaseFighter
    {
        [Key]
        public string Name { get; set; }

        [Required]
        public int Strength { get; set; }
        [Required]
        public int Dexterity { get; set; }
        [Required]
        public int Resilience { get; set; }
        [Required]
        public int Spellpower { get; set; }
        [Required]
        public int Willpower { get; set; }

        public int BaseMaxHP
        {
            get
            {
                return 40 + Resilience * 10 + Willpower * 5;
            }
        }

        public int BaseMaxMana
        {
            get
            {
                return (Willpower * 10 + 60 + (Spellpower * 5 - (Strength * 5)));
            }
        }

        public int BaseMaxStamina
        {
            get
            {
                return (Willpower * 10 + 60 - (Spellpower * 5 - (Strength * 5)));
            }
        }

        public string Stats
        {
            get
            {
                return "[b]" + Name + "[/b]'s stats" + "\n" +
                    "[b][color=red]Strength[/color][/b]:  " + Strength + "      " + "[b][color=yellow]Hit Points[/color][/b]: " + BaseMaxHP + "\n" +
                    "[b][color=pink]Dexterity[/color][/b]: " + Dexterity + "      " + "[b][color=green]Stamina[/color][/b]: " + BaseMaxStamina + "\n" +
                    "[b][color=white]Resilience[/color][/b]:" + Resilience + "      " + "[b][color=blue]Mana[/color][/b]: " + BaseMaxMana + "\n" +
                    "[b][color=cyan]Spellpower[/color][/b]: " + Spellpower + "      " + "\n" +
                    "[b][color=purple]Willpower[/color][/b]: " + Willpower;
            }
        }

        public bool AreStatsValid
        {
            get
            {
                return GetStatsErrors().Count == 0;
            }
        }

        public List<string> GetStatsErrors()
        {
            var errors = new List<string>();
            //Check stat points for conformity to rules
            if (Strength > 10 || Strength < 0) errors.Add(Name + "'s Strength is outside the allowed range (0 to 10).");
            if (Dexterity > 10 || Dexterity < 0) errors.Add(Name + "'s Dexterity is outside the allowed range (0 to 10).");
            if (Resilience > 10 || Resilience < 0) errors.Add(Name + "'s Resilience is outside the allowed range (0 to 10).");
            if (Spellpower > 10 || Spellpower < 0) errors.Add(Name + "'s Spellpower is outside the allowed range (0 to 10).");
            if (Willpower > 10 || Willpower < 0) errors.Add(Name + "'s Willpower is outside the allowed range (0 to 10).");

            var stattotal = Strength + Dexterity + Resilience + Spellpower + Willpower;
            if (stattotal != Constants.DefaultStatPoints && Constants.DefaultStatPoints != 0) errors.Add(Name + " has stats that are too high or too low (" + stattotal + " out of " + Constants.DefaultStatPoints + " points spent).");

            return errors;
        }
    }
}
