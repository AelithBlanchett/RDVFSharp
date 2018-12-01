using FChatSharpLib.Entities.Plugin;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RDVFSharp
{
    public class RendezvousFighting : BasePlugin
    {

        public Battlefield CurrentBattlefield { get; set; }
        public Fighter FirstFighter { get; set; }
        public Fighter SecondFighter { get; set; }
        public RDVFDataContext Context { get; set; }

        public RendezvousFighting(RDVFDataContext context, string channel, bool debug = false, Battlefield currentBattlefield = null) : base(channel, debug)
        {
            Context = context;
            ResetFight(currentBattlefield);
        }

        public void ResetFight(Battlefield currentBattlefield = null)
        {
            if(currentBattlefield != null)
            {
                CurrentBattlefield = currentBattlefield;
            }
            else
            {
                CurrentBattlefield = new Battlefield(this);
            }
            
            FirstFighter = null;
            SecondFighter = null;
        }
    }
}
