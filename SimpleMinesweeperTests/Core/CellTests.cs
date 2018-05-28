using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeperTest.Core
{
    [TestFixture]
    class CellTests
    {
        private IMinefield GetFakeField()
        {
            IMinefield minefield = Substitute.For<IMinefield>();
            minefield.CellsStateCanBeChanged.Returns(true);
            return minefield;
        }

        [Test]
        public void SetFlagToNotOpenedCell_StateChangeToFlaged()
        {
            ICell cell = new Cell(GetFakeField(), 0, 0)
            {
                State = CellState.NotOpened
            };

            cell.SetFlag();

            Assert.AreEqual(CellState.Flagged, cell.State);
        }

        [TestCase(CellState.Explosion)]
        [TestCase(CellState.NoFindedMine)]
        [TestCase(CellState.Opened)]
        [TestCase(CellState.WrongFlag)]
        public void SetFlagToOpenedAndNotFlaggedCell_StateNotChanged(CellState initState)
        {
            ICell cell = new Cell(GetFakeField(), 0, 0)
            {
                State = initState
            };

            cell.SetFlag();

            Assert.AreEqual(initState, cell.State);
        }

        [Test]
        public void SetFlagToFlaggedCell_StateChangeToNotOpened()
        {
            ICell cell = new Cell(GetFakeField(), 0, 0)
            {
                State = CellState.Flagged
            };

            cell.SetFlag();

            Assert.AreEqual(CellState.NotOpened, cell.State);
        }

        [Test]
        public void OpenMinedCell_StateChangeToBlownUp()
        {
            ICell cell = new Cell(GetFakeField(), 0, 0)
            {
                State = CellState.NotOpened,
                Mined = true
            };

            cell.Open();

            Assert.AreEqual(CellState.Explosion, cell.State);
        }

        [Test]
        public void OpenNoMinedCell_StateChangeToOpened()
        {
            ICell cell = new Cell(GetFakeField(), 0, 0)
            {
                State = CellState.NotOpened,
                Mined = false
            };

            cell.Open();

            Assert.AreEqual(CellState.Opened, cell.State);
        }

        private CellState newCellState;
        [TestCase(CellState.Explosion)]
        [TestCase(CellState.Flagged)]
        [TestCase(CellState.NoFindedMine)]
        [TestCase(CellState.Opened)]
        [TestCase(CellState.WrongFlag)]
        public void CellStateChanged_InvokeEventAboutIt(CellState state)
        {
            newCellState = CellState.NotOpened;
            ICell cell = new Cell(GetFakeField(), 0, 0);

            cell.OnStateChanged += Cell_OnStateChanged;

            Assert.AreEqual(newCellState, cell.State);
        }

        private void Cell_OnStateChanged(object sender, CellChangeStateEventArgs e)
        {
            newCellState = e.NewState;
        }

        [TestCase(CellState.Explosion)]
        [TestCase(CellState.Flagged)]
        [TestCase(CellState.NoFindedMine)]
        [TestCase(CellState.Opened)]
        [TestCase(CellState.WrongFlag)]
        public void TryOpenOpenedOrFlaggedCell_NoStateChanged(CellState state)
        {
            ICell cell = new Cell(GetFakeField(), 0, 0)
            {
                State = state
            };

            cell.Open();

            Assert.AreEqual(state, cell.State);
        }

        [Test]
        public void TryOpenCellAfterGameOver_StateNoChanged()
        {
            IMinefield minefield = CreateGameOveredMinefield();
            ICell cell = new Cell(minefield, 0, 0)
            {
                State = CellState.NotOpened
            };

            cell.Open();

            Assert.AreEqual(CellState.NotOpened, cell.State);
        }

        [Test]
        public void TrySetFlagAfterGameOver_StateNoChanged()
        {
            IMinefield minefield = CreateGameOveredMinefield();
            ICell cell = new Cell(minefield, 0, 0)
            {
                State = CellState.NotOpened
            };

            cell.SetFlag();

            Assert.AreEqual(CellState.NotOpened, cell.State);
        }

        private static IMinefield CreateGameOveredMinefield()
        {
            IMinefield minefield = Substitute.For<IMinefield>();
            minefield.State.Returns(FieldState.GameOver);
            return minefield;
        }

        private bool cellOpenCalled;
        [Test]
        public void OpenCell_InvokeEventAboutIt()
        {
            cellOpenCalled = false;
            ICell cell = new Cell(GetFakeField(), 0, 0)
            {
                State = CellState.NotOpened
            };
            cell.OnStateChanged += Cell_OnStateChanged1;

            cell.Open();

            Assert.True(cellOpenCalled);
        }

        private void Cell_OnStateChanged1(object sender, CellChangeStateEventArgs e)
        {
            cellOpenCalled = true;
        }

        private bool flagSetted;
        [Test]
        public void SetFlagToCell_InvokeEventAboutIt()
        {
            flagSetted = false;
            ICell cell = new Cell(GetFakeField(), 0, 0)
            {
                State = CellState.NotOpened
            };
            cell.OnStateChanged += Cell_OnStateChanged2; ;

            cell.SetFlag();

            Assert.True(flagSetted);
        }

        private void Cell_OnStateChanged2(object sender, CellChangeStateEventArgs e)
        {
            flagSetted = true;
        }
    }
}