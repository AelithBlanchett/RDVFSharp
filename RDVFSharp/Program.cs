using System;
using System.IO;
using System.Threading.Tasks;
using FChatSharpLib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;
using Volo.Abp;

namespace RDVFSharp
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.Console())
                .CreateLogger();

            using (var application = AbpApplicationFactory.Create<RDVFPluginAppModule>(options =>
            {
                options.UseAutofac(); //Autofac integration
            }))
            {
                try
                {
                    Log.Information("Starting console host.");
                    await CreateHostBuilder(args).RunConsoleAsync();
                    return 0;
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Host terminated unexpectedly!");
                    return 1;
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }



        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseAutofac()
                .UseSerilog()
                .ConfigureAppConfiguration((context, config) =>
                {
                    //setup your additional configuration sources
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddApplication<RDVFPluginAppModule>();

                    var config = LoadConfiguration();
                    services.AddSingleton(config);

                    services.Configure<FChatSharpPluginOptions>(options => hostContext.Configuration.GetSection("Options").Bind(options));
                    services.Configure<ConnectionFactory>(options => hostContext.Configuration.GetSection("RabbitMQ").Bind(options));

                    services.AddSingleton<RemoteEvents>();
                    services.AddSingleton<RemoteBotController>();
                    services.Configure<RDVFPluginOptions>(options => config.Bind(options));

                    services.AddDbContext<DataContext.RDVFDataContext>(optionsBuilder => optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"), new MariaDbServerVersion("10.5.13")), ServiceLifetime.Transient);
                });

        public static IConfiguration LoadConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
