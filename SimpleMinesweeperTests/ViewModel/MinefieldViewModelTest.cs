using System;
using NUnit.Framework;

using SimpleMinesweeper.Core;
using SimpleMinesweeper.ViewModel;

using SimpleMinesweeperTests.Common;

namespace SimpleMinesweeperTests.ViewModel
{
    [TestFixture]
    class MinefieldViewModelTest
    {
        const double cellSizePx = 5;

        [TestCase(16, 30)]
        [TestCase(30, 16)]
        [TestCase(16, 16)]
        [TestCase(16, 18)]
        [TestCase(18, 16)]
        [TestCase(100, 1)]
        [TestCase(1, 100)]
        public void MinefieldResize_WidthInContainer(double heightRatio, double widthRatio)
        {
            TestMinefieldViewModel vm = new TestMinefieldViewModel();
            double height = cellSizePx * heightRatio;
            double width = cellSizePx * widthRatio;

            vm.SetSize(width, height);

            Assert.LessOrEqual(vm.FieldWidthPx, width);
        }

        [TestCase(16, 30)]
        [TestCase(30, 16)]
        [TestCase(16, 16)]
        [TestCase(16, 18)]
        [TestCase(18, 16)]
        [TestCase(100, 1)]
        [TestCase(1, 100)]
        public void MinefieldResize_HeightInContainer(double heightRatio, double widthRatio)
        {
            TestMinefieldViewModel vm = new TestMinefieldViewModel();
            double height = cellSizePx * heightRatio;
            double width = cellSizePx * widthRatio;

            vm.SetSize(width, height);

            Assert.LessOrEqual(vm.FieldHeightPx, height);
        }

        [TestCase(16, 30, 16, 30)]
        [TestCase(16, 30, 30, 16)]
        [TestCase(16, 30, 16, 16)]
        [TestCase(16, 30, 16, 18)]
        [TestCase(16, 30, 18, 16)]
        [TestCase(16, 30, 100, 1)]
        [TestCase(16, 30, 1, 100)]
        [TestCase(30, 16, 16, 30)]
        [TestCase(30, 16, 30, 16)]
        [TestCase(30, 16, 16, 16)]
        [TestCase(30, 16, 16, 18)]
        [TestCase(30, 16, 18, 16)]        
        [TestCase(16, 30, 100, 1)]
        [TestCase(16, 30, 1, 100)]
        public void CellProportionsMustBeNearSquare(int fieldHeight, int fieldWidth, double heightRatio, double widthRatio)
        {
            // Клетки должны быть примерно квадратными не зависимо от соотношения сторон поля. 
            // Допустимо отклонение в 1-2 пикселя.
            //Так же нужно не забыть выполнить рефакторинг MinefieldViewModel:ResizeField

            TestMinefieldViewModel vm = new TestMinefieldViewModel(fieldHeight, fieldWidth);
            double height = cellSizePx * heightRatio;
            double width = cellSizePx * widthRatio;

            vm.SetSize(width, height);

            double cellHeight = vm.FieldHeightPx / fieldHeight;
            double cellWidth = vm.FieldWidthPx / fieldWidth;

            double allowableError = 2;
            Assert.LessOrEqual(Math.Abs(cellHeight - cellWidth), allowableError);

        }

        class TestMinefieldViewModel : MinefieldViewModel
        {
            public static int DefaultFieldHeightCell { get { return 16; } }
            public static int DefaultFieldWidthCell { get { return 30; } }
            public static int DefaultMineCount { get { return 99; } }


            public static IMinefield DefaultMinefield(int fieldHeightCell, int fieldWidthCell)
            {
                IMinefield field = MinefieldTestHelper.CreateDefaultMinefield();
                field.Fill(fieldHeightCell, fieldWidthCell, DefaultMineCount);
                return field;
            }

            public TestMinefieldViewModel() : 
                base(DefaultMinefield(DefaultFieldHeightCell, DefaultFieldWidthCell))
            {

            }

            public TestMinefieldViewModel(int height, int width) :
                base(DefaultMinefield(height, width))
            {

            }

            public TestMinefieldViewModel(IMinefield minefield) : base(minefield)
            {
            }

            public void SetSize(double widthPx, double heightPx)
            {
                ResizeField(widthPx, heightPx);
            }
        }
    }
}
