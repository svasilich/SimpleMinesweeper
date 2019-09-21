using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleMinesweeper.DialogWindows
{
    public class SimpleRecordViewModelDialogProvider : IRecordViewModelDialogProvider
    {
        public bool AskUserBeforeCleareRecord()
        {
            return MessageBox.Show("Будет очищена таблица рекордов. Продолжить?",
                "Рекорды", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK;
        }
    }
}
