using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleMinesweeper.Core;

namespace SimpleMinesweeperTests.Common
{
    class MinefieldTestHelper
    {
        /// <summary>
        /// Создаёт минное со стандартной фабрикой клеток и генератором мин со случайным порядком.
        /// </summary>
        /// <returns></returns>
        public static IMinefield CreateDefaultMinefield()
        {
            return new Minefield(new CellFactory(), new RandomMinePositionGenerator());
        }
    }
}
