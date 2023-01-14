using Castle.Core.Logging;
using FChatSharpLib;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using RabbitMQ.Client;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using System.Collections.Generic;

namespace RDVFSharp.Tests
{
    public class TestData
    {
        public const string DebugChannel = "ADH-MySuperChannel1234";

        public static RDVFDataContext RDVFDataContext;

        public static RDVFDataContext GetDataContext(bool reset = true)
        {
            if (RDVFDataContext != null && !reset) { return RDVFDataContext; }
            var firstFighter = new BaseFighter()
            {
                Dexterity = 4,
                Resilience = 4,
                Name = "AFighterWithValidStats",
                Spellpower = 8,
                Strength = 4,
                Willpower = 4
            };

            var secondighter = new BaseFighter()
            {
                Dexterity = 4,
                Resilience = 4,
                Name = "AnotherFighterWithValidStats",
                Spellpower = 4,
                Strength = 8,
                Willpower = 4
            };

            var thirdFighter = new BaseFighter()
            {
                Dexterity = 4,
                Resilience = 4,
                Name = "AFighterWithInvalidStats",
                Spellpower = 4,
                Strength = 4,
                Willpower = 4
            };

            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var connectionOptions = new DbContextOptionsBuilder<RDVFDataContext>()
            .UseSqlite(connection)
            .Options;

            // Create the schema in the database
            var context = new RDVFDataContext(connectionOptions);
                context.Database.EnsureCreated();
            context.Fighters.Add(firstFighter);
            context.Fighters.Add(secondighter);
            context.Fighters.Add(thirdFighter);
            context.SaveChanges();
            RDVFDataContext = context;

            return RDVFDataContext;
        }

        public static RDVFPlugin GetPlugin(bool resetConnection = false)
        {
            var options = Options.Create<RDVFPluginOptions>(new RDVFPluginOptions()
            {
                Channels = new List<string>() { DebugChannel },
                Debug = true
            });
            var optionsRabbit = Options.Create<ConnectionFactory>(new ConnectionFactory());
            var loggerRemoteEvents = Substitute.For<ILogger<RemoteEvents>>();
            var logger = Substitute.For<ILogger<RDVFPlugin>>();

            return new RDVFPlugin(options, new FChatSharpLib.RemoteBotController(new RemoteEvents(options, optionsRabbit, loggerRemoteEvents)), GetDataContext(), logger);
        }

    }
}
