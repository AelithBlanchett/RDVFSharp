using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.Errors
{
    public class FighterNotRegistered : Exception
    {
        public FighterNotRegistered(string fighterName) : base($"{fighterName} isn't registered yet.")
        {
        }
    }
}
