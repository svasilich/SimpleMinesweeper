using System;
using System.Linq;
using NUnit.Framework;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeperTests.Core
{
    [TestFixture]
    class MinefieldTests
    {
        [TestCase(0, 1, 1, "высота")]
        [TestCase(1, 0, 1, "ширина")]
        [TestCase(1, 1, 0, "количество мин должно быть больше")]
        [TestCase(5, 5, 100, "слишком много")]
        public void SendFillIncorrectParametersThrowException(int height, int length, int mineCount, string expected)
        {
            IMinefield minefield = new Minefield();

            var ex = Assert.Catch<ArgumentException>(() => minefield.Fill(height, length, mineCount));
            StringAssert.Contains(expected.ToUpper(), ex.Message.ToUpper());
        }

        [Test]
        public void MineFieldAndCheckCorrectMineCount()
        {
            int mineCount = 30;
            IMinefield mineField = new Minefield();
            mineField.Fill(10, 10, mineCount);

            int factMineCount = mineField.Cells.Sum(x => x.Sum(y => y.Mined ? 1 : 0));
            Assert.AreEqual(mineCount, factMineCount);
        }
    }
}
