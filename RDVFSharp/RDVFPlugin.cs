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
        public Dictionary<string, Battlefield> CurrentBattlefields { get; set; } = new Dictionary<string, Battlefield>();
        public IOptions<RDVFPluginOptions> RDVFPluginOptions { get; }
        public RDVFDataContext DataContext { get; }

        public RDVFPlugin(IOptions<RDVFPluginOptions> pluginOptions, RemoteBotController fChatClient, RDVFDataContext dataContext) : base(pluginOptions, fChatClient)
        {
            RDVFPluginOptions = pluginOptions;
            DataContext = dataContext;
            ResetAllFight();
        }

        public Battlefield GetCurrentBattlefield(string channel)
        {
            if (!CurrentBattlefields.ContainsKey(channel.ToLower()))
            {
                ResetFight(channel.ToLower());
            }

            return CurrentBattlefields[channel.ToLower()];
        }

        public void ResetAllFight()
        {
            foreach (var channel in RDVFPluginOptions.Value.Channels)
            {
                ResetFight(channel.ToLower());
            }
        }

        public void ResetFight(string channel)
        {
            if (CurrentBattlefields.ContainsKey(channel.ToLower()))
            {
                CurrentBattlefields[channel.ToLower()] = new Battlefield(this, channel.ToLower());
            }
            else
            {
                CurrentBattlefields.Add(channel.ToLower(), new Battlefield(this, channel.ToLower()));
            }
        }
    }
}
