using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDVFSharp.FightingLogic
{
    public class FatalitySelect
    {
        public static string SelectRandom()
        {
            var fatalities = new List<string>() {
            "Decapitation",
            "Strangulation",
            "Beating to death",
            "Exposing internal organs",
            "Blood loss",
            "Heart damage",
            "Brain damage",
            "Breaking Neck",
            "Breaking bones",
            "Dismemberment",
            "Crushing",
            "Severing the jaw",
            "Remove top part of a head",
            "Maceration",
            "Brutality!",
            "Slow and sensual death",
            "Literally fucking to death",
            "Extremely staged and theatrical finisher"
            };

            return fatalities[Utils.GetRandomNumber(0, fatalities.Count - 1)];
        }
    }
}
