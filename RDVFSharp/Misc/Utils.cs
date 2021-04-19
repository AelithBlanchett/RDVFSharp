using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RDVFSharp
{
    public class Utils
    {
        public static int RollDice(int sides)
        {
            return GetRandomNumber(1, sides + 1);
        }

        public static int RollDice(List<int> sides)
        {
            var total = 0;
            for (var i = 0; i < sides.Count(); i++)
            {
                total += RollDice(sides[i] + 1);
            }
            return total;
        }

        private static Random rnum = new Random();

        public static int GetRandomNumber(int min, int max)
        {
            return rnum.Next(min, max);
        }

        public static int Clamp(int n, int min, int max)
        {
            return Math.Max(min, Math.Min(n, max));
        }

        public static int CoinFlip()
        {
            return rnum.NextDouble() < 0.5d ? 0 : 1;
        }
    }
}
