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

        public RendezvousFighting(string channel, bool debug = false) : base(channel, debug)
        {
            Console.WriteLine(string.Join(", ", GetCommandList()));
            CurrentBattlefield = new Battlefield(this);
            Context = new RDVFDataContext();
            Run();
        }
    }
}
