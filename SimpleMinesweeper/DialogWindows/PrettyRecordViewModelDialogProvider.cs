namespace SimpleMinesweeper.DialogWindows
{
    public class PrettyRecordViewModelDialogProvider : IRecordViewModelDialogProvider
    {
        #region Public methods

        public bool AskUserBeforeCleareRecord()
        {
            return CommonPrettyAskDialogProvider.Ask(new CleareRecordDialogModel());
        }

        #endregion
    }
}
