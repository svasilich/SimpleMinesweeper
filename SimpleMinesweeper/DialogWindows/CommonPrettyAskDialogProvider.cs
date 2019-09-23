using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleMinesweeper.DialogWindows
{
    public static class CommonPrettyAskDialogProvider
    {
        public static bool Ask(IPrettyDialogWindowModel dialogModel)
        {
            var dialogWindow = new PrettyDialogWindow();
            var prettyDialogWindowViewModel = new PrettyDialogWindowViewModel(dialogWindow, dialogModel);
            dialogWindow.DataContext = prettyDialogWindowViewModel;
            dialogWindow.ShowDialog();
            return prettyDialogWindowViewModel.DialogResult == dialogModel.AcceptAnswer;
        }
    }
}
