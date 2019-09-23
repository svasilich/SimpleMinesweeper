using System;
using System.Windows;

namespace SimpleMinesweeper.DialogWindows
{
    public class BasePrettyDialogModel : IPrettyDialogWindowModel
    {
        #region Fields

        private string caption;
        private string message;
        private string imageSource;        

        #endregion

        #region Properties

        public string Caption
        {
            get => caption;
            set
            {
                caption = value;
                OnCaptionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnMessageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public string ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                OnImageSourceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public MessageBox DialogResult { get; set; }

        public MessageBoxResult AcceptAnswer
        {
            get
            {
                switch(DialogType)
                {
                    case PrettyDialogType.OkCancel:
                        return MessageBoxResult.OK;

                    case PrettyDialogType.YesNo:
                        return MessageBoxResult.Yes;

                    default:
                        throw new Exception($"Указан недопустимый тип DialogType: {DialogType}");
                }
            }
        }

        public MessageBoxResult CancelAnswer
        {
            get
            {
                switch (DialogType)
                {
                    case PrettyDialogType.OkCancel:
                        return MessageBoxResult.Cancel;

                    case PrettyDialogType.YesNo:
                        return MessageBoxResult.No;

                    default:
                        throw new Exception($"Указан недопустимый тип DialogType: {DialogType}");
                }
            }
        }

        public PrettyDialogType DialogType { get; }

        #endregion

        #region Constructor

        protected BasePrettyDialogModel(PrettyDialogType dialogType)
        {
            DialogType = dialogType;
        }

        #endregion

        #region Events

        public event EventHandler OnMessageChanged;

        public event EventHandler OnCaptionChanged;

        public event EventHandler OnImageSourceChanged;

        #endregion
    }
}
