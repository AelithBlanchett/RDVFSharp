using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    abstract class BaseFightAction
    {
        public abstract bool Execute(int roll, Battlefield battlefield, Fighter initiator, Fighter targeted);

        public virtual bool ExecuteMultiFight(int roll, Battlefield battlefield, Fighter initiator, Fighter targeted)
        {
            return Execute(roll, battlefield, initiator, targeted);
        }
    }
}
