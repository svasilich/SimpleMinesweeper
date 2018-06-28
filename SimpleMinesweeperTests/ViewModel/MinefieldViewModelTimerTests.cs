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
    class MinefieldViewModelTimerTests
    {
        [Test]
        public void GameTimer_TimerObjectNotSet_ReturnZeru()
        {
            var sm = new MinefieldViewModelWithManualTimerObject();
            sm.SetTimerObject(null);

            int ticks = sm.GameTime;

            Assert.AreEqual(0, ticks);
        }

        [Test]
        public void GameTimer_TimerObjectSetted_ReturnToTime()
        {
            var sm = new MinefieldViewModelWithManualTimerObject();
            IGameTimer gameTimer = Substitute.For<IGameTimer>();

            int expectedTicks = 10;

            gameTimer.Seconds.Returns(expectedTicks);
            sm.SetTimerObject(gameTimer);

            Assert.AreEqual(expectedTicks, sm.GameTime);


        }

        class MinefieldViewModelWithManualTimerObject : MinefieldViewModel
        {
            public void SetTimerObject(IGameTimer timerObject)
            {
                gameTimer = timerObject;
            }

            public MinefieldViewModelWithManualTimerObject() : base(MinefieldTestHelper.CreateDefaultMinefield(), MinefieldTestHelper.FakeMainWindow())
            {
            }
        }
    }
}
