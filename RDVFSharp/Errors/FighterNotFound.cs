using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.Errors
{
    public class FighterNotFound : Exception
    {
        public FighterNotFound(string fighterName) : base($"{fighterName} wasn't found.")
        {
        }
    }
}
