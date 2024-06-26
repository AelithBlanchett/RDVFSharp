﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Submission : Action
    {
        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            var attacker = Plugin.GetCurrentBattlefield(channel).GetActor();
            var target = Plugin.GetCurrentBattlefield(channel).GetTarget();

            if (attacker.IsGrappling(target))
            {
                base.ExecuteCommand(character, args, channel);
            }
            else
            {
                Plugin.FChatClient.SendMessageInChannel("You can only use Submission if you are grappling your opponent.", channel);
            }
        }
    }
}
