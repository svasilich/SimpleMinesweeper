using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeper.DialogWindows
{
    public class GetRecordsmanNameProvider : IGetRecordsmanNameProvider
    {
        public string GetRecordsmanName(GameType gameType, int recordTime)
        {
            var recordWindow = new NewRecordWindow(gameType, recordTime);
            if (recordWindow.ShowDialog() == true)
                return recordWindow.WinnerName;
            else
                return string.Empty;
        }
    }
}
