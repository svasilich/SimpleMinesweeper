using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleMinesweeper.DialogWindows
{
    public class AskUserBeforeQuitDialogModel : BasePrettyDialogModel
    {
        #region Fields
        private static Random random = new Random();
        #endregion

        #region Constructor
        public AskUserBeforeQuitDialogModel() : base(PrettyDialogType.YesNo)
        {
            Caption = "Завершение игры";
            GenerateContent();
        }
        #endregion

        #region Business logic
        public void GenerateContent()
        {
            int type = random.Next(3);

            switch (type)
            {
                case 0:
                    Message = "Зачем вам эта работа? Вы можете сыграть ещё пару раз!";
                    ImageSource = @"pack://application:,,,/Icons/message_exit_1.jpg";
                    break;
                case 1:
                    Message = "Уже уходите... А как же мы? Вы уверены?";
                    ImageSource = @"pack://application:,,,/Icons/message_exit_2.jpg";
                    break;
                case 2:
                    Message = "Уже уходите? Ну.. раз вы больше ничего не хотите...";
                    ImageSource = @"pack://application:,,,/Icons/message_exit_3.png";
                    break;
            }
        }
        #endregion
    }
}
