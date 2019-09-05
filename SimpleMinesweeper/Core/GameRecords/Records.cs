using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace SimpleMinesweeper.Core.GameRecords
{
    public class Records : IRecords
    {
        #region Consts
        private const int RecordListSize = 3;
        #endregion

        #region Fields
        public string FilePath { get; set; }
        private List<IRecordItem> records;
        #endregion

        #region Constructor
        public Records()
        {
            records = new List<IRecordItem>();
        }

        public Records(string saveLoasPath) : this()
        {
            FilePath = saveLoasPath;
        }

        #endregion

        public event EventHandler OnRecordChanged;

        public List<IRecordItem> GetRecords()
        {
            return records;
        }

        public bool IsRecord(GameType gameType, int seconds)
        {
            if (records == null)
                return true;

            IRecordItem ri = records.FirstOrDefault(r => r.GameType == gameType);
            if (ri == null)
                return true;

            return ri.Time > seconds;
        }

        public void Clear()
        {
            records.Clear();
            OnRecordChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Load()
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                throw new Exception("Попытка вызвать загрузку таблицы рекордов не указав адрес файла храннения.");

            if (!File.Exists(FilePath))
                return;

            using (Stream recordsFile = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                records.Clear();
                var serializer = new XmlSerializer(typeof(List<RecordItem>));
                var data = (List<RecordItem>)serializer.Deserialize(recordsFile);
                foreach (var d in data)
                    records.Add(d);
            }

            OnRecordChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                throw new Exception("Попытка вызвать сохранение таблицы рекордов не указав адрес файла храннения.");

            using (Stream recordsFile = new FileStream(FilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {                
                var serializeData = new List<RecordItem>(records.Count);
                foreach (var ri in records)
                {
                    var item = (RecordItem)ri;
                    serializeData.Add(item);
                }

                var serializer = new XmlSerializer(typeof(List<RecordItem>));
                serializer.Serialize(recordsFile, serializeData);
            }
        }

        public void UpdateRecord(GameType gameType, int seconds, string player)
        {
            if (gameType == GameType.Custom)
                throw new ArgumentException("Невозможно добавить рекорд для случайной игры.");

            if (records == null)
                records = new List<IRecordItem>(RecordListSize);

            IRecordItem ri = records.FirstOrDefault(r => r.GameType == gameType);
            RecordItem recordItem = (RecordItem)ri;
            if (recordItem != null)
            {
                if (recordItem.Time < seconds)
                    throw new ArgumentException($"Прошлый рекорд для этого типа игры {recordItem.Time} секунд. Время нового рекорда должно быть меньше.");

                recordItem.Time = seconds;
                recordItem.Player = player;
            }
            else
            {
                RecordItem newRecord = new RecordItem
                {
                    GameType = gameType,
                    Time = seconds,
                    Player = player
                };
                records.Add(newRecord);
            }

            OnRecordChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
