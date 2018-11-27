using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.Errors
{
    public class FighterAlreadyExists : Exception
    {
        public FighterAlreadyExists(string fighterName) : base($"{fighterName} is already registered.")
        {
        }
    }
}
