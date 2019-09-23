using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleMinesweeper.DialogWindows
{
    public class CleareRecordDialogModel : BasePrettyDialogModel
    {
        #region Constructor

        public CleareRecordDialogModel() : base(PrettyDialogType.OkCancel)
        {
            Caption = "Рекорды";
            Message = "Будет очищена таблица рекордов.Продолжить ?";
            ImageSource = @"pack://application:,,,/Icons/basket.png";
        }

#endregion
    }
}
