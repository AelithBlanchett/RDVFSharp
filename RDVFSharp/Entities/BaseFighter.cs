using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        public int Endurance { get; set; }
        [Required]
        public int Spellpower { get; set; }
        [Required]
        public int Willpower { get; set; }
    }
}
