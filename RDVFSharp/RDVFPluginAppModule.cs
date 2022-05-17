using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace RDVFSharp
{

    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    public class RDVFPluginAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHostedService<RDVFPluginHostedService>();
        }

        public const string RemoteServiceName = "Default";
    }
}
