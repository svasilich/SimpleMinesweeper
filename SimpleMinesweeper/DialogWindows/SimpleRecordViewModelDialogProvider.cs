using System.Windows;

namespace SimpleMinesweeper.DialogWindows
{
    public class SimpleRecordViewModelDialogProvider : IRecordViewModelDialogProvider
    {
        #region Public methods

        public bool AskUserBeforeCleareRecord()
        {
            return MessageBox.Show("Будет очищена таблица рекордов. Продолжить?",
                "Рекорды", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK;
        }

        #endregion
    }
}
