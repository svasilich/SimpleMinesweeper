using System;

namespace SimpleMinesweeper.Core.GameRecords
{
    [Serializable]
    public class RecordItem : IRecordItem
    {
        public GameType GameType { get; set; }

        public int Time { get; set; }

        public string Player { get; set; }
    }
}
