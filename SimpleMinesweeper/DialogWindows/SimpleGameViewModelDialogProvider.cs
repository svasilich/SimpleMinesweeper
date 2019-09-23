using System.Windows;

namespace SimpleMinesweeper.DialogWindows
{
    public class SimpleGameViewModelDialogProvider : IGameViewModelDialogProvider
    {
        #region Public methods
        
        public bool AskUserBeforeQuit()
        {
            return MessageBox.Show("OK to close?", "Are you sure?",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        #endregion
    }
}
