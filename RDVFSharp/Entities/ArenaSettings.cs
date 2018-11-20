using System;
using System.Collections.Generic;
using System.Text;

namespace RDVFSharp.Entities
{
    class ArenaSettings
    {
        public int StatPoints { get; set; }
        public int GameSpeed { get; set; }
        public int DisorientedAt { get; set; }
        public int UnconsciousAt { get; set; }
        public int DeadAt { get; set; }

        public ArenaSettings(int statPoints = Constants.DefaultStatPoints, int gameSpeed = Constants.DefaultGameSpeed, int disorientedAt = Constants.DefaultDisorientedAt, int unconsciousAt = Constants.DefaultUnconsciousAt, int deadAt = Constants.DefaultDeadAt)
        {
            StatPoints = statPoints;
            GameSpeed = gameSpeed;
            DisorientedAt = disorientedAt;
            UnconsciousAt = unconsciousAt;
            DeadAt = deadAt;
        }
    }
}
