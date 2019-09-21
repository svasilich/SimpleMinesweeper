using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.DialogWindows
{
    public interface IDialogProviderFactory
    {
        IRecordViewModelDialogProvider GetRecordViewModelDialogProvider();
        IGameViewModelDialogProvider GetGameViewModelDialogProvider();
    }
}
