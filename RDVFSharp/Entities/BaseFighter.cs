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
                    "[b][color=red]Strength[/color][/b]:  " + Strength + "      " + "[b][color=green]Hit Points[/color][/b]: " + BaseMaxHP + "\n" +
                    "[b][color=pink]Dexterity[/color][/b]: " + Dexterity + "      " + "[b][color=yellow]Stamina[/color][/b]: " + BaseMaxStamina + "\n" +
                    "[b][color=white]Resilience[/color][/b]:" + Resilience + "      " + "[b][color=blue]Mana[/color][/b]: " + BaseMaxMana + "\n" +
                    "[b][color=cyan]Spellpower[/color][/b]: " + Spellpower + "      " + "\n" +
                    "[b][color=purple]Willpower[/color][/b]: " + Willpower;
            }
        }

        public string StatErrors
        {
            get
            {
                return GetStatsErrors(Strength, Dexterity, Resilience, Spellpower, Willpower).JoinAsString("\n");
            }
        }

        public static List<string> GetStatsErrors(int strength, int dexterity, int resilience, int spellpower, int willpower)
        {
            var errors = new List<string>();
            //Check stat points for conformity to rules
            if (strength > 10 || strength < 0) errors.Add("Strength is outside the allowed range (0 to 10).");
            if (dexterity > 10 || dexterity < 0) errors.Add("Dexterity is outside the allowed range (0 to 10).");
            if (resilience > 10 || resilience < 0) errors.Add("Resilience is outside the allowed range (0 to 10).");
            if (spellpower > 10 || spellpower < 0) errors.Add("Spellpower is outside the allowed range (0 to 10).");
            if (willpower > 10 || willpower < 0) errors.Add("Willpower is outside the allowed range (0 to 10).");

            var stattotal = strength + dexterity + resilience + spellpower + willpower;
            if (stattotal != Constants.DefaultStatPoints && Constants.DefaultStatPoints != 0) errors.Add("You have stats that are too high or too low (" + stattotal + " out of " + Constants.DefaultStatPoints + " points spent).");

            return errors;
        }
    }
}
