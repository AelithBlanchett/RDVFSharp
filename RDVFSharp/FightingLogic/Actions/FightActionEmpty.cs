using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionEmpty : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            return false;
        }
    }
}
