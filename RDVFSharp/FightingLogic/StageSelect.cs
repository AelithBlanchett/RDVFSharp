using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDVFSharp.FightingLogic
{
    public class StageSelect
    {
        public static string SelectRandom()
        {
            var stages = new List<string>() {
            "The Pit",
            "RF:Wrestling Ring",
            "Arena",
            "Subway",
            "Skyscraper Roof",
            "Forest",
            "Cafe",
            "Street road",
            "Alley",
            "Park",
            "RF:MMA Hexagonal Cage",
            "Hangar",
            "Swamp",
            "RF:Glass Box",
            "RF:Free Space",
            "Magic Shop",
            "Locker Room",
            "Library",
            "Pirate Ship",
            "Baazar",
            "Supermarket",
            "Night Club",
            "Docks",
            "Hospital",
            "Dark Temple",
            "Restaurant",
            "Graveyard",
            "Zoo",
            "Slaughterhouse",
            "Junkyard",
            "Theatre",
            "Circus",
            "Castle",
            "Museum",
            "Beach",
            "Bowling Club",
            "Concert Stage",
            "Wild West Town",
            "Movie Set"
            };

            return stages[Utils.GetRandomNumber(0, stages.Count - 1)];
        }
    }
}
