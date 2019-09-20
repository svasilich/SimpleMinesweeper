using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeper.DialogWindows
{
    public interface IGetRecordsmanNameProvider
    {
        string GetRecordsmanName(GameType gameType, int recordTime);
    }
}
