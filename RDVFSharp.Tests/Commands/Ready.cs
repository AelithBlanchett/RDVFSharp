using RDVFSharp.Errors;
using System;
using Xunit;

namespace RDVFSharp.Tests
{
    public class Ready
    {
        [Fact]
        public void ExecuteCommand_UnregisteredCharacter_Fail()
        {
            var readyCommand = new RDVFSharp.Commands.Ready();
            readyCommand.Plugin = new RendezvousFighting(Constants.DebugChannel);
            Assert.Throws<FighterNotFound>(() => readyCommand.ExecuteCommand("MyNonExistingCharacter", null, Constants.DebugChannel));
        }
    }
}
