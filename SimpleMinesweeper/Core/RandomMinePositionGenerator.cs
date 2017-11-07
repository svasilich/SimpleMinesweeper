using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.Core
{
    public class RandomMinePositionGenerator : IMinePositionsGenerator
    {
        private Random generator;

        public RandomMinePositionGenerator()
        {
            generator = new Random((int)DateTime.Now.Ticks);
        }

        public int Next(int max)
        {
            return generator.Next(max);
        }
    }
}
