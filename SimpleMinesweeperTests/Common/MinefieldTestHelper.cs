using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleMinesweeper.Core;
using SimpleMinesweeper.ViewModel;
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeper.Core.GameRecords;

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
            IMinefield field = new Minefield(DefaultCellFactory, DefaultMinePositionsGenerator);
            SettingsItem settings = new SettingsItem();
            settings.Height = 10;
            settings.Width = 10;
            settings.MineCount = 10;
            field.SetGameSettings(settings);
            field.Fill();
            return field;
        }

        /// <summary>
        /// Создаёт поддельное окно для MinefieldViewModel.
        /// </summary>
        /// <returns></returns>
        public static IDynamicGameFieldSize FakeMainWindow()
        {
            return NSubstitute.Substitute.For<IDynamicGameFieldSize>();
        }

        public static IRecords GetDefaultRecords()
        {
            return new Records();
        }

        public static string RecordsPath => AppContext.BaseDirectory + "records_test.dat";

        public static ICellFactory DefaultCellFactory => new CellFactory();

        public static IMinePositionsGenerator DefaultMinePositionsGenerator => new RandomMinePositionGenerator();
    }
}
