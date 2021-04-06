using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RDVFSharp.Entities
{
    public class BaseFight
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long FightId { get; set; }

        [Required]
        public string Room { get; set; }
        [Required]
        public string WinnerId { get; set; }
        [Required]
        public string LoserId { get; set; }
        [Required]
        public DateTime FinishDate { get; set; }

        [ForeignKey(nameof(WinnerId))]
        public BaseFighter Winner { get; set; }
        [ForeignKey(nameof(LoserId))]
        public BaseFighter Loser { get; set; }
    }
}
