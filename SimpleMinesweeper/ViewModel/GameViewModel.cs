using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;
using System.Globalization;

using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeper.View;


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
            if (parameter is GameType)
            {
                GameType gameType = (GameType)parameter;
                game.SetGameType(gameType);
            }
            else if (parameter is SettingsItem)
            {
                // На самом деле здесь всегда будет тип игры Custom.
                var si = (SettingsItem)parameter;
                game.Game.Settings.SetCustomSize(si.Height, si.Width, si.MineCount);
                game.SetGameType(si.Type);
            }
            else
            {
                MessageBox.Show("Wrong command parameter!!");
                return;
            }
        }

        #region Set game type logic
        private void SetStandardType(GameType type)
        {
            
        }
        #endregion
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

    #region Converter types
    public class CustomSettingsEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GameType currentGameType = (GameType)value;
            return currentGameType == GameType.Custom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isEnable = (bool)value;
            if (isEnable)
                return GameType.Custom;
            else
                return GameType.Newbie;
        }
    }

    public class CustomGameTypeCommandConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            SettingsItem settings = new SettingsItem();
            settings.Type = GameType.Custom;
            if (int.TryParse(values[0].ToString(), out int width))
                settings.Width = width;

            if (int.TryParse(values[1].ToString(), out int height))
                settings.Height = height;

            if (int.TryParse(values[2].ToString(), out int minesCount))
                settings.MineCount = minesCount;

            return settings;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
