using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SimpleMinesweeper.CommonMVVM;

namespace SimpleMinesweeper.DialogWindows
{
    class PrettyDialogWindowViewModel : ViewModelBase
    {
        private Window dialogWindow;
        private IPrettyDialogWindowModel model;
        private MessageBoxResult acceptResult;
        private MessageBoxResult cancelResult;

        #region Properties
        public string WindowCaption
        { get => model.Caption;
          set => NotifyPropertyChanged();
        }

        public string Message {
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
            //ImageSource = @"pack://application:,,,/Icons/message_exit_1.jpg";
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

        #endregion

        #region Event handlers

        private void Model_OnImageSourceChanged()
        {
            ImageSource = model.ImageSource;
        }

        private void Model_OnMessageChanged()
        {
            Message = model.Message;
        }

        private void Model_OnCaptionChanged()
        {
            WindowCaption = model.Caption;
        }

        #endregion
    }
}
