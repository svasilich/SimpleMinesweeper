using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SimpleMinesweeper.View
{
    public enum MinesweeperPageType
    {
        Game,
        Settings,
        Records,
    }

    public abstract class MinesweeperPage : Page
    {
        public abstract MinesweeperPageType PageType { get; }
    }

    
}
