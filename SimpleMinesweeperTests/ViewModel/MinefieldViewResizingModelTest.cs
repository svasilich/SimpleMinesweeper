using System;
using NUnit.Framework;

namespace SimpleMinesweeperTests.ViewModel
{

    [TestFixture]
    class MinefieldViewModelResizingTest
    {
        const double cellSizePx = 5;

        [TestCase(30, 16, 16, 30)]
        [TestCase(30, 16, 30, 16)]
        [TestCase(30, 16, 16, 16)]
        [TestCase(30, 16, 16, 18)]
        [TestCase(30, 16, 18, 16)]
        [TestCase(30, 16, 100, 1)]
        [TestCase(30, 16, 1, 100)]
        [TestCase(16, 30, 16, 30)]
        [TestCase(16, 30, 30, 16)]
        [TestCase(16, 30, 16, 16)]
        [TestCase(16, 30, 16, 18)]
        [TestCase(16, 30, 18, 16)]
        [TestCase(16, 30, 100, 1)]
        [TestCase(16, 30, 1, 100)]
        public void MinefieldResize_WidthInContainer(int fieldHeight, int fieldWidth, double heightRatio, double widthRatio)
        {
            MinefieldViewModelTest vm = new MinefieldViewModelTest(fieldHeight, fieldWidth);
            double height = cellSizePx * heightRatio;
            double width = cellSizePx * widthRatio;

            vm.SetSize(width, height);

            Assert.LessOrEqual(vm.FieldWidthPx, width);
        }

        [TestCase(30, 16, 16, 30)]
        [TestCase(30, 16, 30, 16)]
        [TestCase(30, 16, 16, 16)]
        [TestCase(30, 16, 16, 18)]
        [TestCase(30, 16, 18, 16)]
        [TestCase(30, 16, 100, 1)]
        [TestCase(30, 16, 1, 100)]
        [TestCase(16, 30, 16, 30)]
        [TestCase(16, 30, 30, 16)]
        [TestCase(16, 30, 16, 16)]
        [TestCase(16, 30, 16, 18)]
        [TestCase(16, 30, 18, 16)]
        [TestCase(16, 30, 100, 1)]
        [TestCase(16, 30, 1, 100)]
        public void MinefieldResize_HeightInContainer(int fieldHeight, int fieldWidth, double heightRatio, double widthRatio)
        {
            MinefieldViewModelTest vm = new MinefieldViewModelTest(fieldHeight, fieldWidth);
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
        public void CellProportionsMustBeNearSquare(int fieldHeight, int fieldWidth, double heightRatio, double widthRatio)
        {
            // Клетки должны быть примерно квадратными не зависимо от соотношения сторон поля. 
            // Допустимо отклонение в 1-2 пикселя.
            //Так же нужно не забыть выполнить рефакторинг MinefieldViewModel:ResizeField

            MinefieldViewModelTest vm = new MinefieldViewModelTest(fieldHeight, fieldWidth);
            double height = cellSizePx * heightRatio;
            double width = cellSizePx * widthRatio;

            vm.SetSize(width, height);

            double cellHeight = vm.FieldHeightPx / fieldHeight;
            double cellWidth = vm.FieldWidthPx / fieldWidth;

            double allowableError = 2;
            Assert.LessOrEqual(Math.Abs(cellHeight - cellWidth), allowableError);

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
        public void OneOfSideAllwaysHasMaximum(int fieldHeight, int fieldWidth, double heightRatio, double widthRatio)
        {
            MinefieldViewModelTest vm = new MinefieldViewModelTest(fieldHeight, fieldWidth);

            double height = cellSizePx * heightRatio;
            double width = cellSizePx * widthRatio;

            vm.SetSize(width, height);

            bool widthIsMax = width == vm.FieldWidthPx;
            bool heightIsMax = height == vm.FieldHeightPx;

            Assert.IsTrue(widthIsMax || heightIsMax);
        } 
    }
}
