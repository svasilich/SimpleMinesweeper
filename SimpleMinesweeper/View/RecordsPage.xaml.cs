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

namespace SimpleMinesweeper.View
{
    /// <summary>
    /// Логика взаимодействия для RecordsPage.xaml
    /// </summary>
    public partial class RecordsPage : MinesweeperPage
    {
        #region Properties

        public override MinesweeperPageType PageType => MinesweeperPageType.Records;

        #endregion

        #region Constructor

        public RecordsPage()
        {
            InitializeComponent();
        }

        #endregion
    }
}
