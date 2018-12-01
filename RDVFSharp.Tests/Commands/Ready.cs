using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using RDVFSharp.DataContext;
using RDVFSharp.Entities;
using RDVFSharp.Errors;
using System;
using Xunit;

namespace RDVFSharp.Tests
{
    public class Ready
    {

        [Theory]
        [InlineData("MyNonExistingCharacter")]
        public void ExecuteCommand_FightAlreadyGoingOn_Fail(string characterName)
        {
            var readyCommand = new RDVFSharp.Commands.Ready();
            readyCommand.Plugin = TestData.GetPlugin();
            readyCommand.Plugin.CurrentBattlefield.IsActive = true;
            Assert.Throws<FightInProgress>(() => readyCommand.ExecuteCommand(characterName, new string[0], TestData.DebugChannel));
        }

        [Theory]
        [InlineData("MyNonExistingCharacter")]
        public void ExecuteCommand_UnregisteredCharacter_Fail(string characterName)
        {
            var readyCommand = new RDVFSharp.Commands.Ready();
            readyCommand.Plugin = TestData.GetPlugin();
            Assert.Throws<FighterNotRegistered>(() => readyCommand.ExecuteCommand(characterName, new string[0], TestData.DebugChannel));
        }

        [Fact]
        public async void ExecuteCommand_SameCharacterShouldntJoinTwice_Fail()
        {
            var readyCommand = new RDVFSharp.Commands.Ready();
            readyCommand.Plugin = TestData.GetPlugin();
            var fighter = await readyCommand.Plugin.Context.Fighters.FirstOrDefaultAsync();
            readyCommand.ExecuteCommand(fighter.Name, new string[0], TestData.DebugChannel);
            Assert.Throws<FighterAlreadyExists>(() => readyCommand.ExecuteCommand(fighter.Name, new string[0], TestData.DebugChannel));
        }
    }
}
