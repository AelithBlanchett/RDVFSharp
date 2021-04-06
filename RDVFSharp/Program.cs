using System.Collections.Generic;
using System.Linq;

namespace RDVFSharp
{
    class Program
    {
        public static RendezvousFighting RDV { get; set; }
        public static bool IsDebugging { get; set; } = false;

        static void Main()
        {
#if DEBUG 
            var flistUsername = "";
            var flistPassword = "";
            var botCharacterName = ""; //The character used to log in with
            var adminName = "Aelith Blanchette";
            var channelToWatch = "adh-2bef661405a83f74cd94"; //Your testing channel code, obtainable with /code in the chat
            var bot = new FChatSharpLib.Bot(flistUsername, flistPassword, botCharacterName, adminName, true, 4000);
            bot.Connect();

            RDV = new RendezvousFighting(new DataContext.RDVFDataContext(), channelToWatch, IsDebugging);
            RDV.Run();
#else
            var channelToWatch = "ADH-b3c88050e9c580631c70"; //The actual channel you want to connect to
            RDV = new RendezvousFighting(new DataContext.RDVFDataContext(), channelToWatch, IsDebugging);
            RDV.Run();
#endif

        }
    }
}
