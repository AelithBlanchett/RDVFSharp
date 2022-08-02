using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp
{
    static class Constants
    {
        public const int DefaultStatPoints = 24;
        public const int DefaultGameSpeed = 1;
        public const int DefaultDisorientedAt = 50;
        public const int DefaultUnconsciousAt = 0;
        public const int DefaultDeadAt = 0;
        public const double DefaultDizzyHPMultiplier = 0.5;

        public const string VCAdvertisement = @"[color=red][sub]☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰[/sub][/color]
𝑇ℎ𝑖𝑠 𝑚𝑎𝑡𝑐ℎ 𝑖𝑠 𝑠𝑝𝑜𝑛𝑠𝑜𝑟𝑒𝑑 𝑏𝑦 𝓥𝓮𝓵𝓿𝓮𝓽𝓒𝓾𝓯𝓯.
Click [url=https://mydommewallet.velvetcuff.me/app/main/fightingleague]here[/url] to check out the latest fights and submit your own results.
Register [url=https://mydommewallet.velvetcuff.me/app/main/register?referringCharacterId=Aelith%20Blanchette]now[/url] and submit your wins/losses as Hardcore fights with logs to start earning money!
Conditions: +18 Good human(-oids) profiles only [sub]no furries, sorry[/sub]
[color=red][sub]☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰☰[/sub][/color]";


        public const string Help = "[u][b]IF YOU WANT TO SEE A COMPLETE DETAILED DESCRIPTION OF THESE COMMANDS, PLEASE LOOK AT THE ACTION DETAILS LIST HERE:[/b][/u] [icon]Rendezvous Fight[/icon]" + "\n" +
                    "[u][b]General Commands[/b][/u]" + "\n" +
                    "\n" +
                    "[b]!ready[/b]: Inserts your character into the battlefield. Preferably followed by one of four colours (red/blue/yellow/purple). And also preferably done in such a way that each team member readies right after each other, so that the status looks better. Example: !ready red. Not usable in the bar!" + "\n" +
                    "[b]!start[/b]: Starts the match." + "\n" +
                    "[b]!register[/b]: Registers you with the fight bot. Must be followed by your STR, DEX, RES, SPW, and WIL values, in that order. Example: !register 5 8 8 1 2" + "\n" +
                    "[b]!restat[/b]: Changes your stats on the fight bot. Must be followed by your STR, DEX, RES, SPW, and WIL values, in that order. Example: !register 5 8 8 1 2" + "\n" +
                    "[b]!stats[/b]: The bot will PM you with your stats." + "\n" +
                    "[b]!forfeit[/b]: Ends the match, and declares your opponent the winner." + "\n" +
                    "[b]!status[/b]: Shows you the last message sent by the bot in the fight. If the bot does not send a status message in response to your command, or to check if a fight is occurring in the venue/arena, please use this! Now also comes with a '!status Arena'/'!status Venue' command which can be used in the bar to see these same messages, or just use '!status' in the bar to see if a fight is happening in any of the fight rooms!" + "\n" +
                    "[b]!leave[/b]: If you've readied, but there is no match going on, this clears the arena. Otherwise, if all the participants in an ongoing match type this, the fight becomes null and void." + "\n" +
                    "[b]!help/!commands[/b]: Makes the bot PM all the commands available to you." + "\n" +
                    "[b]!roomcreate[/b]: Makes a new private room for you to invite people to! Invite them using the '/invite name' command in the room, and it will come up on their console. Only usable in the bar. Also, remember to add the name of the room after the command! Example: !roomcreate Mayank vs Silver. Otherwise, the room will just be named 'RDVF - '" + "\n" +
                    "[b]!looking[/b]: Add yourself to a list of people looking for a fight! This command expires in 2 hours, though it will let you know when it disappears so you can reset the timer by doing the command again! Only usable in the bar" + "\n" +
                    "[b]!look[/b]: Look at the list of people that are looking for a fight! Only usable in the bar" + "\n" +
                    "\n" +
                    "\n" +
                    "[u][b]Infight commands[/b][/u]" + "\n" +
                    "[b]!target[/b]: Changes your target. Must be followed by the name of the person you wish to target. Example: !target Mayank. THIS DOES NOT USE YOUR TURN UP. Please, always double check your targets to make sure they are correct!" + "\n" +
                    "[b]!light[/b]: A medium damage STR based attack that does damage to both HP and stamina." + "\n" +
                    "[b]!heavy[/b]: A high damage STR based attack that does damage to HP. Leaves you exposed on a miss." + "\n" +
                    "[b]!ranged[/b]: A high damage STR and DEX based attack that does damage to HP. Harder to hit than a !heavy, but does not leave you exposed." + "\n" +
                    "[b]!cleave[/b]: A high damage STR based attack that does damage to your target's entire team. Only usable in fights with more than 2 fighters, but only recommended for use in team fights!" + "\n" +
                    "[b]!tackle[/b]: A low damage STR based attack that stuns your opponents on hit, giving you another turn, and moves you into grappling range." + "\n" +
                    "[b]!grab[/b]: A low damage STR based attack that grapples your opponent." + "\n" +
                    "[b]!throw[/b]: A high damage STR based attack that frees you of being grabbed, or does extra damage if you're grappling the opponent. Ends the 'grabbed' status." + "\n" +
                    "[b]!submission[/b]: A fixed damage finisher (30 damage when opponent is below 50% HP, and 15 damage if they're at or above 50%) that can only be used when grappling." + "\n" +
                    "[b]!stab[/b]: An STR based damage over time move that does damage to both HP and stamina." + "\n" +
                    "[b]!move[/b]: A DEX based move that provides a one time attack buff when not grabbed, or a defensive buff when escaping from a grapple." + "\n" +
                    "[b]!hex[/b]: A medium damage SPW based attack that does damage to both HP and mana." + "\n" +
                    "[b]!magic[/b]: A high damage SPW based attack that does damage to HP. Leaves you exposed on a miss." + "\n" +
                    "[b]!spell[/b]: A high damage SPW based attack that does damage to HP. Harder to hit than a !heavy, but does not leave you exposed." + "\n" +
                    "[b]!manastorm[/b]: A high damage SPW based attack that does damage to your target's entire team. Does double the damage to the target than the rest of the team. Only usable in fights with more than 2 fighters, but only recommended for use in team fights!" + "\n" +
                    "[b]!curse[/b]: A single use (Even if it misses, it counts!) maximum damage magic attack. Note: Do not use this move if you have too many points in STR, as they are subtracted from the overall damage, rather than added to it!" + "\n" +
                    "[b]!drain[/b]: An SPW based damage over time move that does damage to both HP and mana." + "\n" +
                    "[b]!teleport[/b]: An SPW based move that provides a one time attack buff when not grabbed, or a defensive buff when escaping from a grapple." + "\n" +
                    "[b]!rest[/b]: A move that regains stamina." + "\n" +
                    "[b]!channel[/b]: A move that regains mana." + "\n" +
                    "[b]!focus[/b]: A move that adds damage to ranged or spell attacks. Lowers as you take damage from your opponent: And can be stacked." + "\n" +
                    "[b]!guard[/b]: A WIL based move that provides a defensive buff to your character. Only usable in fights with more than 2 fighters!";
    }
}
