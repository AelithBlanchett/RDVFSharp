using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RDVFSharp.Commands
{
    public class Help : BaseCommand<RDVFPlugin>
    {
        public override string Description => "Gets the status of an ongoing fight.";

        public override async Task ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            
                Plugin.FChatClient.SendPrivateMessage(Constants.Help, character);
            
            
        }
    }
}
