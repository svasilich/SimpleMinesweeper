namespace SimpleMinesweeper.DialogWindows
{
    // Объединяет методы для общения с пользователем, необходимые для GameViewModel.
    public interface IGameViewModelDialogProvider
    {
        bool AskUserBeforeQuit();
    }
}
