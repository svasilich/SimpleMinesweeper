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
        public MainWindow()
        {
            InitializeComponent();


            IMinefield minefield = new Minefield(new CellFactory(), new RandomMinePositionGenerator());
            MinefieldViewModel minefieldViewModel = new MinefieldViewModel(minefield);
            DataContext = minefieldViewModel;

            minefield.Fill(10, 10, 5);   
        }

        private void Cell_OnOpen(object sender, EventArgs e)
        {
            ICell cell = (ICell)sender;
            if (cell.State == CellState.NotOpened)
                cell.State = CellState.Opened;
            else
                cell.State = CellState.NotOpened;

        }
    }
}
