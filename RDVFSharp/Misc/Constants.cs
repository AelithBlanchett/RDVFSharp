using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp
{
    static class Constants
    {
        public const string MayankAdmin = "Mayank";
        public const string EliseAdmin = "Elise Pariat";
        public const string AelithAdmin = "Aelith Blanchette";
        public const string LeonDuChain = "Leon DuChain";
        public const string CantTouchThis = "Cant Touch This";
        public const string VCBot = "VelvetCuff";
        public const string RDVFBar = "adh-a823a4e998a2b3d31794";
        public const string RDVFArena = "adh-b3c88050e9c580631c70";
        public const string RDVFVenue = "adh-51710b5ac8cce7e99f19";
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


        public const string Help = @"
[u][b]IF YOU WANT TO SEE A COMPLETE DETAILED DESCRIPTION OF THESE COMMANDS, PLEASE LOOK AT THE ACTION DETAILS LIST HERE:[/b][/u] [icon]Rendezvous Fight[/icon]

[u][b]General Commands[/b][/u]
[b]!ready[/b]: Inserts your character into the battlefield. Preferably followed by one of four colours (red/blue/yellow/purple). And also preferably done in such a way that each team member readies right after each other, so that the status looks better. Example: !ready red. Not usable in the bar!
[b]!start[/b]: Starts the match! (PS: If you type the name of an arena after !start, then you choose the arena! E.G. typing '!start COLISEUM' will name the arena 'COLISEUM'!
[b]!register[/b]: Registers you with the fight bot. Must be followed by your STR, DEX, RES, SPW, and WIL values, in that order. Example: !register 5 8 8 1 2
[b]!restat[/b]: Changes your stats on the fight bot. Must be followed by your STR, DEX, RES, SPW, and WIL values, in that order. Example: !restat 5 8 8 1 2
[b]!stats[/b]: The bot will PM you with your stats.
[b]!forfeit[/b]: Ends the match, and declares your opponent the winner.
[b]!status[/b]: Shows you the last message sent by the bot in the fight. If the bot does not send a status message in response to your command, or to check if a fight is occurring in the venue/arena, please use this! Now also comes with a '!status Arena'/'!status Venue' command which can be used in the bar to see these same messages, or just use '!status' in the bar to see if a fight is happening in any of the fight rooms!
[b]!leave[/b]: If you've readied, but there is no match going on, this clears the arena. Otherwise, if all the participants in an ongoing match type this, the fight becomes null and void.
[b]!help/!commands[/b]: Makes the bot PM all the commands available to you.
[b]!createroom[/b]: Makes a new private room for you to invite people to! Invite them using the '/invite name' command in the room, and it will come up on their console. Only usable in the bar. Also, remember to add the name of the room after the command! Example: !createroom Mayank vs Silver. Otherwise, the room will just be named 'RDVF - '
[b]!looking[/b]: Add yourself to a list of people looking for a fight! This command expires in 2 hours, though it will let you know when it disappears so you can reset the timer by doing the command again! Only usable in the bar
[b]!look[/b]: Look at the list of people that are looking for a fight! Only usable in the bar


[u][b]Infight commands[/b][/u]
[u][b]Physical Attacks[/b][/u]
[b]!light[/b]: A medium damage STR based attack that does damage to both HP and stamina.
[b]!heavy[/b]: A high damage STR based attack that does damage to HP. Leaves you exposed on a miss.
[b]!ranged[/b]: A high damage STR and DEX based attack that does damage to HP. Harder to hit than a !heavy, but does not leave you exposed on a miss.
[b]!tackle[/b]: A low damage STR based attack that stuns your opponents on hit, giving you another turn, and moves you into grappling range. Leaves you exposed on a miss.
[b]!grab[/b]: A low damage STR based attack that grapples your opponent.
[b]!throw[/b]: A high damage STR based attack that ends the 'grabbed' status for both you and the opponent, and does bonus damage if your opponent is not grabbing you, but you’re grabbing them.
[b]!submission[/b]: A fixed damage finisher (30 damage) that can only be used when grappling. If grappled whilst using it: Forces your opponent to release you. Note: This is a high stamina cost move (25 stamina), and the difficulty is also affected by STR, not just DEX like every other move. 
[b]!stab[/b]: A DEX based damage over time move that does damage to both HP and stamina when the opponent takes an action.

[u][b]Magic Attacks[/b][/u]
[b]!hex[/b]: A medium damage SPW based attack that does damage to both HP and mana.
[b]!magic[/b]: A high damage SPW based attack that does damage to HP. Leaves you exposed on a miss.
[b]!spell[/b]: A high damage SPW based attack that does damage to HP. Harder to hit than a !magic, but does not leave you exposed on a miss.
[b]!curse[/b]: A single use (Even if it misses, it counts!) maximum damage spell attack. Note: Do not use this move if you have too many points in STR, as they are subtracted from the overall damage, rather than added to it!
[b]!drain[/b]: An SPW based damage over time move that does damage to both HP and mana when the opponent takes an action.

[u][b](De)Buffs/Recovery[/b][/u]
[b]!move[/b]: A DEX based move that provides a one time attack buff when not grabbed, or a defensive buff when escaping from a grapple.
[b]!teleport[/b]: An SPW based move that provides a one time attack buff when not grabbed, or a defensive buff when escaping from a grapple.
[b]!rest[/b]: A WIL based move that regains stamina.
[b]!channel[/b]: A WIL based move that regains mana.
[b]!focus[/b]: A WIL based move that adds damage to ranged or spell attacks. Lowers as you take damage from your opponent: And can be stacked.
[b]!skip[/b]: Skips your current turn.
[b]!fumble[/b]: Skips your current turn, and leaves you exposed.
[b]!forfeit[/b]: Immediately reduces your HP to 0.

[u][b]Team/Free For All specific commands[/b][/u]
[b]!target[/b]: Changes your target. Must be followed by the name of the person you wish to target. Example: !target Mayank. THIS DOES NOT USE YOUR TURN UP. Please, always double check your targets to make sure they are correct!
[b]!guard[/b]: A WIL based move that provides a defensive buff to your character. Only usable in fights with more than 2 fighters!
[b]!cleave[/b]: A high damage STR based attack that does damage to your target's entire team. Only usable in fights with more than 2 fighters, but only recommended for use in team fights!
[b]!manastorm[/b]: A high damage SPW based attack that does damage to your target's entire team. Does double the damage to the target than the rest of the team. Only usable in fights with more than 2 fighters, but only recommended for use in team fights!
";
    }
}
