using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleMinesweeper.Core;
using SimpleMinesweeper.ViewModel;

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
            return new Minefield(DefaultCellFactory, DefaultMinePositionsGenerator);
        }

        /// <summary>
        /// Создаёт поддельное окно для MinefieldViewModel.
        /// </summary>
        /// <returns></returns>
        public static IDynamicGameFieldSize FakeMainWindow()
        {
            return NSubstitute.Substitute.For<IDynamicGameFieldSize>();
        }

        public static ICellFactory DefaultCellFactory => new CellFactory();

        public static IMinePositionsGenerator DefaultMinePositionsGenerator => new RandomMinePositionGenerator();
    }
}
