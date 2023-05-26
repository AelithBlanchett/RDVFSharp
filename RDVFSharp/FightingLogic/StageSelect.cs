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
            "Train",
            "Skyscraper Roof",
            "Forest",
            "Cafe",
            "Bathhouse",
            "Street road",
            "Church",
            "Alley",
            "Park",
            "RF:MMA Hexagonal Cage",
            "Hangar",
            "Magic Shop",
            "Locker Room",
            "Library",
            "Pirate Ship",
            "Baazar",
            "Supermarket",
            "Night Club",
            "Docks",
            "Volcano",
            "Hospital",
            "Apartment",
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
            "Movie Set",
            "Furniture Store",
            "Classroom",
            "RF:Free Space"
            };

            return stages[Utils.GetRandomNumber(0, stages.Count - 1)];
        }
    }
}
