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

            IMinefield minefield = new Minefield(new CellFactory(), new RandomMinePositionGenerator());
            viewModel = new MinefieldViewModel(minefield, ViewElement);
            DataContext = viewModel;
            ViewElement.SizeChanged += viewModel.MainWindow_SizeChanged;

            SettingsItem settings = new SettingsItem()
            {
                Height = 16,
                Width = 30,
                MineCount = 99
            };
            minefield.SetGameSettings(settings);
            minefield.Fill();
        }
    }
}
