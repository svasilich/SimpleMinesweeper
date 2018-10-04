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
    class SettingsTest
    {
        // SettingsManager должен предоставлять все существующие типы.
        [Test]
        public void GetAvailableTypes_CheckDefaultParams()
        {
            SettingsManager manager = new SettingsManager();

            var settings = manager.AvailableGameTypes;

            var values = Enum.GetValues(typeof(GameType));
            foreach (var enumVal in values)
                Assert.That(settings.Any(x => x.Type == (GameType)enumVal));
        }

        // Проверить стандартные размеры.
        [TestCase(GameType.Newbie, 9, 9, 10)]
        [TestCase(GameType.Advanced, 16, 16, 40)]
        [TestCase(GameType.Professional, 16, 30, 99)]
        public void CheckDefaultTypesPatameters(GameType gameType, int height, int width, int mineCount)
        {
            SettingsManager manager = new SettingsManager();

            var settings = manager.AvailableGameTypes;

            SettingsItem item = settings.Where(x => x.Type == gameType).First();
            Assert.AreEqual(height, item.Height);
            Assert.AreEqual(width, item.Width);
            Assert.AreEqual(mineCount, item.MineCount);
        }

        // Установить тип игры и проверить результаты.
        [TestCase(GameType.Newbie)]
        [TestCase(GameType.Advanced)]
        [TestCase(GameType.Professional)]
        [TestCase(GameType.Custom)]
        public void SetGameType_CheckCurrentSettingsItemType(GameType testType)
        {
            SettingsManager manager = new SettingsManager();

            manager.SelectGameType(testType);

            Assert.AreEqual(testType, manager.Current.Type);
        }

        // Установить корректные размеры для пользовательской игры и проверить результаты.
        [TestCase(100, 100, 1000)]
        public void SetCustomTypeSize_CheckResult(int expectedWidth, int expectedHeight, int expectedMineCount)
        {
            SettingsManager manager = new SettingsManager();
            
            manager.SetCustomSize(expectedHeight, expectedWidth, expectedMineCount);
            manager.SelectGameType(GameType.Custom);

            Assert.AreEqual(expectedHeight, manager.Current.Height);
            Assert.AreEqual(expectedWidth, manager.Current.Width);
            Assert.AreEqual(expectedMineCount, manager.Current.MineCount);
        }

        // Тестирование взаимодействия с файловой системой.
        [Test]
        public void TestSaveLoadToFile()
        {
            // Создать экземпляр.
            SettingsManager source = new SettingsManager();


            // Изменить его настройки.
            source.SelectGameType(GameType.Advanced);

            int testHeight = 11;
            int testWidth = 15;
            int testMineCount = 25;            
            source.SetCustomSize(testHeight, testWidth, testMineCount);


            // Сохранить во временный файл.
            string fileName = AppContext.BaseDirectory + "settings_test.dat";
            source.Save(fileName);

            // Создать новый экземпляр из временного файла.
            SettingsManager result = new SettingsManager();
            result.Load(fileName);

            // Проверить эквивалентность созранённых настроек.
            Assert.AreEqual(source.Current.Type, result.Current.Type);

            SettingsItem sourceCustom = source.GetItemByType(GameType.Custom);
            SettingsItem resultCustom = result.GetItemByType(GameType.Custom);
            Assert.AreEqual(sourceCustom.Height, resultCustom.Height);
            Assert.AreEqual(sourceCustom.Width, resultCustom.Width);
            Assert.AreEqual(sourceCustom.MineCount, resultCustom.MineCount);
        }
    }
}
