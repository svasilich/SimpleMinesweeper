namespace SimpleMinesweeper.DialogWindows
{
    public class SimpleDialogProviderFactory : IDialogProviderFactory
    {
        #region Public methods
        
        public IGameViewModelDialogProvider GetGameViewModelDialogProvider()
        {
            return new SimpleGameViewModelDialogProvider();
        }

        public IRecordViewModelDialogProvider GetRecordViewModelDialogProvider()
        {
            return new SimpleRecordViewModelDialogProvider();
        }

        public IUpdateDialogProvider GetUpdateDialogProvider()
        {
            return null;
        }

        #endregion
    }
}
