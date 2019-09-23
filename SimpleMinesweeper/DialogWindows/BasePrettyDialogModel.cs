using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #region Propertirs

        public string Caption
        {
            get => caption;
            set
            {
                caption = value;
                OnCaptionChanged?.Invoke();
            }
        }

        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnMessageChanged?.Invoke();
            }
        }

        public string ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                OnImageSourceChanged?.Invoke();
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

        public event Action OnMessageChanged;
        public event Action OnCaptionChanged;
        public event Action OnImageSourceChanged;
    }
}
