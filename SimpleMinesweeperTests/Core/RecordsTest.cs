using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameRecords;
using System.IO;

namespace SimpleMinesweeperTests.Core
{

    [TestFixture]
    class RecordsTest
    {
        private static string RecordsPath = AppContext.BaseDirectory + "records.dat";

        [Test]
        public void AddNewRecordInEmptyList()
        {
            Records records = new Records();

            GameType gameType = GameType.Newbie;
            int time = 5;
            string playerName = "Вася Пупкин";
            records.UpdateRecord(gameType, time, playerName);

            List<IRecordItem> items = records.GetRecords();

            Assert.AreEqual(1, items.Count);

            var item = items[0];
            Assert.AreEqual(gameType, item.GameType);
            Assert.AreEqual(time, item.Time);
            Assert.AreEqual(playerName, item.Player);
        }

        [Test]
        public void AddRecordWithBigTime_ThrowException()
        {
            Records records = new Records();

            GameType gameType = GameType.Newbie;
            int recordTime = 10;
            string playerName = "Вася Пупкин";

            records.UpdateRecord(gameType, recordTime, playerName);

            int badTime = 15;
            var expected = "Время нового рекорда должно быть меньше";
            var ex = Assert.Catch<Exception>(() => records.UpdateRecord(gameType, badTime, playerName));
            StringAssert.Contains(expected.ToUpper(), ex.Message.ToUpper());
        }

        [Test]
        public void AddNewRecordWithAnotherGameType()
        {
            Records records = new Records();

            GameType gameType = GameType.Newbie;
            int recordTime = 10;
            string playerName = "Вася Пупкин";
            records.UpdateRecord(gameType, recordTime, playerName);

            GameType anotherType = GameType.Professional;
            int anotherTime = 15;
            string anotherName = "Иванов";
            records.UpdateRecord(anotherType, anotherTime, anotherName);

            var results = records.GetRecords();
            IRecordItem item = results.First(r => r.GameType == anotherType);

            Assert.AreEqual(anotherTime, item.Time);
            Assert.AreEqual(anotherName, item.Player);
        }

        [Test]
        public void UpdateExistsRecord()
        {
            Records records = new Records();

            GameType gameType = GameType.Newbie;
            int recordTime = 10;
            string playerName = "Вася Пупкин";
            records.UpdateRecord(gameType, recordTime, playerName);

            int anotherTime = 5;
            string anotherName = "Иванов";
            records.UpdateRecord(gameType, anotherTime, anotherName);

            List<IRecordItem> items = records.GetRecords();

            Assert.AreEqual(1, items.Count);

            var item = items[0];
            Assert.AreEqual(gameType, item.GameType);
            Assert.AreEqual(anotherTime, item.Time);
            Assert.AreEqual(anotherName, item.Player);
        }

        [TestCase(59, true)]
        [TestCase(60, false)]
        [TestCase(61, false)]
        public void CheckRecord(int time, bool expected)
        {
            IRecords records = GetStandardRecordsTable();
            bool result = records.IsRecord(GameType.Advanced, time);

            Assert.AreEqual(expected, result);
        }

        [TestCase(GameType.Newbie)]
        [TestCase(GameType.Advanced)]
        [TestCase(GameType.Professional)]
        public void CheckRecordWithEmptyList_AlwaysRecord(GameType gameType)
        {
            IRecords records = new Records();

            bool result = records.IsRecord(gameType, 1);

            Assert.IsTrue(result);
        }

        [TestCase(new GameType[] { GameType.Newbie, GameType.Advanced }, GameType.Professional)]
        [TestCase(new GameType[] { GameType.Professional, GameType.Advanced }, GameType.Newbie)]
        [TestCase(new GameType[] { GameType.Newbie, GameType.Professional }, GameType.Advanced)]
        public void CheckRecord_WithNoExistsGameType(GameType[] exists, GameType newType)
        {
            IRecords records = new Records();
            foreach (var t in exists)
                records.UpdateRecord(t, 1, "testPlayer");

            bool result = records.IsRecord(newType, 1);

            Assert.IsTrue(result);
        }


        [Test]
        public void AddCustomGameRecord_ThrowException()
        {
            IRecords records = GetStandardRecordsTable();

            var ex = Assert.Catch<ArgumentException>(() => records.UpdateRecord(GameType.Custom, 99, "Custom"));
            StringAssert.Contains("рекорд для случайной игры".ToUpper(), ex.Message.ToUpper());
        }

        [Test]
        public void UpdateRecord_EventHandled()
        {
            IRecords records = GetStandardRecordsTable();

            var registrator = new EventRegistrator();
            records.OnRecordChanged += registrator.Records_OnRecordChanged;

            records.UpdateRecord(GameType.Professional, 9, "Test");

            Assert.IsTrue(registrator.IsHandled);
        }

        [Test]
        public void CleareRecords()
        {
            IRecords records = GetStandardRecordsTable();

            records.Clear();

            Assert.AreEqual(0, records.GetRecords().Count);
        }

        [Test]
        public void LoadRecords_EventHandled()
        {
            IRecords records = GetStandardRecordsTable();
            records.Save();

            var registrator = new EventRegistrator();
            records.OnRecordChanged += registrator.Records_OnRecordChanged;

            records.Load();

            Assert.IsTrue(registrator.IsHandled);
            File.Delete(RecordsPath);
        }

        [Test]
        public void LoadRecord_PathNotSetted_GetException()
        {
            Records records = (Records)GetStandardRecordsTable();

            records.FilePath = string.Empty;

            var ex = Assert.Catch<Exception>(() => records.Load());
            StringAssert.Contains("не указав адрес файла храннения".ToLower(), ex.Message.ToLower());
        }

        [Test]
        public void SaveRecord_PathNotSetted_GetException()
        {
            Records records = (Records)GetStandardRecordsTable();

            records.FilePath = string.Empty;

            var ex = Assert.Catch<Exception>(() => records.Save());
            StringAssert.Contains("не указав адрес файла храннения".ToLower(), ex.Message.ToLower());
        }

        #region Help methods

        public static IRecords GetStandardRecordsTable()
        {
            Records result = new Records(RecordsPath);

            result.UpdateRecord(GameType.Newbie, 10, "Newbie");
            result.UpdateRecord(GameType.Advanced, 60, "Advanced");
            result.UpdateRecord(GameType.Professional, 100, "Professional");

            return result;
        }

        private class EventRegistrator
        {
            public bool IsHandled { get; private set; }

            public EventRegistrator()
            {
                IsHandled = false;
            }
            public void Records_OnRecordChanged(object sender, EventArgs e)
            {
                IsHandled = true;
            }

        }

        #endregion
    }
}
