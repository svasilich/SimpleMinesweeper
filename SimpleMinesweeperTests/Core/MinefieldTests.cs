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
