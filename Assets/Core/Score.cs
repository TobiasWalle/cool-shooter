using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Core
{
    public enum ScoreType
    {
        Kills,
        Deaths
    }

    public struct Score
    {
        public int Kills;
        public int Deaths;

        private Score(int Kills, int Deaths)
        {
            this.Kills = Kills;
            this.Deaths = Deaths;
        }

        internal Score Increment(ScoreType type)
        {
            switch (type)
            {
                case ScoreType.Kills:
                    return new Score(Kills + 1, Deaths);
                case ScoreType.Deaths:
                    return new Score(Kills, Deaths + 1);
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }
    }
}