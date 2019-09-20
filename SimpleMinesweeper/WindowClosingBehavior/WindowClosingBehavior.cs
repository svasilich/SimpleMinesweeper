using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;

namespace SimpleMinesweeper.WindowClosingBehavior
{
    // Автор Nish Nishant,
    // https://www.codeproject.com/Articles/73251/Handling-a-Window-s-Closed-and-Closing-events-in-t

    public class WindowClosingBehavior
    {

        // Событие генерируется непосредственно перед закрытием окна. Закрытие окна уже нельзя отменить.
        #region Closed        
        public static ICommand GetClosed(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ClosedProperty);
        }

        public static void SetClosed(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ClosedProperty, value);
        }

        public static readonly DependencyProperty ClosedProperty = 
            DependencyProperty.RegisterAttached(
                "Closed",
                typeof(ICommand),
                typeof(WindowClosingBehavior),
                new UIPropertyMetadata(new PropertyChangedCallback(ClosedChanged)));

        private static void ClosedChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Window window = target as Window;
            if (window == null)
                return;

            if (e.NewValue != null)
                window.Closed += Window_Closed;
            else
                window.Closed -= Window_Closed;
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            ICommand closed = GetClosed(sender as Window);
            closed?.Execute(null);
        }

        #endregion

        // Событие генерируется после вызова метода окна Close().
        // С помощью CanExecute события можно отменить закрытие окна.
        // Execute вызывается, если CanExecute вернул true.
        #region Closing
        public static ICommand GetClosing(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(ClosingProperty);
        }

        public static void SetClosing(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ClosingProperty, value);
        }

        public static readonly DependencyProperty ClosingProperty = 
            DependencyProperty.RegisterAttached(
                "Closing",
                typeof(ICommand),
                typeof(WindowClosingBehavior),
                new UIPropertyMetadata(new PropertyChangedCallback(ClosingChanged)));

        private static void ClosingChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Window window = target as Window;
            if (window == null)
                return;

            if (e.NewValue != null)
                window.Closing += Window_Closing;
            else
                window.Closing -= Window_Closing;
        }

        private static void Window_Closing(object sender, CancelEventArgs e)
        {
            ICommand closing = GetClosing(sender as Window);
            bool? canExecute = closing?.CanExecute(null);
            if (canExecute.HasValue && canExecute.Value)
                closing?.Execute(null);
            else
            {
                ICommand cancelClosing = GetCancelClosing(sender as Window);
                cancelClosing?.Execute(null);
                e.Cancel = true;
            }
        }

        // Событие вызывается, если произошла отмена закрытия окна (Closing.CanExecute вернул false).
        #region Cancel Closing
        
        public static ICommand GetCancelClosing(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CancelClosingPropety);
        }

        public static void SetCancelClosing(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CancelClosingPropety, value);
        }

        public static readonly DependencyProperty CancelClosingPropety =
            DependencyProperty.RegisterAttached(
                "CancelClosing",
                typeof(ICommand),
                typeof(WindowClosingBehavior));
        #endregion

        #endregion
    }
}
