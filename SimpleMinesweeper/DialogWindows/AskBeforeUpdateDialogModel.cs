using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinesweeper.DialogWindows
{
    public class AskBeforeUpdateDialogModel : BasePrettyDialogModel
    {
        public AskBeforeUpdateDialogModel(Version version) : base(PrettyDialogType.YesNo)
        {
            Caption = "Доступно обновление";
            Message = $"Доступна версия {version.ToString()}. Желаете установить обновление прямо сейчас?\n(Приложение будет закрыто).";
            ImageSource = @"pack://application:,,,/Icons/basket.png";
        }
    }
}
