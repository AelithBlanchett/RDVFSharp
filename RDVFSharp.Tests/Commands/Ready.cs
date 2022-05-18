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
        public async void ExecuteCommand_FightAlreadyGoingOn_Fail(string characterName)
        {
            var readyCommand = new RDVFSharp.Commands.Ready();
            readyCommand.Plugin = TestData.GetPlugin();
            readyCommand.Plugin.CurrentBattlefield.IsInProgress = true;
            await Assert.ThrowsAsync<FightInProgress>(async () => await readyCommand.ExecuteCommand(characterName, new string[0], TestData.DebugChannel));
        }

        [Theory]
        [InlineData("MyNonExistingCharacter")]
        public async void ExecuteCommand_UnregisteredCharacter_Fail(string characterName)
        {
            var readyCommand = new RDVFSharp.Commands.Ready();
            readyCommand.Plugin = TestData.GetPlugin();
            await Assert.ThrowsAsync<FighterNotRegistered>(async () => await readyCommand.ExecuteCommand(characterName, new string[0], TestData.DebugChannel));
        }

        [Theory]
        [InlineData("AnotherFighterWithValidStats")]
        public async void ExecuteCommand_SameCharacterShouldntJoinTwice_Fail(string characterName)
        {
            var readyCommand = new RDVFSharp.Commands.Ready();
            readyCommand.Plugin = TestData.GetPlugin();
            await readyCommand.ExecuteCommand(characterName, new string[0], TestData.DebugChannel);
            await Assert.ThrowsAsync<FighterAlreadyExists>(async () => await readyCommand.ExecuteCommand(characterName, new string[0], TestData.DebugChannel));
        }
    }
}
