using RDVFSharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp
{
    class Battlefield
    {
        public List<Fighter> Fighters { get; set; }
        public ArenaSettings GlobalFighterSettings { get; set; }
        public bool DisplayGrabbed { get; set; }
        public WindowController WindowController { get; set; }

        private int currentFighter = 0;
        public bool InGrabRange { get; set; }
        public RendezvousFighting Plugin { get; }

        public Battlefield(RendezvousFighting plugin)
        {
            Plugin = plugin;
        }

        public bool AddFighter(ArenaSettings settings)
        {
            try
            {
                Fighters.Add(new Fighter(this, settings)); //TODO
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            return true;
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

        public void OutputFighterStatus(dynamic windowContorller)
        {
            for (int i = 0; i < Fighters.Count; i++)
            {
                windowContorller.addStatus(Fighters[i].GetStatus());
            }
        }
    }
}
