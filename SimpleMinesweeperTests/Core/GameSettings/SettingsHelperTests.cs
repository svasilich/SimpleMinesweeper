using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleMinesweeper.Core.GameSettings;

namespace SimpleMinesweeperTests.Core.GameSettings
{
    [TestFixture]
    class SettingsHelperTests
    {
        [Test]
        public void CheckValidityWithCorrectParameters_ReturnTrue()
        {
            bool isValid = SettingsHelper.CheckValidity(10, 10, 9, out string reason);

            Assert.IsTrue(isValid);
            Assert.IsTrue(string.IsNullOrEmpty(reason));
        }

        [TestCase(0, 1, 1, "высота")]
        [TestCase(1, 0, 1, "ширина")]
        [TestCase(1, 1, 0, "количество мин должно быть больше")]
        [TestCase(5, 5, 100, "слишком много")]
        public void CheckValidityWithIncorrectParameters_ReturnFalseWithReason(int height, int width, int mineCount, string expected)
        {
            bool isValid = SettingsHelper.CheckValidity(height, width, mineCount, out string reason);

            Assert.IsFalse(isValid);
            StringAssert.Contains(expected.ToUpper(), reason.ToUpper());
        }
    }
}
