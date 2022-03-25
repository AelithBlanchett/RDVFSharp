using RDVFSharp.Entities;
using RDVFSharp.FightingLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RDVFSharp
{
    public class Battlefield
    {
        public RendezvousFighting Plugin { get; }
        public List<Fighter> Fighters { get; set; }
        public OutputController OutputController { get; set; }

        public string Stage { get; set; }
        public bool IsInProgress { get; set; }

        public bool InGrabRange { get; set; }
        public bool DisplayGrabbed { get; set; }

        private int currentFighter = 0;


        public Battlefield(RendezvousFighting plugin)
        {
            Plugin = plugin;
            OutputController = new OutputController();
            Fighters = new List<Fighter>();
            Stage = StageSelect.SelectRandom();

            InGrabRange = false;
            DisplayGrabbed = true;
            IsInProgress = false;
        }

        public void InitialSetup(Fighter firstFighter, Fighter secondFighter)
        {
            Fighters.Clear();
            Fighters.Add(firstFighter);
            Fighters.Add(secondFighter);

            PickInitialActor();
            OutputController.Hit.Add("Game started!");
            OutputController.Hit.Add("FIGHTING STAGE: " + Stage + " - " + GetActor().Name + " goes first!");
            OutputFighterStatuses(); // Creates the fighter status blocks (HP/Mana/Stamina)
            OutputFighterStats(); // Creates the fighter stat blocks (STR/DEX/END/INT/WIL)
            OutputController.Info.Add("[url=http://www.f-list.net/c/rendezvous%20fight/]Visit this page for game information[/url]");
            IsInProgress = true;
            OutputController.Broadcast(this);
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

            OutputController.Action.Add(action);

            // Update tracked sum of all rolls and number of rolls the actor has made. Then calculate average value of actor's rolls in this fight.
            actor.RollTotal += roll;
            actor.RollsMade += 1;
            if (actor.RollsMade > 0)
            {
                luck = (int)Math.Round((double)actor.RollTotal / actor.RollsMade);
            }

            var fightAction = FightActionFactory.Create(action);
            fightAction.Execute(roll, this, actor, this.GetFighterTarget(actor.Name));

            OutputController.Info.Add("Raw Dice Roll: " + roll);
            OutputController.Info.Add(actor.Name + "'s Average Dice Roll: " + luck);
            if (roll == 20) OutputController.Info.Add("\n" + "[eicon]d20crit[/eicon]" + "\n");//Test to see if this works. Might add more graphics in the future.

            TurnUpKeep(); //End of turn upkeep (Stamina regen, check for being stunned/knocked out, etc.)
            OutputFighterStatuses(); // Creates the fighter status blocks (HP/Mana/Stamina)
                                     //Battlefield.outputFighterStats();
            OutputController.Broadcast(this); //Tells the window controller to format and dump all the queued up messages to the results screen.
        }

        #region Turn-based logic
        public void TurnUpKeep()
        {
            for (var i = 0; i < Fighters.Count; i++)
            {
                Fighters[i].UpdateCondition();
            }

            Fighters[currentFighter].Regen();
            NextFighter();
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

        public bool IsThisCharactersTurn(string characterName)
        {
            return GetActor().Name == characterName;
        }

        public bool IsAbleToAttack(string characterName)
        {
            return IsInProgress && IsThisCharactersTurn(characterName);
        }
        #endregion

        #region Fighter management

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

        public Fighter FirstFighter
        {
            get
            {
                return Fighters.FirstOrDefault();
            }
        }

        public Fighter SecondFighter
        {
            get
            {
                return Fighters.LastOrDefault();
            }
        }

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

        #endregion     

        #region Output shortcuts
        public void OutputFighterStatuses()
        {
            for (int i = 0; i < Fighters.Count; i++)
            {
                OutputController.Status.Add(Fighters[i].GetStatus());
            }
        }

        public void OutputFighterStats()
        {
            for (int i = 0; i < Fighters.Count; i++)
            {
                OutputController.Status.Add(Fighters[i].GetStatBlock());
            }
        }

        #endregion

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
    }
}
