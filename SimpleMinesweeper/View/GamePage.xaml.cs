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
using SimpleMinesweeper.Core.GameSettings;
using SimpleMinesweeper.ViewModel;

namespace SimpleMinesweeper.View
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private MinefieldViewModel viewModel;

        public GamePage()
        {
            InitializeComponent();

            IGame game = Game.GetInstance();
            viewModel = new MinefieldViewModel(game.GameField, ViewElement);
            DataContext = viewModel;
            ViewElement.SizeChanged += viewModel.MainWindow_SizeChanged;
        }
    }
}
