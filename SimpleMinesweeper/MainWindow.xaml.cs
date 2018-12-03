using System;
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
using SimpleMinesweeper.ViewModel;

namespace SimpleMinesweeper
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameManager GameManager;

        public MainWindow()
        {
            InitializeComponent();

            GameManager = new GameManager();
            WorkArea.Source = new Uri(@"View\GamePage.xaml", UriKind.Relative);
        }
    }
}
