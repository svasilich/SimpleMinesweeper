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
        MainWindow mainWindow;
        #endregion

        #region Properties

        public IGame Game { get; private set; }

        #endregion

        #region Constructor

        public GameViewModel(IGame game, MainWindow mainWindow)
        {
            Game = game;
            this.mainWindow = mainWindow;
            this.mainWindow.DataContext = this;
            MenuSetGameTypeCommand = new MenuSetGameTypeCommand(Game.Settings);
            MenuOpenSettingsCommand = new MenuOpenSettingsCommand(this);
        }

        #endregion

        #region Commands
        public MenuSetGameTypeCommand MenuSetGameTypeCommand { get; }
        public MenuOpenSettingsCommand MenuOpenSettingsCommand { get; }
        #endregion

        #region Load page logic
        public void LoadGamePage()
        {
            mainWindow.WorkArea.Source = new Uri(@"View\GamePage.xaml", UriKind.Relative);
        }

        public void LoadSettingsPage()
        {
            mainWindow.WorkArea.Source = new Uri(@"View\SettingsPage.xaml", UriKind.Relative);
        }
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

    public class MenuOpenSettingsCommand : ICommand
    {
        private GameViewModel owner;

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
            owner.LoadSettingsPage();
        }

        public MenuOpenSettingsCommand(GameViewModel owner)
        {
            this.owner = owner;
        }
    }
    #endregion
}
