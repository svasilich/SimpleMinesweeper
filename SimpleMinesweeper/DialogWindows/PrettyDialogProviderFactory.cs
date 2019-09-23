using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.DialogWindows
{
    public class PrettyDialogProviderFactory : IDialogProviderFactory
    {
        public IGameViewModelDialogProvider GetGameViewModelDialogProvider()
        {
            return new PrettyGameViewModelDialogProvider();
        }

        public IRecordViewModelDialogProvider GetRecordViewModelDialogProvider()
        {
            return new PrettyRecordViewModelDialogProvider();
        }
    }
}
