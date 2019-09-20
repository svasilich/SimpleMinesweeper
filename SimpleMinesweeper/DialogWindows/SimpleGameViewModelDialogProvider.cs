using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleMinesweeper.DialogWindows
{
    public class SimpleGameViewModelDialogProvider : IGameViewModelDialogProvider
    {
        public bool AskUserBeforeQuit()
        {
            return MessageBox.Show("OK to close?", "Are you sure?",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
    }
}
