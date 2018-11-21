using FChatSharpLib.Entities.Plugin;
using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp
{
    class RendezvousFighting : BasePlugin
    {

        public Battlefield CurrentBattlefield { get; set; }
        public BaseFighter FirstFighter { get; set; }
        public BaseFighter SecondFighter { get; set; }

        public RendezvousFighting(string channel) : base(nameof(RendezvousFighting), "1.0.0", channel)
        {
            CurrentBattlefield = new Battlefield(this);
        }
    }
}
