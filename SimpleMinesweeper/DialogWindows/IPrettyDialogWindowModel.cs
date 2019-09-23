using System;
using System.Windows;

namespace SimpleMinesweeper.DialogWindows
{
    public interface IPrettyDialogWindowModel
    {
        string Caption { get; }
        string Message { get; }
        string ImageSource { get; }
        MessageBox DialogResult { get; set; }
        PrettyDialogType DialogType { get; }

        MessageBoxResult AcceptAnswer { get; }
        MessageBoxResult CancelAnswer { get; }

        event Action OnMessageChanged;
        event Action OnCaptionChanged;
        event Action OnImageSourceChanged;
    }

    public enum PrettyDialogType
    {
        YesNo,
        OkCancel
    }
}
