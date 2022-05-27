using FChatSharpLib.Entities.Plugin.Commands;
using RDVFSharp.Errors;
using System.Collections.Generic;

namespace RDVFSharp.Commands
{
    public class Help : BaseCommand<RendezvousFighting>
    {
        public override string Description => "Gets the status of an ongoing fight.";

        public override void ExecuteCommand(string character, IEnumerable<string> args, string channel)
        {
            
                Plugin.FChatClient.SendPrivateMessage("[u][b]General Commands[/b][/u]" + "\n" +
                    "\n" +
                    "[b]!ready[/b]: Inserts your character into the battlefield. Preferably followed by one of four colours (red/blue/yellow/purple). And also preferably done in such a way that each team member readies right after each other, so that the status looks better. Example: !ready red" + "\n" +
                    "[b]!start[/b]: Starts the match." + "\n" +
                    "[b]!register[/b]: Registers you with the fight bot. Must be followed by your STR, DEX, RES, SPW, and WIL values, in that order. Example: !register 5 8 8 1 2" + "\n" +
                    "[b]!restat[/b]: Changes your stats on the fight bot. Must be followed by your STR, DEX, RES, SPW, and WIL values, in that order. Example: !register 5 8 8 1 2" + "\n" +
                    "[b]!stats[/b]: The bot will PM you with your stats." + "\n" +
                    "[b]!forfeit[/b]: Ends the match, and declares your opponent the winner." + "\n" +
                    "[b]!leave[/b]: If you've readied, but there is no match going on, this clears the arena. Otherwise, if all the participants in an ongoing match type this, the fight becomes null and void." + "\n" +
                    "[b]!commands/!help[/b]: Makes the bot PM all the commands available to you." + "\n" +
                    "\n" +
                    "\n" +
                    "[u][b]Infight commands[/b][/u]" + "\n" +
                    "[b]!target[/b]: Changes your target. Must be followed by the name of the person you wish to target. Example: !target Mayank. This DOES NOT USE YOUR TURN UP." + "\n" +
                    "[b]!light[/b]: A medium damage STR based attack that does damage to both HP and stamina." + "\n" +
                    "[b]!heavy[/b]: A high damage STR based attack that does damage to HP. Leaves you exposed on a miss." + "\n" +
                    "[b]!ranged[/b]: A high damage STR and DEX based attack that does damage to HP. Harder to hit than a !heavy, but does not leave you exposed." + "\n" +
                    "[b]!cleave[/b]: A high damage STR based attack that does damage to your target's entire team." + "\n" +
                    "[b]!tackle[/b]: A low damage STR based attack that stuns your opponents on hit, giving you another turn, and moves you into grappling range." + "\n" +
                    "[b]!grab[/b]: A low damage STR based attack that grapples your opponent." + "\n" +
                    "[b]!throw[/b]: A high damage STR based attack that frees you of being grabbed, or does extra damage if you're grappling the opponent. Ends the 'grabbed' status." + "\n" +
                    "[b]!submission[/b]: A fixed damage finisher (30 damage) that can only be used when grappling." + "\n" +
                    "[b]!stab[/b]: An STR based damage over time move that does damage to both HP and stamina." + "\n" +
                    "[b]!move[/b]: A DEX based move that provides a one time attack buff when not grabbed, or a defensive buff when escaping from a grapple." + "\n" +
                    "[b]!hex[/b]: A medium damage SPW based attack that does damage to both HP and stamina." + "\n" +
                    "[b]!magic[/b]: A high damage SPW based attack that does damage to HP. Leaves you exposed on a miss." + "\n" +
                    "[b]!spell[/b]: A high damage SPW based attack that does damage to HP. Harder to hit than a !heavy, but does not leave you exposed." + "\n" +
                    "[b]!manastorm[/b]: A high damage SPW based attack that does damage to your target's entire team. Does double the damage to the target than the rest of the team." + "\n" +
                    "[b]!curse[/b]: A single use (Even if it misses, it counts!) maximum damage magic attack." + "\n" +
                    "[b]!drain[/b]: An SPW based damage over time move that does damage to both HP and stamina." + "\n" +
                    "[b]!teleport[/b]: An SPW based move that provides a one time attack buff when not grabbed, or a defensive buff when escaping from a grapple." + "\n" +
                    "[b]!rest[/b]: A move that regains stamina." + "\n" +
                    "[b]!channel[/b]: A move that regains mana." + "\n" +
                    "[b]!focus[/b]: A move that adds damage to ranged or spell attacks. Lowers as you take damage from your opponent: And can be stacked." + "\n" +
                    "[b]!guard[/b]: A WIL based move that provides a defensive buff to your character.", character);
            
            
        }
    }
}
