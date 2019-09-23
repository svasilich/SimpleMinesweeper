using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleMinesweeper.DialogWindows
{
    public class PrettyGameViewModelDialogProvider : IGameViewModelDialogProvider
    {
        public bool AskUserBeforeQuit()
        {
            return CommonPrettyAskDialogProvider.Ask(new AskUserBeforeQuitDialogModel());
        }
    }
}
