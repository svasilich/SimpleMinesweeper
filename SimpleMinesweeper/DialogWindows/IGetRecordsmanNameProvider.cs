using SimpleMinesweeper.Core;

namespace SimpleMinesweeper.DialogWindows
{
    public interface IGetRecordsmanNameProvider
    {
        string GetRecordsmanName(GameType gameType, int recordTime);
    }
}
