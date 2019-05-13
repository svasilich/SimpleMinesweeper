using System;
using System.Windows;
using System.Windows.Input;
using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameSettings;


namespace SimpleMinesweeper.ViewModel
{
    public class GameViewModel
    {
        #region Fields
        #endregion

        #region Properties

        public IGame Game { get; private set; }

        #endregion

        #region Constructor

        public GameViewModel(IGame game)
        {
            Game = game;
            MenuSetGameTypeCommand = new MenuSetGameTypeCommand(Game.Settings);
        }

        #endregion

        #region Commands
        public MenuSetGameTypeCommand MenuSetGameTypeCommand { get; }
        #endregion
        
    }

    #region Command types
    public class MenuSetGameTypeCommand : ICommand
    {
        #region Fields
        private readonly ISettingsManager settings;
        #endregion

        #region Constructors

        public MenuSetGameTypeCommand(ISettingsManager settingsManager)
        {
            settings = settingsManager;
        }

        #endregion

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!(parameter is GameType))
            {
                MessageBox.Show("Wrong command parameter!!");
                return;
            }

            GameType gameType = (GameType)parameter;
            settings.SelectGameType(gameType);
        }
    }
    #endregion
}
