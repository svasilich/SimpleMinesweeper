using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using SimpleMinesweeper.ViewModel;

namespace SimpleMinesweeperTests.ViewModel
{
    [TestFixture]
    class TimerSecundsConverterTests
    {
        [TestCase(0, "00:00")]
        [TestCase(60, "01:00")]
        [TestCase(5999, "99:59")]
        [TestCase(6000, "100:00")]
        [TestCase(60000, "999:59")]
        public void TestTimeConvertions(int seconds, string expected)
        {
            var converter = new TimerSecondsConverter();
            string result = (string)converter.Convert(seconds, null, null, null);

            Assert.AreEqual(expected, result);
        }
    }
}
