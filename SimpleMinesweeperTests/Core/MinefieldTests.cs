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
            return new Minefield(new CellFactory(), new RandomMinePositionGenerator());
        }

        [TestCase(0, 1, 1, "высота")]
        [TestCase(1, 0, 1, "ширина")]
        [TestCase(1, 1, 0, "количество мин должно быть больше")]
        [TestCase(5, 5, 100, "слишком много")]
        public void SendFillIncorrectParameters_ThrowException(int height, int length, int mineCount, string expected)
        {
            IMinefield minefield = CreateDefaultMineField();

            var ex = Assert.Catch<ArgumentException>(() => minefield.Fill(height, length, mineCount));
            StringAssert.Contains(expected.ToUpper(), ex.Message.ToUpper());
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
            field.Fill(10, 10, 3);

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

        [Test]        
        public void FirstClick_StateChangeToInGame()
        {
            IMinefield minefield = SetMineToCenter();

            minefield.Cells[0][0].Open();

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
            field.Fill(10, 10, 3);
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

        [Test]
        public void FillMinefield_StateToNotStarted()
        {
            IMinefield minefield = SetMineToCenter();
            minefield.Cells[0][0].Open();
            minefield.Fill(10, 10, 1);

            Assert.AreEqual(FieldState.NotStarted, minefield.State);
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

            field.Fill(10, 10, mineCount);
            return field;
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
