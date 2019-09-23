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

        event EventHandler OnMessageChanged;
        event EventHandler OnCaptionChanged;
        event EventHandler OnImageSourceChanged;
    }

    public enum PrettyDialogType
    {
        YesNo,
        OkCancel
    }
}
