namespace SimpleMinesweeper.DialogWindows
{
    public class PrettyDialogProviderFactory : IDialogProviderFactory
    {
        #region Public methods

        public IGameViewModelDialogProvider GetGameViewModelDialogProvider()
        {
            return new PrettyGameViewModelDialogProvider();
        }

        public IRecordViewModelDialogProvider GetRecordViewModelDialogProvider()
        {
            return new PrettyRecordViewModelDialogProvider();
        }

        #endregion

    }
}
