namespace SimpleMinesweeper.DialogWindows
{
    public interface IDialogProviderFactory
    {
        IRecordViewModelDialogProvider GetRecordViewModelDialogProvider();
        IGameViewModelDialogProvider GetGameViewModelDialogProvider();

        IUpdateDialogProvider GetUpdateDialogProvider();
    }
}
