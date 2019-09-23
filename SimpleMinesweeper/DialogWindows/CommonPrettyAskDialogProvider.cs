namespace SimpleMinesweeper.DialogWindows
{
    public static class CommonPrettyAskDialogProvider
    {
        #region Public methods
        
        public static bool Ask(IPrettyDialogWindowModel dialogModel)
        {
            var dialogWindow = new PrettyDialogWindow();
            var prettyDialogWindowViewModel = new PrettyDialogWindowViewModel(dialogWindow, dialogModel);
            dialogWindow.DataContext = prettyDialogWindowViewModel;
            dialogWindow.ShowDialog();
            return prettyDialogWindowViewModel.DialogResult == dialogModel.AcceptAnswer;
        }

        #endregion
    }
}
