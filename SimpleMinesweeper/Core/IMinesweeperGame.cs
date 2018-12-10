using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMinesweeper.Core.GameSettings;


namespace SimpleMinesweeper.Core
{
    public interface IMinesweeperGame
    {
        IMinefield Minefield { get; }

        SettingsManager Settings { get; }
    }
}
