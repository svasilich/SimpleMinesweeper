using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeperTests.Core
{
    [TestFixture]
    class CellFactoryTest
    {
        [Test]
        public void CreateCell()
        {
            ICellFactory factory = new CellFactory();
            IMinePositionsGenerator generator = Substitute.For<IMinePositionsGenerator>();
            IMinefield field = new Minefield(factory, generator);

            ICell cell = factory.CreateCell(field, 0, 0);

            Assert.AreEqual(new Cell(field, 0, 0), cell);
        }

    }
}
