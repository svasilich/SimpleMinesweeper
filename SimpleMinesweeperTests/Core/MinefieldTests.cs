using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeperTests.Core
{
    [TestFixture]
    class MinefieldTests
    {
        private IMinefield CreateDefaultMineField()
        {
            return new Minefield(new RandomMinePositionGenerator());
        }

        [TestCase(0, 1, 1, "высота")]
        [TestCase(1, 0, 1, "ширина")]
        [TestCase(1, 1, 0, "количество мин должно быть больше")]
        [TestCase(5, 5, 100, "слишком много")]
        public void SendFillIncorrectParametersThrowException(int height, int length, int mineCount, string expected)
        {
            IMinefield minefield = CreateDefaultMineField();

            var ex = Assert.Catch<ArgumentException>(() => minefield.Fill(height, length, mineCount));
            StringAssert.Contains(expected.ToUpper(), ex.Message.ToUpper());
        }

        [Test]
        public void FillMineFieldAndCheckCorrectMineCount()
        {
            int mineCount = 30;
            IMinefield mineField = CreateDefaultMineField();
            mineField.Fill(10, 10, mineCount);

            int factMineCount = mineField.Cells.Sum(x => x.Sum(y => y.Mined ? 1 : 0));
            Assert.AreEqual(mineCount, factMineCount);
        }

        [Test]
        public void FillMineFieldAndCheckMinedPosition()
        {
            int mineCount = 3;
            List<int> coords = new List<int> { 0, 0, 5, 5, 9, 9 };
            IMinePositionsGenerator generator = new CollectionMinePositionGenerator(coords);
            IMinefield field = new Minefield(generator);

            field.Fill(10, 10, mineCount);

            for (int y = 0; y < 10; ++y)
                for (int x = 0; x < 10; ++x)
                {
                    if ((x == 0 && y == 0) ||
                        (x == 5 && y == 5) ||
                        (x == 9 && y == 9))
                        Assert.True(field.GetCellByCoords(x, y).Mined);
                    else
                        Assert.False(field.GetCellByCoords(x, y).Mined);

                }
        }

        [Test]
        public void OpenCellAndCheckSender()
        {
            MinefieldCellEventTest field = new MinefieldCellEventTest(new RandomMinePositionGenerator());

            field.Fill(10, 10, 30);

            ICell clickedCell = field.GetCellByCoords(1, 1);
            clickedCell.Open();

            Assert.AreSame(clickedCell, field.OpenedCell);
        }
        
        [Test]
        public void SetCellCellAndCheckSender()
        {
            MinefieldCellEventTest field = new MinefieldCellEventTest(new RandomMinePositionGenerator());

            field.Fill(10, 10, 30);

            ICell clickedCell = field.GetCellByCoords(1, 1);
            clickedCell.SetFlag();

            Assert.AreSame(clickedCell, field.FlaggedCell);
        }

        [Test]
        public void SetMineToCenterAndCheckNearby()
        {
            IMinefield field = SetMineToCenter();
            IEnumerable<ICell> toCheck = GetNearbyFromCenter(field);

            foreach (var cell in toCheck)
                Assert.AreEqual(1, cell.MinesNearby, $"X:{cell.CoordX}; Y:{cell.CoordY}");
        }

        [Test]
        public void SetMineToCornerAndCheckNearby()
        {
            IMinefield field = SetMineToPosition(0, 0);

            List<ICell> toCheck = new List<ICell>(3);
            toCheck.Add(field.GetCellByCoords(0, 1));
            toCheck.Add(field.GetCellByCoords(1, 1));
            toCheck.Add(field.GetCellByCoords(1, 0));

            foreach (var cell in toCheck)
                Assert.AreEqual(1, cell.MinesNearby, $"X:{cell.CoordX}; Y:{cell.CoordY}");
        }

        [Test]
        public void SetMineToCenterThenUnmineAndCellNearbyHesNoMined()
        {
            IMinefield field = SetMineToCenter();
            IEnumerable<ICell> toCheck = GetNearbyFromCenter(field);

            ICell center = field.GetCellByCoords(5, 5);
            ((Minefield)field).RemoveMineFromCell(center);

            foreach (var cell in toCheck)
                Assert.AreEqual(0, cell.MinesNearby, $"X:{cell.CoordX}; Y:{cell.CoordY}");
        }

        [Test]
        public void SetMineToCenterAndItselfHasNoMinedNearby()
        {
            IMinefield field = SetMineToCenter();
            IEnumerable<ICell> toCheck = GetNearbyFromCenter(field);

            ICell center = field.GetCellByCoords(5, 5);

            Assert.AreEqual(0, center.MinesNearby);
        }

        [TestCase(CellState.BlownUpped, CellState.BlownUpped)]
        [TestCase(CellState.Flagged, CellState.NotOpened)]
        [TestCase(CellState.NotOpened, CellState.Flagged)]
        [TestCase(CellState.Opened, CellState.Opened)]
        public void SetFlagToCell(CellState begin, CellState expected)
        {
            IMinefield field = SetMineToCenter();

            ICell testCell = field.GetCellByCoords(0, 0);
            testCell.State = begin;
            testCell.SetFlag();

            Assert.AreEqual(expected, testCell.State);
        }

        [TestCase(5, 5, CellState.BlownUpped)]
        [TestCase(0, 0, CellState.Opened)]
        public void OpenCell(int x, int y, CellState expected)
        {
            IMinefield field = SetMineToCenter();

            ICell testCell = field.GetCellByCoords(x, y);
            testCell.Open();

            Assert.AreEqual(expected, testCell.State);
        }

        [TestCase(2, 2, 1, 1, CellState.Opened)]
        [TestCase(4, 4, 3, 3, CellState.NotOpened)]
        public void OpenNoMinedCellAndCheckNearbyOpening(int xOpen, int yOpen, int xCheck, int yCheck, CellState expected)
        {
            IMinefield field = SetMineToCenter();

            ICell cellToOpen = field.GetCellByCoords(xOpen, yOpen);
            cellToOpen.Open();

            ICell cellToCheck = field.GetCellByCoords(xCheck, yCheck);

            Assert.AreEqual(expected, cellToCheck.State);
        }

        [Test]
        public void GameStartingAfterCellOpened()
        {
            IMinefield minefield = SetMineToCenter();

            minefield.Cells[0][0].Open();

            Assert.AreEqual(FieldState.InGame, minefield.State);
        }

        [Test]
        public void GameOverSetWrongFlagsAndNoFindedMines()
        {
            List<int> coords = new List<int> { 5, 5, 1, 2 };
            IMinePositionsGenerator generator = new CollectionMinePositionGenerator(coords);
            IMinefield minefield = new Minefield(generator);
            minefield.Fill(10, 10, 2);

            ICell flaggedCell = minefield.GetCellByCoords(1, 1);
            flaggedCell.SetFlag();

            ICell minedCell = minefield.GetCellByCoords(5, 5);
            minedCell.Open();

            ICell noFindedMine = minefield.GetCellByCoords(1, 2);

            Assert.AreEqual(FieldState.GameOver, minefield.State);
            Assert.AreEqual(CellState.WrongFlag, flaggedCell.State);
            Assert.AreEqual(CellState.NoFindedMine, noFindedMine.State);
        }

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
            IMinefield field = new Minefield(generator);

            field.Fill(10, 10, mineCount);
            return field;
        }

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
                return coords.ElementAt(step++);
            }
        }

        class MinefieldCellEventTest : Minefield
        {
            public ICell OpenedCell { get; private set; }
            public ICell FlaggedCell { get; private set; }

            public MinefieldCellEventTest(IMinePositionsGenerator minePositionsGenerator) : base(minePositionsGenerator)
            {
            }

            protected override void Cell_OnOpen(object sender, EventArgs e)
            {
                OpenedCell = (ICell)sender;
            }

            protected override void Cell_OnSetFlag(object sender, EventArgs e)
            {
                FlaggedCell = (ICell)sender;
            }
        }
        #endregion 
    }
}
