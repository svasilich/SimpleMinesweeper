using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleMinesweeper.DialogWindows
{
    public class PrettyRecordViewModelDialogProvider : IRecordViewModelDialogProvider
    {
        public bool AskUserBeforeCleareRecord()
        {
            return CommonPrettyAskDialogProvider.Ask(new CleareRecordDialogModel());
        }
    }
}
