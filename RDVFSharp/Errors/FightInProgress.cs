using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.Errors
{
    public class FightInProgress : Exception
    {
        public FightInProgress() : base($"A fight that you are not participating in is already in progress.")
        {
        }
    }
}
