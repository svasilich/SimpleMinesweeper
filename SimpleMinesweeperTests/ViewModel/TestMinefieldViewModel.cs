using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeper.ViewModel;
using SimpleMinesweeperTests.Common;

namespace SimpleMinesweeperTests.ViewModel
{
    class TestMinefieldViewModel : MinefieldViewModel
    {
        public static int DefaultFieldHeightCell { get { return 16; } }
        public static int DefaultFieldWidthCell { get { return 30; } }
        public static int DefaultMineCount { get { return 99; } }       

        public static IMinefield DefaultMinefield(int fieldHeightCell, int fieldWidthCell)
        {
            IMinefield field = MinefieldTestHelper.CreateDefaultMinefield();
            field.Fill(fieldHeightCell, fieldWidthCell, DefaultMineCount);
            return field;
        }

        public TestMinefieldViewModel() :
            base(DefaultMinefield(DefaultFieldHeightCell, DefaultFieldWidthCell), MinefieldTestHelper.FakeMainWindow())
        {
            ReloadMinefield(DefaultFieldHeightCell, DefaultFieldWidthCell, DefaultMineCount);
        }

        public TestMinefieldViewModel(int height, int width) :
            base(DefaultMinefield(height, width), MinefieldTestHelper.FakeMainWindow())
        {
            ReloadMinefield(height, width, 1);
        }

        public TestMinefieldViewModel(IMinefield minefield) : base(minefield, MinefieldTestHelper.FakeMainWindow())
        {
            ReloadMinefield(minefield.Height, minefield.Length, minefield.MinesCount);
        }

        private void ReloadMinefield(int height, int width, int mineCount)
        {
            settings.SetCustomSize(height, width, mineCount);
            settings.SelectGameType(GameType.Custom);
            ReloadCommand.Execute(null);
        }

        public void SetSize(double widthPx, double heightPx)
        {
            ResizeField(widthPx, heightPx);
        }

        public IMinefield Field { get { return field; } }
    }
}
