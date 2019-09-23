using System;
using System.Collections.Generic;

namespace SimpleMinesweeper.Core.GameRecords
{
    public interface IRecords
    {
        List<IRecordItem> GetRecords();

        bool IsRecord(GameType gameType, int seconds);

        void UpdateRecord(GameType gameType, int seconds, string player);

        void Clear();        

        void Load();

        void Save();

        event EventHandler OnRecordChanged;
    }
}
