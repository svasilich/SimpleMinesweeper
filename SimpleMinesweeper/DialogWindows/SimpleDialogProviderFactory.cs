using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.DialogWindows
{
    public class SimpleDialogProviderFactory : IDialogProviderFactory
    {
        public IGameViewModelDialogProvider GetGameViewModelDialogProvider()
        {
            return new SimpleGameViewModelDialogProvider();
        }

        public IRecordViewModelDialogProvider GetRecordViewModelDialogProvider()
        {
            return new SimpleRecordViewModelDialogProvider();
        }
    }
}
