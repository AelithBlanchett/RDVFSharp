using System;

namespace RDVFSharp
{
    class Program
    {
        public static RendezvousFighting RDV { get; set; }

        static void Main(string[] args)
        {
            RDV = new RendezvousFighting("adh-xxxxxxxx");
        }
    }
}
