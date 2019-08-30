using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeper.View;
using System.Windows.Controls;

namespace SimpleMinesweeper.ViewModel
{
    public class GameViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Fields
        private MainWindow mainWindow;
        private MinesweeperPage currentPage;

        private readonly MinesweeperPage gamePage;
        private readonly MinesweeperPage recordsPage;
        private readonly MinesweeperPage settingsPage;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public IGame Game { get; private set; }

        #region Custom game settings interface
        // Этот набор полей нужен для успешной валидации вводимых данных.
        private int customWidth;
        private int custopmHeight;
        private int customMineCount;

        public void UpdatePageValuesFromApp()
        {
            CustomWidth = Game.Settings.GetItemByType(GameType.Custom).Width;
            CustomHeight = Game.Settings.GetItemByType(GameType.Custom).Height;
            CustomMineCount = Game.Settings.GetItemByType(GameType.Custom).MineCount;
        }

        
        public int CustomWidth
        {
            get
            {
                return customWidth;
            }
            set
            {
                customWidth = value;
                NotifyPropertyChanged();
            }
        }
        public int CustomHeight
        {
            get
            {
                return custopmHeight;
            }
            set
            {
                custopmHeight = value;
                NotifyPropertyChanged();
            }
        }
        public int CustomMineCount
        {
            get
            {
                return customMineCount;
            }
            set
            {
                customMineCount = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        public GameType GameType
        {
            get => Game.Settings.CurrentSettings.Type;
        }

        public void UpdateCustomSettings()
        {
            Game.Settings.SetCustomSize(CustomHeight, CustomWidth, CustomMineCount);
            Game.Settings.Save(Properties.Resources.settingsPath);
        }

        #endregion

        #region Constructor

        public GameViewModel(IGame game, MainWindow mainWindow)
        {
            Game = game;
            game.OnRecord += Game_OnRecord;

            gamePage = new GamePage(); // Контекст установим свой собственный, в конструкторе.
            settingsPage = new SettingsPage { DataContext = this };
            recordsPage = new RecordsPage { DataContext = new RecordsViewModel(this) };
            
            this.mainWindow = mainWindow;
            this.mainWindow.DataContext = this;
            UpdatePageValuesFromApp();

            Game.Settings.OnCurrentGameChanged += Settings_OnCurrentGameChanged;
            Game.Settings.OnCustomSizeChanged += Settings_OnCustomSizeChanged;

            MenuSetGameTypeCommand = new MenuSetGameTypeCommand(this);
            MenuOpenSettingsCommand = new MenuOpenSettingsCommand(this);
            MenuOpenRecordsCommand = new MenuOpenRecordsCommand(this);

            LoadPage(gamePage);
        }

        private void Game_OnRecord(object sender, EventArgs e)
        {
            LoadRecordsPage();
        }

        private void Settings_OnCustomSizeChanged(object sender, EventArgs e)
        {
            UpdatePageValuesFromApp();
        }

        private void Settings_OnCurrentGameChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged(nameof(GameType));
        }

        #endregion        

        #region IDataErorrInfo

        private string validationError;
        public string Error
        {
            get { return validationError; }
            set
            {
                validationError = value;
                NotifyPropertyChanged();
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "CustomWidth":
                    case "CustomHeight":
                    case "CustomMineCount":
                        { 
                            if (!SettingsHelper.CheckValidity(CustomHeight, CustomWidth, CustomMineCount, out string err))
                                return Error = err;
                            break;
                        }
                }
                return Error = string.Empty;
            }
        }

        private void SettingsTextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            Error = e.Error.ErrorContent.ToString();
        }
        #endregion

        #region Commands
        public MenuSetGameTypeCommand MenuSetGameTypeCommand { get; }
        public MenuOpenSettingsCommand MenuOpenSettingsCommand { get; }
        public MenuOpenRecordsCommand MenuOpenRecordsCommand { get; }
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

        public void LoadRecordsPage()
        {
            LoadPage(recordsPage);
        }

        internal void LoadCurrentGamePage()
        {
            LoadPage(gamePage);
        }

        private void LoadPage(MinesweeperPage page)
        {
            mainWindow.WorkArea.Content = page;
            currentPage = page;
            UpdatePageValuesFromApp();
        }
        #endregion

        #region NotifiProperty
        private void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
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
                game.UpdateCustomSettings();                
                game.SetGameType(si.Type);
            }
            else
            {
                MessageBox.Show("Wrong command parameter!!");
                return;
            }
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

    public class MenuOpenRecordsCommand : ICommand
    {
        private GameViewModel owner;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public MenuOpenRecordsCommand(GameViewModel owner)
        {
            this.owner = owner;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            owner.LoadRecordsPage();
        }
    }
    #endregion

    #region Converter types
    public class CustomSettingsCheckboxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GameType currentGameType = (GameType)value;
            return currentGameType == GameType.Custom;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CustomSettingsEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool customGameCheckboxValue = (bool)values[0];
            if (!customGameCheckboxValue)
                return false;

            string errText = values[1].ToString();
            return string.IsNullOrEmpty(errText);
        }        

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
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

    public class ValidErrorMessageVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string errText = (string)value;
            if (string.IsNullOrWhiteSpace(errText))
                return Visibility.Hidden;
            else
                return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
