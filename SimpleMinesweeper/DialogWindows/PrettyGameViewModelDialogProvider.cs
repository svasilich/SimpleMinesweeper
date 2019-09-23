namespace SimpleMinesweeper.DialogWindows
{
    public class PrettyGameViewModelDialogProvider : IGameViewModelDialogProvider
    {
        #region Public methods

        public bool AskUserBeforeQuit()
        {
            return CommonPrettyAskDialogProvider.Ask(new AskUserBeforeQuitDialogModel());
        }

        #endregion
    }
}
