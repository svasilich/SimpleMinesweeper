using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeper.Core.GameRecords;
using SimpleMinesweeper.DialogWindows;


namespace SimpleMinesweeper.Core
{
    public class Game : IGame
    {
        #region Members
        private static IGame gameInstance;
        #endregion

        #region Fields
        public IMinefield GameField { get; private set; }
        public ISettingsManager Settings { get; private set; }

        public IGameTimer Timer { get; private set; }

        public IRecords Records { get; private set; }
        #endregion

        #region Constructors
        protected Game(ISettingsManager settings, IRecords records, IMinefield gameField)
        {
            Settings = settings;
            Settings.OnCurrentGameChanged += Settings_OnCurrentGameChanged;

            Records = records;

            Timer = new GameTimer();

            GameField = gameField;
            GameField.SetGameSettings(Settings.CurrentSettings);
            GameField.Fill();
            GameField.OnStateChanged += GameField_OnStateChanged;

            
        }        
        #endregion

        #region Events

        protected virtual void Settings_OnCurrentGameChanged(object sender, EventArgs e)
        {
            GameField.SetGameSettings(Settings.CurrentSettings);
            GameField.Fill();
            Settings.Save(Properties.Resources.settingsPath);
        }

        private void GameField_OnStateChanged(object sender, EventArgs e)
        {            
            switch (GameField.State)
            {
                case FieldState.InGame:
                    Timer.Start();
                    break;

                case FieldState.NotStarted:
                    Timer.Reset();
                    break;

                case FieldState.Win:
                    Timer.Stop();
                    if (Records.IsRecord(Settings.CurrentSettings.Type, Timer.Seconds))
                        HandleRecord(Settings.CurrentSettings.Type, Timer.Seconds);
                    else
                        MessageBox.Show("Не рекород!");

                    break;

                default:
                    Timer.Stop();
                    break;
            }
        }

        private void HandleRecord(GameType gameType, int winnerTime)
        {
            var recordWindow = new NewRecordWindow(Settings.CurrentSettings.Type, winnerTime);            

            if (recordWindow.ShowDialog() == true)
                Records.UpdateRecord(gameType, winnerTime, recordWindow.WinnerName);
        }
        #endregion

        #region Get default instance logic (static)
        public static IGame GetInstance()
        {
            return gameInstance ?? CreateInstance();
        }

        private static IGame CreateInstance()
        {
            var settings = new SettingsManager();
            settings.Load(AppContext.BaseDirectory + Properties.Resources.settingsPath);

            var records = new Records();
            records.Load(AppContext.BaseDirectory + Properties.Resources.recordsPath);

            return gameInstance = 
                new Game(settings,
                    records,
                    new Minefield(new CellFactory(), 
                    new RandomMinePositionGenerator())); ;
        }
        #endregion

    }
}
