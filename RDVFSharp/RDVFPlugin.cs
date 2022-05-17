using FChatSharpLib;
using FChatSharpLib.Entities.Plugin;
using Microsoft.Extensions.Options;
using RDVFSharp;
using RDVFSharp.DataContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace RDVFSharp
{
    public class RDVFPlugin : BasePlugin, ISingletonDependency
    {
        public Battlefield CurrentBattlefield { get; set; }
        public IOptions<RDVFPluginOptions> RDVFPluginOptions { get; }
        public RDVFDataContext DataContext { get; }

        public RDVFPlugin(IOptions<RDVFPluginOptions> pluginOptions, RemoteBotController fChatClient, RDVFDataContext dataContext) : base(pluginOptions, fChatClient)
        {
            RDVFPluginOptions = pluginOptions;
            DataContext = dataContext;
            ResetFight(CurrentBattlefield);
        }

        public void ResetFight(Battlefield currentBattlefield = null)
        {
            if (currentBattlefield != null)
            {
                CurrentBattlefield = currentBattlefield;
            }
            else
            {
                CurrentBattlefield = new Battlefield(this);
            }
        }
    }
}
