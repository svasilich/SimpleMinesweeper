using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NSubstitute;
using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeperTests.Common;


namespace SimpleMinesweeperTest.Core
{
    [TestFixture]
    class MinefieldTests
    {
        [TestCase(0, 1, 1, "высота")]
        [TestCase(1, 0, 1, "ширина")]
        [TestCase(1, 1, 0, "количество мин должно быть больше")]
        [TestCase(5, 5, 100, "слишком много")]
        public void SendFillIncorrectParameters_ThrowException(int height, int width, int mineCount, string expected)
        {
            IMinefield minefield = MinefieldTestHelper.CreateDefaultMinefield();
            SettingsItem settings = CreateCustomSettings(height, width, mineCount);
            minefield.SetGameSettings(settings);

            var ex = Assert.Catch<ArgumentException>(() => minefield.Fill());
            StringAssert.Contains(expected.ToUpper(), ex.Message.ToUpper());
        }

        [Test]
        public void FillFromSettingsObject()
        {
            SettingsItem settings = CreateCustomSettings(10, 11, 9);
            IMinePositionsGenerator generator = new RandomMinePositionGenerator();
            IMinefield field = new Minefield(new CellFactory(), generator);

            field.SetGameSettings(settings);
            field.Fill();

            Assert.AreEqual(10, field.Height);
            Assert.AreEqual(11, field.Width);
            Assert.AreEqual(9, field.MinesCount);
        }
        
        [Test]
        public void SetMineToCornerAndCheckNearby()
        {
            IEnumerable<int> coords = new List<int>()
            {
                1, 1,
                1,0,
                0, 1
            };

            IMinePositionsGenerator generator = new CollectionMinePositionGenerator(coords);
            IMinefield field = new Minefield(new CellFactory(), generator);
            SettingsItem settings = CreateCustomSettings(10, 10, 3);
            field.SetGameSettings(settings);
            field.Fill();

            int minesCount = field.GetCellMineNearbyCount(field.Cells[0][0]);

            Assert.AreEqual(3, minesCount);
        }

        [Test]
        public void SetMineToCenterAndItselfHasNoMinedNearby()
        {
            IMinefield field = SetMineToCenter();
            IEnumerable<ICell> toCheck = GetNearbyFromCenter(field);

            ICell center = field.GetCellByCoords(5, 5);

            Assert.AreEqual(0, center.MinesNearby);
        }

        /// <summary>
        /// При первом нажатии состояние должно стать InGame.
        /// </summary>
        [Test]        
        public void FirstClick_StateChangeToInGame()
        {
            IMinefield minefield = SetMineToCenter();

            minefield.Cells[4][4].Open();

            Assert.AreEqual(FieldState.InGame, minefield.State);
        }

        /// <summary>
        /// При установке флага инра должна начаться.
        /// </summary>
        [Test]
        public void FirstSetFlag_StateChangeToInGame()
        {
            IMinefield minefield = SetMineToCenter();

            minefield.Cells[4][4].SetFlag();

            Assert.AreEqual(FieldState.InGame, minefield.State);
        }

        [Test]
        public void FirstClickOnMinedCell_NoExplosion()
        {
            IMinefield minefield = SetMineToCenter();
        
            ICell cell = minefield.Cells[5][5];
            cell.Open();
        
            Assert.AreNotEqual(CellState.Explosion, cell.State);
            Assert.AreNotEqual(FieldState.GameOver, minefield.State);
        }
        
        [Test]
        public void MineExplosion_StateToGameOver()
        {
            IMinefield field = SetMineToCenter();
            field.GetCellByCoords(0, 1).Mined = true;
            ICell cell = field.GetCellByCoords(0, 0);
            cell.Open();

            // Boom!
            cell = field.GetCellByCoords(5, 5);
            cell.Open();

            Assert.AreEqual(FieldState.GameOver, field.State);            
        }

        [Test]
        public void MineExplosion_AllCellsAreShowedUp()
        {
            List<int> coords = new List<int> { 0, 0, 0, 1, 5, 5 };
            IMinePositionsGenerator generator = new CollectionMinePositionGenerator(coords);
            IMinefield field = new Minefield(new CellFactory(), generator);
            SettingsItem settings = CreateCustomSettings(10, 10, 3);
            field.SetGameSettings(settings);
            field.Fill();
            ICell cell = field.GetCellByCoords(1, 0);
            cell.Open();

            ICell cellFlagged = field.GetCellByCoords(0, 1);
            cellFlagged.SetFlag();

            ICell cellWrongFlag = field.GetCellByCoords(2, 2);
            cellWrongFlag.SetFlag();

            // Boom!
            cell = field.GetCellByCoords(5, 5);
            cell.Open();

            // Assertation

            ICell cellOpened = field.GetCellByCoords(9, 9);
            Assert.AreEqual(CellState.Opened, cellOpened.State);

            ICell cellNoFinded = field.GetCellByCoords(0, 0);
            Assert.AreEqual(CellState.NoFindedMine, cellNoFinded.State);

            Assert.AreEqual(CellState.Flagged, cellFlagged.State);

            Assert.AreEqual(CellState.WrongFlag, cellWrongFlag.State);
        }
        
        [Test]
        public void OpenAllNoMinedCells_StateToWin()
        {
            IMinefield minefield = SetMineToCenter();
            WinGame(minefield);

            Assert.AreEqual(FieldState.Win, minefield.State);
        }

        /// <summary>
        /// После заполнения поля его состояние должно стать NotStarted.
        /// </summary>
        [Test]
        public void FillMinefield_StateToNotStarted()
        {
            IMinefield minefield = SetMineToCenter();
            SettingsItem settings = CreateCustomSettings(10, 10, 1);
            minefield.SetGameSettings(settings);

            minefield.Cells[0][0].Open();            
            minefield.Fill();

            Assert.AreEqual(FieldState.NotStarted, minefield.State);
        }

        [Test]
        public void Explosion_StateToLosse()
        {
            IMinefield minefield = SetMineToCenter()    ;

            minefield.Cells[4][4].Open();
            minefield.Cells[5][5].Open();

            Assert.AreEqual(FieldState.GameOver, minefield.State);
        }

        [Test]
        public void EndGame_CanNotOpenCell()
        {
            IMinefield minefield = SetMineToCenter();
            WinGame(minefield);

            ICell minedCell = minefield.GetCellByCoords(5, 5);
            minedCell.Open();

            Assert.AreEqual(CellState.NotOpened, minedCell.State);
        }

        /// <summary>
        /// Если вокруг открываемой клетки нет ни одной мины, её соседи должны стать открытыми.
        /// </summary>
        [Test]
        public void OpenCellWithNoAdjacentMine_AllAdjacentCellStateToOpen()
        {
            IMinefield minefield = SetMineToCenter();
            minefield.Cells[0][0].Open();

            Assert.AreEqual(CellState.Opened, minefield.Cells[1][0].State);
            Assert.AreEqual(CellState.Opened, minefield.Cells[0][1].State);
            Assert.AreEqual(CellState.Opened, minefield.Cells[1][1].State);
        }

        /// <summary>
        /// Минное поле должно знать, сколько всего на нём мин.
        /// </summary>
        [Test]
        public void CreateFieldWithMine_CheckMinesCount()
        {
            IMinefield minefield = SetMineToCenter(); // Количество мин равно единице.

            Assert.AreEqual(1, minefield.MinesCount);
        }

        /// <summary>
        /// Когда установлен флаг, количество флажков должно вырасти на единицу.
        /// </summary>
        [Test]
        public void SetFlag_FlagsCountUp()
        {
            IMinefield minefield = SetMineToCenter();

            minefield.Cells[0][0].SetFlag();
            Assert.AreEqual(1, minefield.FlagsCount);

            minefield.Cells[1][1].SetFlag();
            Assert.AreEqual(2, minefield.FlagsCount);
        }

        /// <summary>
        /// Когда флаг сбрасывается, количество флажков уменьшается на единицу.
        /// </summary>
        [Test]
        public void ResetFlag_FlagsCountDown()
        {
            IMinefield minefield = SetMineToCenter();

            minefield.Cells[0][0].SetFlag();
            minefield.Cells[1][1].SetFlag();

            minefield.Cells[1][1].SetFlag();
            Assert.AreEqual(1, minefield.FlagsCount);
        }

        /// <summary>
        /// Минное опле должно уведомить, что количество мин поменялось.
        /// </summary>
        [Test]
        public void FlaggedCountChanged_MinefieldSignalizedAboutIt()
        {
            IMinefield minefield = SetMineToCenter();

            minefield.OnFlagsCountChanged += Minefield_OnFlagsCountChanged;

            onFlagsCountChangedEvendReceaved = false;
            minefield.Cells[0][0].SetFlag();

            Assert.AreEqual(true, onFlagsCountChangedEvendReceaved);

            onFlagsCountChangedEvendReceaved = false;
            minefield.Cells[0][0].SetFlag();

            Assert.AreEqual(true, onFlagsCountChangedEvendReceaved);
        }

        [Test]
        public void GameRestart_FlagsCountReset()
        {
            IMinefield minefield = SetMineToCenter();
            SettingsItem settings = CreateCustomSettings(10, 10, 1);
            minefield.SetGameSettings(settings);

            minefield.Cells[0][0].SetFlag();
            minefield.Cells[1][1].SetFlag();

            minefield.Fill();

            Assert.AreEqual(0, minefield.FlagsCount);
        }

        private bool onFlagsCountChangedEvendReceaved;
        private void Minefield_OnFlagsCountChanged(object sender, EventArgs e)
        {
            onFlagsCountChangedEvendReceaved = true;
        }

        [TestCase(FieldState.GameOver, false)]
        [TestCase(FieldState.Win, false)]
        [TestCase(FieldState.NotStarted, true)]
        public void CanCellStateChangedInAllStates(FieldState gameState, bool expected)
        {
            var minefield = Substitute.For<Minefield>(new CellFactory(), new RandomMinePositionGenerator());
            minefield.State.Returns(gameState);

            Assert.AreEqual(expected, minefield.CellsStateCanBeChanged);
        }

        private static void WinGame(IMinefield minefield)
        {
            foreach (var row in minefield.Cells)
                foreach (var cell in row)
                    if (!cell.Mined)
                        cell.Open();
        }
        
        #region Вспомогательные методы
        private static IMinefield SetMineToCenter()
        {
            IMinefield field = SetMineToPosition(5, 5);
            return field;
        }

        private IEnumerable<ICell> GetNearbyFromCenter(IMinefield field)
        {
            List<ICell> nearby = new List<ICell>(8);
            nearby.Add(field.GetCellByCoords(4, 4));
            nearby.Add(field.GetCellByCoords(4, 5));
            nearby.Add(field.GetCellByCoords(4, 6));
            nearby.Add(field.GetCellByCoords(5, 4));
            nearby.Add(field.GetCellByCoords(5, 6));
            nearby.Add(field.GetCellByCoords(6, 4));
            nearby.Add(field.GetCellByCoords(6, 5));
            nearby.Add(field.GetCellByCoords(6, 6));
            return nearby;
        }

        private static IMinefield SetMineToPosition(int x, int y)
        {
            int mineCount = 1;
            List<int> coords = new List<int> { x, y };
            IMinePositionsGenerator generator = new CollectionMinePositionGenerator(coords);
            IMinefield field = new Minefield(new CellFactory(), generator);
            SettingsItem settings = CreateCustomSettings(10, 10, mineCount);
            field.SetGameSettings(settings);
            field.Fill();
            return field;
        }

        private static SettingsItem CreateCustomSettings(int height, int width, int mineCount)
        {
            return new SettingsItem()
            {
                Height = height,
                Width = width,
                MineCount = mineCount,
                Type = GameType.Custom
            };
        }

        #endregion

        #region Subtypes

        class CollectionMinePositionGenerator : IMinePositionsGenerator
        {
            private int step;
            IEnumerable<int> coords;

            public CollectionMinePositionGenerator(IEnumerable<int> coords)
            {
                this.coords = coords;
                step = 0;
            }

            public int Next(int max)
            {
                if (step >= coords.Count())
                    return step++;

                return coords.ElementAt(step++);
            }
        }
        #endregion 
    }
}
