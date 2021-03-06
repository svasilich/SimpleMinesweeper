﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.Updater;
using SimpleMinesweeper.ViewModel;
using SimpleMinesweeper.DialogWindows;


namespace SimpleMinesweeper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameViewModel gameView;

        public MainWindow()
        {
            InitializeComponent();

            IDialogProviderFactory factory = new PrettyDialogProviderFactory();
            Updater.SetDialogProvider(factory.GetUpdateDialogProvider());
            IGame game = Game.GetInstance();            
            gameView = new GameViewModel(game, this, factory);

            Task.Run(() => Updater.CheckVersionAndUpdateProgram());
        }
    }
}
