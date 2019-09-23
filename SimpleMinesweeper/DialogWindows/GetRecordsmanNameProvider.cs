using SimpleMinesweeper.Core;

namespace SimpleMinesweeper.DialogWindows
{
    public class GetRecordsmanNameProvider : IGetRecordsmanNameProvider
    {

        #region Public methods
        
        public string GetRecordsmanName(GameType gameType, int recordTime)
        {
            var recordWindow = new NewRecordWindow(gameType, recordTime);
            if (recordWindow.ShowDialog() == true)
                return recordWindow.WinnerName;
            else
                return string.Empty;
        }

        #endregion
    }
}
