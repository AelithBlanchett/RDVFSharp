using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RDVFSharp
{
    public class Battlefield
    {
        public List<Fighter> Fighters { get; set; }
        public string Stage { get; set; }
        public bool DisplayGrabbed { get; set; }
        public WindowController WindowController { get; set; }

        private int currentFighter = 0;
        public bool InGrabRange { get; set; }
        public RendezvousFighting Plugin { get; }

        public bool IsInFight(string character)
        {
            return Fighters.Any(x => x.Name.ToLower() == character.ToLower());
        }

        public Fighter GetFighter(string character)
        {
            return Fighters.FirstOrDefault(x => x.Name.ToLower() == character.ToLower());
        }

        public Fighter GetFighterTarget(string character)
        {
            return Fighters.FirstOrDefault(x => x.Name.ToLower() != character.ToLower());
        }

        public bool IsActive { get; set; }

        public Battlefield(RendezvousFighting plugin)
        {
            Plugin = plugin;
            WindowController = new WindowController();
            Fighters = new List<Fighter>();
            Stage = PickStage();
            InGrabRange = false;
            DisplayGrabbed = true;
            IsActive = false;
        }

        public void InitialSetup(Fighter firstFighter, Fighter secondFighter)
        {
            Fighters.Clear();
            Fighters.Add(firstFighter);
            Fighters.Add(secondFighter);

            PickInitialActor();
            WindowController.Hit.Add("Game started!");
            WindowController.Hit.Add("FIGHTING STAGE: " + Stage + " - " + GetActor().Name + " goes first!");
            OutputFighterStatus(); // Creates the fighter status blocks (HP/Mana/Stamina)
            OutputFighterStats(); // Creates the fighter stat blocks (STR/DEX/END/INT/WIL)
            WindowController.Info.Add("[url=http://www.f-list.net/c/rendezvous%20fight/]Visit this page for game information[/url]");
            IsActive = true;
            WindowController.UpdateOutput(this);
        }

        public BaseFight EndFight(Fighter victor, Fighter loser)
        {
            var fightResult = new BaseFight()
            {
                Room = Plugin.Channel,
                WinnerId = victor.Name,
                LoserId = loser.Name,
                FinishDate = DateTime.UtcNow
            };

            using (var context = Plugin.Context)
            {
                context.Add(fightResult);
                context.SaveChanges();
            }
            

            Plugin.ResetFight();

            return fightResult;
        }

        public bool IsThisCharactersTurn(string characterName)
        {
            return GetActor().Name == characterName;
        }

        public bool IsAbleToAttack(string characterName)
        {
            return IsActive && IsThisCharactersTurn(characterName);
        }
        
        public void TakeAction(string actionMade)
        {
            var action = actionMade;
            var actor = GetActor();
            var roll = Utils.RollDice(20);
            while (actor.LastRolls.IndexOf(roll) != -1)
            {
                roll = Utils.RollDice(20);
            }
            actor.LastRolls.Add(roll);
            if (actor.LastRolls.Count > 5)
            {
                actor.LastRolls.RemoveAt(0);
            }
            Console.WriteLine(actor.LastRolls);
            var luck = 0; //Actor's average roll of the fight.

            WindowController.Action.Add(action);

            // Update tracked sum of all rolls and number of rolls the actor has made. Then calculate average value of actor's rolls in this fight.
            actor.RollTotal += roll;
            actor.RollsMade += 1;
            if (actor.RollsMade > 0)
            {
                luck = (int)Math.Round((double)actor.RollTotal / actor.RollsMade);
            }
            Type fighterType = actor.GetType();
            MethodInfo theMethod = fighterType.GetMethod("Action" + action);
            theMethod.Invoke(actor, new object[] { roll });

            WindowController.Info.Add("Raw Dice Roll: " + roll);
            WindowController.Info.Add(actor.Name + "'s Average Dice Roll: " + luck);
            if (roll == 20) WindowController.Info.Add("\n" + "[eicon]d20crit[/eicon]" + "\n");//Test to see if this works. Might add more graphics in the future.

            TurnUpKeep(); //End of turn upkeep (Stamina regen, check for being stunned/knocked out, etc.)
            OutputFighterStatus(); // Creates the fighter status blocks (HP/Mana/Stamina)
                                   //Battlefield.outputFighterStats();
            WindowController.UpdateOutput(this); //Tells the window controller to format and dump all the queued up messages to the results screen.
        }

        //public bool AddFighter(ArenaSettings settings)
        //{
        //    try
        //    {
        //        Fighters.Add(new Fighter(this, settings)); //TODO
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        return false;
        //    }
        //    return true;
        //}

        public void ClearFighters()
        {
            Fighters.Clear();
        }

        public Fighter GetActor()
        {
            return Fighters[currentFighter];
        }

        public Fighter GetTarget()
        {
            return Fighters[1 - currentFighter];
        }

        public void OutputFighterStatus()
        {
            for (int i = 0; i < Fighters.Count; i++)
            {
                WindowController.Status.Add(Fighters[i].GetStatus());
            }
        }

        public void OutputFighterStats()
        {
            for (int i = 0; i < Fighters.Count; i++)
            {
                WindowController.Status.Add(Fighters[i].GetStatBlock());
            }
        }

        public void NextFighter()
        {
            currentFighter = (currentFighter == Fighters.Count - 1) ? 0 : currentFighter + 1;

            if (Fighters[currentFighter].IsStunned)
            {
                Fighters[currentFighter].IsStunned = false;
                NextFighter();
            }
        }

        public void PickInitialActor()
        {
            currentFighter = Utils.GetRandomNumber(0, Fighters.Count - 1);
        }

        public string PickStage()
        {
            var stages = new List<string>() {
            "The Pit",
            "RF:Wrestling Ring",
            "Arena",
            "Subway",
            "Skyscraper Roof",
            "Forest",
            "Cafe",
            "Street road",
            "Alley",
            "Park",
            "RF:MMA Hexagonal Cage",
            "Hangar",
            "Swamp",
            "RF:Glass Box",
            "RF:Free Space",
            "Magic Shop",
            "Locker Room",
            "Library",
            "Pirate Ship",
            "Baazar",
            "Supermarket",
            "Night Club",
            "Docks",
            "Hospital",
            "Dark Temple",
            "Restaurant",
            "Graveyard",
            "Zoo",
            "Slaughterhouse",
            "Junkyard",
            "Theatre",
            "Circus",
            "Castle",
            "Museum",
            "Beach",
            "Bowling Club",
            "Concert Stage",
            "Wild West Town",
            "Movie Set"
            };

            return stages[Utils.GetRandomNumber(0, stages.Count - 1)];
        }

        public void TurnUpKeep()
        {
            for (var i = 0; i < Fighters.Count; i++)
            {
                Fighters[i].UpdateCondition();
            }

            Fighters[currentFighter].Regen();
            NextFighter();
        }
    }
}
