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
