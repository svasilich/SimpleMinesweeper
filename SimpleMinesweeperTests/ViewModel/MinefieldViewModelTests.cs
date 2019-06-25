using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeper.ViewModel;
using SimpleMinesweeperTests.Common;

namespace SimpleMinesweeperTests.ViewModel
{
    class MinefieldViewModelTest : MinefieldViewModel
    {
        public static int DefaultFieldHeightCell { get { return 16; } }
        public static int DefaultFieldWidthCell { get { return 30; } }
        public static int DefaultMineCount { get { return 99; } }


        public static IGame GameWithDefaultMIneField(int fieldHeightCell, int fieldWidthCell)
        {
            ISettingsManager settingsManager = Substitute.For<ISettingsManager>();
            settingsManager.CurrentSettings.Returns(
                new SettingsItem()
                {
                    Height = fieldHeightCell,
                    Width = fieldWidthCell,
                    MineCount = DefaultMineCount
                });
            return new GameWithOpenConstructor(settingsManager, DefaultMinefield(fieldHeightCell, fieldWidthCell));
        }

        class GameWithOpenConstructor : Game
        {
            public GameWithOpenConstructor(ISettingsManager settings, IMinefield gameField) : base(settings, gameField)
            {
            }
        }

        public static IMinefield DefaultMinefield(int fieldHeightCell, int fieldWidthCell)
        {
            IMinefield field = MinefieldTestHelper.CreateDefaultMinefield();
            SettingsItem settings = new SettingsItem()
            {
                Height = fieldHeightCell,
                Width = fieldWidthCell,
                MineCount = DefaultMineCount
            };
            field.SetGameSettings(settings);
            
            field.Fill();
            return field;
        }

        public MinefieldViewModelTest() :
            base(GameWithDefaultMIneField(DefaultFieldHeightCell, DefaultFieldWidthCell), MinefieldTestHelper.FakeMainWindow())
        {

        }

        public MinefieldViewModelTest(int height, int width) :
            base(GameWithDefaultMIneField(height, width), MinefieldTestHelper.FakeMainWindow())
        {

        }

        public MinefieldViewModelTest(IGame game) : base(game, MinefieldTestHelper.FakeMainWindow())
        {
        }

        public void SetSize(double widthPx, double heightPx)
        {
            ResizeField(widthPx, heightPx);
        }

        public IMinefield Field { get { return game.GameField; } }
    }
}
