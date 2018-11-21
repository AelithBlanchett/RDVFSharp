using FChatSharpLib.Entities.Plugin;
using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.Commands
{
    class Ready : BaseCommand
    {
        public override string Description => "Sets a player as ready.";

        public override string ExampleUsage => "!ready";

        public override BasePlugin MyPlugin { get; set; }

        public override void ExecuteCommand(string[] args, string channel)
        {
            var rdv = (RendezvousFighting)MyPlugin;
            var context = new RDVFDataContext();
            if (rdv.FirstFighter == null)
            {
                rdv.FirstFighter = context.Fighters.Find("Aelith");
                if(rdv.FirstFighter != null)
                {
                    MyPlugin.FChatClient.SendMessageInChannel("Added first fighter Aelith", channel); //TODO 
                }
                
            }
            else
            {
                //rdv.SecondFighter = GetFighter("SecondFighter");
                MyPlugin.FChatClient.SendMessageInChannel("Added second fighter", channel);
            }
        }
    }
}
