using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RDVFSharp
{
    class Program
    {
        public static RendezvousFighting RDV { get; set; }
        public static bool IsDebugging { get; set; } = false;

        static void Main(string[] args)
        {
            RDV = new RendezvousFighting(new DataContext.RDVFDataContext(), "ADH-3d64bb9568c39a2818bc", IsDebugging);
            RDV.Run();
        }
    }
}
