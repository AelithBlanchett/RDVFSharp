using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.Entities
{
    class BaseFighter
    {
        public string Name { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Endurance { get; set; }
        public int Spellpower { get; set; }
        public int Willpower { get; set; }
    }
}
