using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.FightingLogic.Actions
{
    class FightActionSkip : BaseFightAction
    {
        public override bool Execute(int roll, Battlefield battlefield, Fighter initiatingActor, Fighter targetedActor)
        {
            battlefield.OutputController.Hit.Add(initiatingActor.Name + " skipped the turn! ");
            return false;
        }
    }
}
