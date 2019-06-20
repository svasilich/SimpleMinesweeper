using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMinesweeper.Core;

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
