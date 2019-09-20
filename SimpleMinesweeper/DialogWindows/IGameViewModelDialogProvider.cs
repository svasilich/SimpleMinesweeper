using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.DialogWindows
{
    // Объединяет методы для общения с пользователем, необходимые для GameViewModel.
    public interface IGameViewModelDialogProvider
    {
        bool AskUserBeforeQuit();
    }
}
