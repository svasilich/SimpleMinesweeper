using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeper.Core.GameRecords
{
    public interface IRecordItem
    {

        GameType GameType { get; }
        int Time { get; }
        string Player { get; }
    }
}
