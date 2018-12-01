using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;

namespace RDVFSharp.Tests
{
    public class TestData
    {
        public const string DebugChannel = "ADH-MySuperChannel1234";

        public static RDVFDataContext RDVFDataContext;

        public static RDVFDataContext GetDataContext(bool reset = false)
        {
            if (RDVFDataContext != null && !reset) { return RDVFDataContext; }
            var firstFighter = new BaseFighter()
            {
                Dexterity = 4,
                Resilience = 4,
                Name = "AFighterWithValidStats",
                Endurance = 8,
                Strength = 4,
                Special = 4
            };

            var secondighter = new BaseFighter()
            {
                Dexterity = 4,
                Resilience = 4,
                Name = "AnotherFighterWithValidStats",
                Endurance = 4,
                Strength = 8,
                Special = 4
            };

            var thirdFighter = new BaseFighter()
            {
                Dexterity = 4,
                Resilience = 4,
                Name = "AFighterWithInvalidStats",
                Endurance = 4,
                Strength = 4,
                Special = 4
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

        public static RendezvousFighting GetPlugin(bool resetConnection = false)
        {
            return new RendezvousFighting(GetDataContext(resetConnection), DebugChannel, true);
        }

    }
}
