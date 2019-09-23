using System;
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeper.Core.GameRecords;

namespace SimpleMinesweeper.Core
{
    public interface IGame
    {
        IMinefield GameField { get; }

        ISettingsManager Settings { get; }

        IRecords Records { get; }

        IGameTimer Timer { get; }

        event EventHandler OnRecord;
    }
}
