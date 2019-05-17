using System;
using System.Windows;
using System.Windows.Input;
using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeper.View;

using System.Windows.Controls;


namespace SimpleMinesweeper.ViewModel
{
    public class GameViewModel
    {
        #region Fields
        private MainWindow mainWindow;
        private MinesweeperPage currentPage;

        private readonly MinesweeperPage gamePage;
        private readonly MinesweeperPage recordsPage;
        private readonly MinesweeperPage settingsPage;
        #endregion

        #region Properties

        public IGame Game { get; private set; }

        #endregion

        #region Constructor

        public GameViewModel(IGame game, MainWindow mainWindow)
        {
            gamePage = new GamePage(); // Контекст установим свой собственный, в конструкторе.
            settingsPage = new SettingsPage { DataContext = this };
            recordsPage = new RecordsPage { DataContext = this };

            Game = game;
            this.mainWindow = mainWindow;
            this.mainWindow.DataContext = this;
            MenuSetGameTypeCommand = new MenuSetGameTypeCommand(this);
            MenuOpenSettingsCommand = new MenuOpenSettingsCommand(this);
            LoadPage(gamePage);
        }

        #endregion

        #region Commands
        public MenuSetGameTypeCommand MenuSetGameTypeCommand { get; }
        public MenuOpenSettingsCommand MenuOpenSettingsCommand { get; }
        #endregion

        #region Navigation and commands logic
        public void SetGameType(GameType type)
        {
            Game.Settings.SelectGameType(type);
            if (currentPage.PageType != MinesweeperPageType.Game)
                LoadPage(gamePage);
        }

        public void LoadSettingsPage()
        {
            LoadPage(settingsPage);
        }

        private void LoadPage(MinesweeperPage page)
        {
            mainWindow.WorkArea.Content = page;
            currentPage = page;
        }
        #endregion

    }

    #region Command types
    public class MenuSetGameTypeCommand : ICommand
    {
        #region Fields
        private readonly GameViewModel game;
        #endregion

        #region Constructors

        public MenuSetGameTypeCommand(GameViewModel vm)
        {
            game = vm;
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
            game.SetGameType(gameType);
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
