using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeper.CommonMVVM;
using SimpleMinesweeper.View;
using SimpleMinesweeper.DialogWindows;

namespace SimpleMinesweeper.ViewModel
{
    public class GameViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Fields

        private MainWindow mainWindow;
        private MinesweeperPage currentPage;

        private readonly MinesweeperPage gamePage;
        private readonly MinesweeperPage recordsPage;
        private readonly MinesweeperPage settingsPage;
        private readonly MinesweeperPage aboutPage;

        private readonly IGameViewModelDialogProvider dialogProvider;

        // Этот набор полей нужен для успешной валидации вводимых данных.
        private int customWidth;
        private int custopmHeight;
        private int customMineCount;

        #endregion

        #region Properties

        public IGame Game { get; private set; }

        public GameType GameType
        {
            get => Game.Settings.CurrentSettings.Type;
        }

        public void UpdateCustomSettings()
        {
            Game.Settings.SetCustomSize(CustomHeight, CustomWidth, CustomMineCount);
            Game.Settings.Save(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Properties.Resources.settingsPath);
        }

        #region Custom game settings interface

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
        
        #endregion

        #region Constructor

        public GameViewModel(IGame game, MainWindow mainWindow, IDialogProviderFactory dialogProviderFactory)
        {
            Game = game;
            game.OnRecord += Game_OnRecord;

            gamePage = new GamePage(); // Контекст установим свой собственный, в конструкторе.
            settingsPage = new SettingsPage { DataContext = this };
            recordsPage = new RecordsPage { DataContext = new RecordsViewModel(this, dialogProviderFactory.GetRecordViewModelDialogProvider()) };
            aboutPage = new AboutPage { DataContext = new AboutViewModel() };   

            dialogProvider = dialogProviderFactory.GetGameViewModelDialogProvider();
            
            this.mainWindow = mainWindow;
            this.mainWindow.DataContext = this;
            UpdatePageValuesFromApp();

            Game.Settings.OnCurrentGameChanged += Settings_OnCurrentGameChanged;
            Game.Settings.OnCustomSizeChanged += Settings_OnCustomSizeChanged;

            MenuSetGameTypeCommand = new RelayCommand(SetGameTypeExecute);
            MenuOpenSettingsCommand = new RelayCommand(o => LoadPage(settingsPage));
            MenuOpenRecordsCommand = new RelayCommand(o => LoadPage(recordsPage));
            MenuOpenAboutCommand = new RelayCommand(o => LoadPage(aboutPage));

            // Все данные уже сохранены. Action для закрытия не нужен.
            ClosingCommand = new RelayCommand(null, o => dialogProvider.AskUserBeforeQuit());
            MenuExitCommand = new RelayCommand(o => Exit());
            LoadPage(gamePage);
        }
        
        #endregion

        #region Event handlers

        private void Game_OnRecord(object sender, EventArgs e)
        {
            MenuOpenRecordsCommand.Execute(null);
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

        public RelayCommand MenuSetGameTypeCommand { get; }

        public RelayCommand MenuOpenSettingsCommand { get; }

        public RelayCommand MenuOpenRecordsCommand { get; }

        public RelayCommand MenuOpenAboutCommand { get; }

        public RelayCommand MenuExitCommand { get; }

        public RelayCommand ClosingCommand { get; }

        #endregion

        #region Navigation and commands logic

        private void SetGameTypeExecute(object parameter)
        {
            if (parameter is GameType)
            {
                GameType gameType = (GameType)parameter;
                SetGameType(gameType);
            }
            else if (parameter is SettingsItem)
            {
                // На самом деле здесь всегда будет тип игры Custom.
                var settingsItem = (SettingsItem)parameter;
                UpdateCustomSettings();
                SetGameType(settingsItem.Type);
            }
            else
            {
                MessageBox.Show("Wrong command parameter!!");
                return;
            }
        }

        private void SetGameType(GameType type)
        {
            Game.Settings.SelectGameType(type);
            if (currentPage.PageType != MinesweeperPageType.Game)
                LoadPage(gamePage);            
        }
    
        public void LoadCurrentGamePage()
        {
            LoadPage(gamePage);
        }

        private void LoadPage(MinesweeperPage page)
        {
            mainWindow.WorkArea.Content = page;
            currentPage = page;
            UpdatePageValuesFromApp();
        }

        private void Exit()
        {
            mainWindow.Close();
            //Environment.Exit(0);
            //Application.Current.MainWindow.Close();            
        }

        #endregion
        
    }
}
