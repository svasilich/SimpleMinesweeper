﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SimpleMinesweeper.CommonMVVM;
using Newtonsoft.Json;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using SimpleMinesweeper.Core.Updater;

namespace SimpleMinesweeper.ViewModel
{
    class AboutViewModel : ViewModelBase
    {

        #region Fields
        private string version;
        #endregion

        #region Constructor
        public AboutViewModel()
        {
            CheckUpdateCommand = new RelayCommand(o => CheckUpdate());

            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        #endregion

        #region Properties

        public string Version {
            get => version;
            set
            {
                version = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public RelayCommand CheckUpdateCommand { get; }

        #endregion

        #region Commands logic

        private void CheckUpdate()
        {
            Updater.CheckVersionAndUpdateProgram(true);
        }

        #endregion



    }
}
