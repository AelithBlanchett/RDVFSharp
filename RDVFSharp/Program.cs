using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace RDVFSharp
{
    class Program
    {
        public static RendezvousFighting RDV { get; set; }
        public static IConfiguration Configuration { get; set; }
        public static bool IsDebugging { get; set; } = true;

        static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
            RDV = new RendezvousFighting("adh-aede471e10a05cf1d1ae", IsDebugging);
        }
    }
}
