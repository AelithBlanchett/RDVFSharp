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
        public List<Fighter> TeamRed { get; set; }
        public List<Fighter> TeamBlue { get; set; }
        public List<Fighter> TeamYellow { get; set; }
        public List<Fighter> TeamPurple { get; set; }
        public List<Fighter> TurnOrder { get; set; }
        public OutputController OutputController { get; set; }

        public string Stage { get; set; }
        public bool IsInProgress { get; set; }

        public bool DisplayGrabbed { get; set; }

        private int currentFighter = 0;
        private int initialActor = 0;

        public Battlefield(RendezvousFighting plugin)
        {
            Plugin = plugin;
            OutputController = new OutputController();
            Fighters = new List<Fighter>();
            Stage = StageSelect.SelectRandom();

            DisplayGrabbed = true;
            IsInProgress = false;
        }

        public void InitialSetup()
        {
            JoinTeams();
            PickInitialActor();
            SetInitialTargets();
            SetTurnOrder();
            ReassignInitialTargets();
            OutputController.Hit.Add("Game started!");
            OutputController.Hit.Add("FIGHTING STAGE: " + Stage + " - " + GetActor().Name + " goes first!");
            OutputFighterStatuses(); // Creates the fighter status blocks (HP/Mana/Stamina) 
            OutputFighterStats(); // Creates the fighter stat blocks (STR/DEX/END/INT/WIL)
            OutputController.Info.Add("[url=http://www.f-list.net/c/rendezvous%20fight/]Visit this page for game information[/url]");
            IsInProgress = true;
            OutputController.Broadcast(this);
        }

        public void SetInitialTargets()
        {
            foreach (var teamColor in Fighters.Select(x => x.TeamColor).Distinct())
            {
                var opponents = Fighters.Where(x => x.TeamColor != teamColor).OrderBy(x => new Random().Next()).ToList();
                var numberOfOpponentsAvailable = opponents.Count();
                var random = new Random();
                int index = random.Next(opponents.Count());
                if(numberOfOpponentsAvailable == 0)
                {
                    throw new Exception("There are no opponents available.");
                }
                foreach (var teamMember in Fighters.Where(x => x.TeamColor == teamColor).ToList())
                {
                    teamMember.CurrentTarget = opponents[index];
                }
            }
        }

        #region Target Management
        public void ReassignInitialTargets() // Doing the multiple repeats in here for the occasion where multi-fights occur
        {
            foreach (var fighter in Fighters)
            {
                foreach (var teamMember in Fighters.Where(x => x.TeamColor == fighter.TeamColor && x.Name != fighter.Name).ToList())

                {
                    var opponents = Fighters.Where(x => x.TeamColor != fighter.TeamColor && x != teamMember.CurrentTarget).OrderBy(x => new Random().Next()).ToList();

                    if ((fighter.CurrentTarget == teamMember.CurrentTarget) && (opponents.Count() > 0))

                    {
                        fighter.CurrentTarget = opponents.First();
                        if ((fighter.CurrentTarget == teamMember.CurrentTarget) && (opponents.Count() > 0))

                        {
                            fighter.CurrentTarget = opponents.First();
                            if ((fighter.CurrentTarget == teamMember.CurrentTarget) && (opponents.Count() > 0))

                            {
                                fighter.CurrentTarget = opponents.First();
                                if ((fighter.CurrentTarget == teamMember.CurrentTarget) && (opponents.Count() > 0))

                                {
                                    fighter.CurrentTarget = opponents.First();
                                    if ((fighter.CurrentTarget == teamMember.CurrentTarget) && (opponents.Count() > 0))

                                    {
                                        fighter.CurrentTarget = opponents.First();
                                        if ((fighter.CurrentTarget == teamMember.CurrentTarget) && (opponents.Count() > 0))

                                        {
                                            fighter.CurrentTarget = opponents.First();
                                            if ((fighter.CurrentTarget == teamMember.CurrentTarget) && (opponents.Count() > 0))

                                            {
                                                fighter.CurrentTarget = opponents.First();
                                                if ((fighter.CurrentTarget == teamMember.CurrentTarget) && (opponents.Count() > 0))

                                                {
                                                    fighter.CurrentTarget = opponents.First();
                                                    if ((fighter.CurrentTarget == teamMember.CurrentTarget) && (opponents.Count() > 0))

                                                    {
                                                        fighter.CurrentTarget = opponents.First();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    foreach (var opponent in opponents.Where(x => fighter.CurrentTarget == x))
                    {
                        opponent.CurrentTarget = fighter;
                    }

                }

                foreach (var Opponent in Fighters.Where(x => x.TeamColor != fighter.TeamColor).ToList())
                {
                    var opponents = Fighters.Where(x => x.TeamColor != fighter.TeamColor && x != Opponent.CurrentTarget).OrderBy(x => new Random().Next()).ToList();
                    if ((fighter.CurrentTarget == Opponent.CurrentTarget) && (opponents.Count() > 0))

                    {
                        fighter.CurrentTarget = opponents.First();
                        if ((fighter.CurrentTarget == Opponent.CurrentTarget) && (opponents.Count() > 0))

                        {
                            fighter.CurrentTarget = opponents.First();
                            if ((fighter.CurrentTarget == Opponent.CurrentTarget) && (opponents.Count() > 0))

                            {
                                fighter.CurrentTarget = opponents.First();
                                if ((fighter.CurrentTarget == Opponent.CurrentTarget) && (opponents.Count() > 0))

                                {
                                    fighter.CurrentTarget = opponents.First();
                                    if ((fighter.CurrentTarget == Opponent.CurrentTarget) && (opponents.Count() > 0))

                                    {
                                        fighter.CurrentTarget = opponents.First();
                                        if ((fighter.CurrentTarget == Opponent.CurrentTarget) && (opponents.Count() > 0))

                                        {
                                            fighter.CurrentTarget = opponents.First();
                                            if ((fighter.CurrentTarget == Opponent.CurrentTarget) && (opponents.Count() > 0))

                                            {
                                                fighter.CurrentTarget = opponents.First();
                                                if ((fighter.CurrentTarget == Opponent.CurrentTarget) && (opponents.Count() > 0))

                                                {
                                                    fighter.CurrentTarget = opponents.First();
                                                    if ((fighter.CurrentTarget == Opponent.CurrentTarget) && (opponents.Count() > 0))

                                                    {
                                                        fighter.CurrentTarget = opponents.First();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (var opponent in opponents.Where(x => fighter.CurrentTarget == x))
                    {
                        opponent.CurrentTarget = fighter;
                    }
                }
            }
        }

        public void AssignNewTarget(Fighter fighter)
        {
            foreach (var Opponent in Fighters.Where(x => x.TeamColor != fighter.TeamColor).ToList())
            { 
                var opponents = Fighters.Where((x => x.TeamColor != fighter.TeamColor && x.IsDead == false)).OrderBy(x => new Random().Next()).ToList();
                fighter.CurrentTarget = opponents.First();
                if (fighter.CurrentTarget.IsDead == true)
                {
                    fighter.CurrentTarget = opponents.First();
                    if (fighter.CurrentTarget.IsDead == true)
                    {
                        fighter.CurrentTarget = opponents.First();
                        if (fighter.CurrentTarget.IsDead == true)
                        {
                            fighter.CurrentTarget = opponents.First();
                            if (fighter.CurrentTarget.IsDead == true)
                            {
                                fighter.CurrentTarget = opponents.First();
                                if (fighter.CurrentTarget.IsDead == true)
                                {
                                    fighter.CurrentTarget = opponents.First();
                                    if (fighter.CurrentTarget.IsDead == true)
                                    {
                                        fighter.CurrentTarget = opponents.First();
                                        if (fighter.CurrentTarget.IsDead == true)
                                        {
                                            fighter.CurrentTarget = opponents.First();
                                            if (fighter.CurrentTarget.IsDead == true)
                                            {
                                                fighter.CurrentTarget = opponents.First();
                                                if (fighter.CurrentTarget.IsDead == true)
                                                {
                                                    fighter.CurrentTarget = opponents.First();
                                                    if (fighter.CurrentTarget.IsDead == true)
                                                    {
                                                        fighter.CurrentTarget = opponents.First();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                } 
            }
        }

        #endregion

        public void CheckTargetCoherenceAndReassign()
        {
            foreach (var fighter in Fighters)
            {
                if (fighter.CurrentTarget.IsDead)
                {
                    foreach (var enemies in Fighters.Where(x => x.CurrentTarget.Name == fighter.Name))
                    {
                        enemies.RemoveGrappler(fighter);
                    }
                    AssignNewTarget(fighter);
                }
            }
        }

        public bool AddFighter(BaseFighter fighterName, string teamColor)
        {
            Fighters.Add(new Fighter(fighterName, this, teamColor));

            return true;
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

            var fightAction = FightActionFactory.Create(action, Fighters.Count > 2);
            fightAction.Execute(roll, this, actor, this.GetFighterTarget(actor.Name));

            OutputController.Info.Add("Raw Dice Roll: " + roll);
            OutputController.Info.Add(actor.Name + "'s Average Dice Roll: " + luck);
            if (roll == 20) OutputController.Info.Add("\n" + "[eicon]d20crit[/eicon]" + "\n");//Test to see if this works. Might add more graphics in the future.

            TurnUpKeep(); //End of turn upkeep (Stamina regen, check for being stunned/knocked out, etc.)
            OutputFighterStatuses(); // Creates the fighter status blocks (HP/Mana/Stamina)
                                     //Battlefield.outputFighterStats();

            if ((GetActor().IsGrabbable == GetTarget().IsGrabbable) && (GetActor().IsGrabbable > 0) && (GetActor().IsGrabbable < 20))
            {
                OutputController.Hint.Add(GetActor().Name + " and " + GetTarget().Name + " are in grappling range.");
            }

            OutputController.Broadcast(this); //Tells the window controller to format and dump all the queued up messages to the results screen.
        }

        #region Turn-based logic
        public void TurnUpKeep()
        {
            for (var i = 0; i < Fighters.Count; i++)
            {
                TurnOrder[i].UpdateCondition();
            }

            TurnOrder[currentFighter].Regen();

            CheckIfFightIsOver();

            CheckTargetCoherenceAndReassign();
            NextFighter();
            for (var i = 0; i < Fighters.Count; i++)
            {
                TurnOrder[i].FinalStand();
            }
        }

        private void CheckIfFightIsOver()
        {
            if(RemainingTeams == 1)
            {
                OutputController.Hit.Add("The fight is over! CLAIM YOUR SPOILS and VICTORY and FINISH YOUR OPPONENT!");
                OutputController.Special.Add("FATALITY SUGGESTION: " + FatalitySelect.SelectRandom());
                OutputController.Special.Add("It is just a suggestion, you may not follow it if you don't want to.");
                EndFight(GetActor(), GetTarget());
            }
        }

        public int RemainingTeams
        {
            get
            {
                return Fighters.Where(x => x.IsDead == false).Select(x => x.TeamColor).ToList().Distinct().Count();
            }
        }
        public void JoinTeams()
        {
            TeamRed = new List<Fighter>();
            TeamBlue = new List<Fighter>();
            TeamYellow = new List<Fighter>();
            TeamPurple = new List<Fighter>();

            foreach (var fighter in Fighters.Where(x => x.TeamColor == "red"))
            {
                TeamRed.Add(fighter);
            }

            foreach (var fighter in Fighters.Where(x => x.TeamColor == "blue"))
            {
                TeamBlue.Add(fighter);
            }

            foreach (var fighter in Fighters.Where(x => x.TeamColor == "yellow"))
            {
                TeamYellow.Add(fighter);
            }

            foreach (var fighter in Fighters.Where(x => x.TeamColor == "purple"))
            {
                TeamPurple.Add(fighter);
            }
        }

        public void SetTurnOrder()
        {
            TurnOrder = new List<Fighter>();



            if (TeamRed.Count > 0) TurnOrder.Add(TeamRed[0]);
            if (TeamBlue.Count > 0) TurnOrder.Add(TeamBlue[0]);
            if (TeamYellow.Count > 0) TurnOrder.Add(TeamYellow[0]);
            if (TeamPurple.Count > 0) TurnOrder.Add(TeamPurple[0]);
            if (TeamRed.Count > 1) TurnOrder.Add(TeamRed[1]);
            if (TeamBlue.Count > 1) TurnOrder.Add(TeamBlue[1]);
            if (TeamYellow.Count > 1) TurnOrder.Add(TeamYellow[1]);
            if (TeamPurple.Count > 1) TurnOrder.Add(TeamPurple[1]);
            if (TeamRed.Count > 2) TurnOrder.Add(TeamRed[2]);
            if (TeamBlue.Count > 2) TurnOrder.Add(TeamBlue[2]);
            if (TeamYellow.Count > 2) TurnOrder.Add(TeamYellow[2]);
            if (TeamPurple.Count > 2) TurnOrder.Add(TeamPurple[2]);
            if (TeamRed.Count > 3) TurnOrder.Add(TeamRed[3]);
            if (TeamBlue.Count > 3) TurnOrder.Add(TeamBlue[3]); 
            if (TeamYellow.Count > 3) TurnOrder.Add(TeamYellow[3]); 
            if (TeamPurple.Count > 3) TurnOrder.Add(TeamPurple[3]);
            if (TeamRed.Count > 4) TurnOrder.Add(TeamRed[4]);
            if (TeamBlue.Count > 4) TurnOrder.Add(TeamBlue[4]);
            if (TeamYellow.Count > 4) TurnOrder.Add(TeamYellow[4]);
            if (TeamPurple.Count > 4) TurnOrder.Add(TeamPurple[4]);
            if (TeamRed.Count > 5) TurnOrder.Add(TeamRed[5]);
            if (TeamBlue.Count > 5) TurnOrder.Add(TeamBlue[5]);
            if (TeamYellow.Count > 5) TurnOrder.Add(TeamYellow[5]);
            if (TeamPurple.Count > 5) TurnOrder.Add(TeamPurple[5]);
        }

        public void NextFighter()
        {
            currentFighter = (currentFighter == TurnOrder.Count - 1) ? 0 : currentFighter + 1;

            if (TurnOrder[currentFighter].HPBurn > 0 && TurnOrder[currentFighter].IsStunned < 2 && TurnOrder[currentFighter].IsDazed == false)
            {
                TurnOrder[currentFighter].HPBurn--;
            }

            if (TurnOrder[currentFighter].ManaDamage > 0 && TurnOrder[currentFighter].IsStunned < 2 && TurnOrder[currentFighter].IsDazed == false)
            {
                TurnOrder[currentFighter].ManaDamage--;
            }

            if (TurnOrder[currentFighter].StaminaDamage > 0 && TurnOrder[currentFighter].IsStunned < 2 && TurnOrder[currentFighter].IsDazed == false)
            {
                TurnOrder[currentFighter].StaminaDamage--;
            }

            if (TurnOrder[currentFighter].IsExposed > 0)
            {
                TurnOrder[currentFighter].IsExposed-=2;
            }
            
            if (TurnOrder[currentFighter].IsStunned > 1)
            {
                TurnOrder[currentFighter].IsStunned--;
                NextFighter();
            }

            if (TurnOrder[currentFighter].IsDazed == true)
            {
                TurnOrder[currentFighter].IsDazed = false;
                NextFighter();
            }

            
        }

        public void PickInitialActor()
        {
            currentFighter = Utils.GetRandomNumber(0, Fighters.Count);
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

        public void ClearFighters()
        {
            Fighters.Clear();
        }

        public Fighter GetActor()
        {
            return TurnOrder[currentFighter];
        }

        public Fighter GetTarget()
        {
            return GetActor().CurrentTarget;
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
                FinishDate = DateTime.UtcNow,
                AdditionalWinnersId = string.Join(',', Fighters.Where(x => x.Name != victor.Name && x.TeamColor == victor.TeamColor).Select(x => x.Name).ToList()),
                AdditionalLosersId = string.Join(',', Fighters.Where(x => x.Name != loser.Name && x.TeamColor != victor.TeamColor).Select(x => x.Name).ToList())
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
