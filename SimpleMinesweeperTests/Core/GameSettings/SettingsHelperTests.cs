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

        [TestCase(0, 5, 5, "высота")]
        [TestCase(5, 0, 5, "ширина")]
        [TestCase(5, 5, 0, "мин на поле не может быть меньше")]
        [TestCase(5, 5, 100, "слишком много")]
        public void CheckValidityWithIncorrectParameters_ReturnFalseWithReason(int height, int width, int mineCount, string expected)
        {
            bool isValid = SettingsHelper.CheckValidity(height, width, mineCount, out string reason);

            Assert.IsFalse(isValid);
            StringAssert.Contains(expected.ToUpper(), reason.ToUpper());
        }
    }
}
