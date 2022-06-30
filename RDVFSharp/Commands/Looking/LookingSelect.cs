using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class LookingSelect
    {
        public static string SelectRandom()
        {
            var LookingMessage = new List<string>() {
            "is ready to rumble!",
            "is looking for payback!",
            "stretches and flexes, hand raised in a \"Bring it on\" gesture!",
            "is ready if you are!",
            "is looking to brawl!",
            "is raring for a fight!",
            "is hungry for blood!",
            "\"Who's ass needs a pounding?\"",
            "\"Anyone looking to throw down and see who ends up on top?\"",
            "is looking for a fight!",
            "\"Who needs to get pinned nice and hard?\"",
            "is here to kick some ass!",
            "\"I've got two fists, and they've both got your name on em!\""
            };

            return LookingMessage[Utils.GetRandomNumber(0, LookingMessage.Count - 1)];
        }
    }
}
