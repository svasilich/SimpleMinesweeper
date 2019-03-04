using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using SimpleMinesweeper.Core;
using SimpleMinesweeper.ViewModel;
using SimpleMinesweeperTests.Common;

namespace SimpleMinesweeperTests.ViewModel
{
    [TestFixture]
    class MinefieldViewModelMinesCountTests
    {
        [Test]
        public void GameStart_AllMinesLeft()
        {
            var vm = new MinefieldViewModelTest();

            Assert.AreEqual(MinefieldViewModelTest.DefaultMineCount, vm.MinesLeft);
        }

        [Test]
        public void FlagSetted_MinesLeftDown()
        {
            var vm = new MinefieldViewModelTest();

            vm.Field.Cells[0][0].SetFlag();

            Assert.AreEqual(MinefieldViewModelTest.DefaultMineCount - 1, vm.MinesLeft);
        }
        
        [Test]
        public void OpenAllMines_MinenLeftToZero()
        {
            var vm = new MinefieldViewModelTest();
            var cells = vm.Field.Cells;

            for (int i = 0; i < MinefieldViewModelTest.DefaultFieldWidthCell; ++i)
                for (int j = 0; j < MinefieldViewModelTest.DefaultFieldHeightCell; ++j)
                {
                    var cell = cells[j][i];
                    if (cell.Mined)
                        cell.SetFlag();
                }

            Assert.AreEqual(0, vm.MinesLeft);
        }
    }
}
