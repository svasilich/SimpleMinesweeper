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
        public void MineFieldAndCheckCorrectMineCount()
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
