using System.Windows.Controls;

namespace SimpleMinesweeper.View
{
    public abstract class MinesweeperPage : Page
    {
        #region Properties

        public abstract MinesweeperPageType PageType { get; }

        #endregion

    }
}
