using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeper.Core.GameRecords
{
    public interface IRecords
    {
        List<IRecordItem> GetRecords();

        bool IsRecord(GameType gameType, int seconds);

        void UpdateRecord(GameType gameType, int seconds, string player);

        void Clear();

        event EventHandler OnRecordChanged;

        void Load();
        void Save();
    }
}
