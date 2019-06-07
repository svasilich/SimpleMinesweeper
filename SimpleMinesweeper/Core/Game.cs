﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMinesweeper.Core.GameSettings;

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
        #endregion

        #region Constructors
        protected Game(ISettingsManager settings, IMinefield gameField)
        {
            Settings = settings;
            Settings.OnCurrentGameChanged += Settings_OnCurrentGameChanged;
            GameField = gameField;
            GameField.SetGameSettings(Settings.CurrentSettings);
            GameField.Fill();
        }
        #endregion

        #region Events

        protected virtual void Settings_OnCurrentGameChanged(object sender, EventArgs e)
        {
            GameField.SetGameSettings(Settings.CurrentSettings);
            GameField.Fill();
            Settings.Save(Properties.Resources.settingsPath);
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
            settings.Load(Properties.Resources.settingsPath);

            return gameInstance = 
                new Game(settings,
                    new Minefield(new CellFactory(), 
                    new RandomMinePositionGenerator())); ;
        }
        #endregion

    }
}