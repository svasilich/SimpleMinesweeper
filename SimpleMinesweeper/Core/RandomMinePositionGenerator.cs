using System;

namespace SimpleMinesweeper.Core
{
    public class RandomMinePositionGenerator : IMinePositionsGenerator
    {
        #region Fields

        private Random generator;

        #endregion

        #region Constructor

        public RandomMinePositionGenerator()
        {
            generator = new Random((int)DateTime.Now.Ticks);
        }

        #endregion

        #region Public methods

        public int Next(int max)
        {
            return generator.Next(max);
        }

        #endregion


    }
}
