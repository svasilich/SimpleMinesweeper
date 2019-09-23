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

namespace SimpleMinesweeper.View
{
    /// <summary>
    /// Логика взаимодействия для MineFieldView.xaml
    /// </summary>
    public partial class MineFieldView : UserControl, IDynamicGameFieldSize
    {
        #region Properties

        public double ContainerHeight => FieldRow.ActualHeight;

        public double ContainetWidth => FieldColumn.ActualWidth;

        #endregion

        #region Constructor

        public MineFieldView()
        {
            InitializeComponent();
        }

        #endregion        
    }
}
