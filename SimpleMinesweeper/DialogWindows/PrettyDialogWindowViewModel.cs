using System;
using System.Windows;
using SimpleMinesweeper.CommonMVVM;

namespace SimpleMinesweeper.DialogWindows
{
    class PrettyDialogWindowViewModel : ViewModelBase
    {
        #region Fields

        private Window dialogWindow;
        private IPrettyDialogWindowModel model;
        private MessageBoxResult acceptResult;
        private MessageBoxResult cancelResult;

        #endregion

        #region Properties

        public string WindowCaption
        {
            get => model.Caption;
            set => NotifyPropertyChanged();
        }

        public string Message
        {
            get => model.Message;
            set => NotifyPropertyChanged();
        }

        public string ImageSource
        {
            get => model.ImageSource;
            set => NotifyPropertyChanged();
        }

        public string ButtonAccepText { get; }

        public string ButtonCancelText { get; }        

        public MessageBoxResult DialogResult { get; private set; }

        #endregion        

        #region Constructor

        public PrettyDialogWindowViewModel(Window dialogWindow, IPrettyDialogWindowModel model)
        {
            this.model = model;
            WindowCaption = model.Caption;
            Message = model.Message;
            ImageSource = model.ImageSource;

            model.OnCaptionChanged += Model_OnCaptionChanged;
            model.OnMessageChanged += Model_OnMessageChanged;
            model.OnImageSourceChanged += Model_OnImageSourceChanged;

            switch (model.DialogType)
            {
                case PrettyDialogType.OkCancel:
                    ButtonAccepText = "Ок";
                    ButtonCancelText = "Отмена";
                    break;

                case PrettyDialogType.YesNo:
                    ButtonAccepText = "Да";
                    ButtonCancelText = "Нет";
                    break;
            }

            acceptResult = model.AcceptAnswer;
            cancelResult = model.CancelAnswer;
            ButtonAcceptCommand = new RelayCommand(AcceptCommandExecute);
            ButtonCancelCommand = new RelayCommand(CancelCommandExecute);
            this.dialogWindow = dialogWindow;
        }

        #endregion

        #region Commands

        public RelayCommand ButtonAcceptCommand { get; }

        public RelayCommand ButtonCancelCommand { get; }

        #endregion

        #region Commans logic

        private void AcceptCommandExecute(object param)
        {
            DialogResult = acceptResult;
            dialogWindow.Close();
        }

        private void CancelCommandExecute(object param)
        {
            DialogResult = cancelResult;
            dialogWindow.Close();
        }

        #endregion
        
        #region Event handlers

        private void Model_OnImageSourceChanged(object sender, EventArgs e)
        {
            ImageSource = model.ImageSource;
        }

        private void Model_OnMessageChanged(object sender, EventArgs e)
        {
            Message = model.Message;
        }

        private void Model_OnCaptionChanged(object sender, EventArgs e)
        {
            WindowCaption = model.Caption;
        }

        #endregion
    }
}
