
namespace SimpleMinesweeper.Core.GameRecords
{
    public interface IRecordItem
    {
        GameType GameType { get; }

        int Time { get; }

        string Player { get; }
    }
}
