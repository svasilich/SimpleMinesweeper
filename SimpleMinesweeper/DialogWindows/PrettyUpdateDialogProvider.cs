using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.DialogWindows
{
    public class PrettyUpdateDialogProvider : IUpdateDialogProvider
    {
        public bool AskBeforeUpdate(Version newVersion)
        {
            return CommonPrettyAskDialogProvider.Ask(new AskBeforeUpdateDialogModel(newVersion));
        }
    }
}
