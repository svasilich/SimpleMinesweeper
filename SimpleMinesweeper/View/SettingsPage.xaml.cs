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

namespace SimpleMinesweeper.View
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : MinesweeperPage
    {

        public SettingsPage() : base()
        {
            InitializeComponent();
            TextBlock b = new TextBlock();
            b.Visibility = Visibility.Visible;
        }

        public override MinesweeperPageType PageType => MinesweeperPageType.Settings;

        private void TextBoxWithValidation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out int result);
        }
    }
}
