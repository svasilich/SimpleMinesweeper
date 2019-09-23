using System;
using System.Windows.Input;

namespace SimpleMinesweeper.CommonMVVM
{
    public class RelayCommand : ICommand
    {
        #region Fields

        private Action<object> execute;

        private Func<object, bool> canExecute;

        #endregion

        #region Constructor

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region Public methods

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke(parameter);
        }

        #endregion

        #region Events

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion   
    }
}
