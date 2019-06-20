using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;

using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeperTests.Common;


namespace SimpleMinesweeperTests.Core
{
    [TestFixture]
    class GameTests
    {
        #region Test types
        class GameWithOpenConstructor : Game
        {
            public GameWithOpenConstructor(ISettingsManager settings, IMinefield gameField) : base(settings, gameField)
            {

            }

            #region Event handlers
            protected override void Settings_OnCurrentGameChanged(object sender, EventArgs e)
            {
                
            }
            #endregion
        }

        class TestGame : Game
        {
            #region Properties
            public bool EventWasHandled { get; private set; }
            #endregion

            #region Constructors
            public TestGame(ISettingsManager settings, IMinefield gameField) : base(settings, gameField)
            {
                EventWasHandled = false;
            }
            #endregion

            #region Event handlers
            protected override void Settings_OnCurrentGameChanged(object sender, EventArgs e)
            {
                //base.Settings_OnCurrentGameChanged(sender, e);
                EventWasHandled = true;
            }
            #endregion
        }

        class MinefieldWithCheckFillCalled : Minefield
        {
            #region Properties
            public bool FillMethodCalled { get; private set; }
            #endregion

            #region Static methods

            public static MinefieldWithCheckFillCalled CreateDefault()
            {
                return new MinefieldWithCheckFillCalled(MinefieldTestHelper.DefaultCellFactory,
                    MinefieldTestHelper.DefaultMinePositionsGenerator);
            }

            #endregion

            #region Constructors

            public MinefieldWithCheckFillCalled(ICellFactory cellFactory, IMinePositionsGenerator minePositionsGenerator) : base(cellFactory, minePositionsGenerator)
            {
                FillMethodCalled = false;
            }

            #endregion

            public override void Fill()
            {
                FillMethodCalled = true;
            }
        }
        #endregion

        [Test]
        public void OnChangeGameType_EventHandled()
        {
            IMinefield minefield = MinefieldTestHelper.CreateDefaultMinefield();
            SettingsManager settingsManager = new SettingsManager();
            settingsManager.SelectGameType(GameType.Advanced);

            TestGame game = new TestGame(settingsManager, minefield);

            game.Settings.SelectGameType(GameType.Newbie);

            Assert.AreEqual(true, game.EventWasHandled);
        }        

        [Test]
        public void OnChangeGameType_MinefieldFillCalled()
        {
            var field = MinefieldWithCheckFillCalled.CreateDefault();
            SettingsManager settingsManager = new SettingsManager();
            settingsManager.SelectGameType(GameType.Advanced);
            Game game = new GameWithOpenConstructor(settingsManager, field);

            game.Settings.SelectGameType(GameType.Newbie);

            Assert.AreEqual(true, field.FillMethodCalled);
        }
    }
}
