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
            var vm = new TestMinefieldViewModel();

            Assert.AreEqual(TestMinefieldViewModel.DefaultMineCount, vm.MinesLeft);
        }

        [Test]
        public void FlagSetted_MinesLeftDown()
        {
            var vm = new TestMinefieldViewModel();

            vm.Field.Cells[0][0].SetFlag();

            Assert.AreEqual(TestMinefieldViewModel.DefaultMineCount - 1, vm.MinesLeft);
        }
    }
}
