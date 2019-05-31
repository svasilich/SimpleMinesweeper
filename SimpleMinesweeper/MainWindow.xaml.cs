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
    public partial class MainWindow : Window, IDynamicGameFieldSize
    {
        private MinefieldViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();


            IMinefield minefield = new Minefield(new CellFactory(), new RandomMinePositionGenerator());
            viewModel = new MinefieldViewModel(minefield, this);
            DataContext = viewModel;
            this.SizeChanged += viewModel.MainWindow_SizeChanged;

            minefield.Fill(16, 30, 99);   
        }

        public MenuCommand MenuCommand
        {
            get
            {
                return viewModel.MenuCommand;
            }
        }

        public double ContainerHeight => MainView.FieldRow.ActualHeight;

        public double ContainetWidth => MainView.FieldColumn.ActualWidth;

        public Window MainGameWindow
        {
            get { return this; }
        }
    }
}
