﻿using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Submission : Action
    {
        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            var attacker = Plugin.CurrentBattlefield.GetActor();
            var target = Plugin.CurrentBattlefield.GetTarget();

            if (attacker.IsGrappling(target))
            {
                base.ExecuteCommand(character, args, channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You can only use Submission if you are grappling your opponent.", Plugin.Channel);
            }
        }
    }
}
