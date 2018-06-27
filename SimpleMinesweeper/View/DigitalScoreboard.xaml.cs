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
    /// Логика взаимодействия для DigitalScoreboard.xaml
    /// </summary>
    public partial class DigitalScoreboard : UserControl
    {
        public DigitalScoreboard()
        {
            InitializeComponent();
        }
        
        public string ScoreboardText
        {
            get { return (string)GetValue(ScoreboardTextProperty); }
            set { SetValue(ScoreboardTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScoreboardText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScoreboardTextProperty =
            DependencyProperty.Register("ScoreboardText", typeof(string), typeof(DigitalScoreboard), 
                new PropertyMetadata(string.Empty, new PropertyChangedCallback(ScoreboardTextChanged)));
        
        private static void ScoreboardTextChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs args)
        {
            DigitalScoreboard board = (DigitalScoreboard)dependency;
            TextBlock content = board.ContentTextBlock;
            content.Text = args.NewValue.ToString();
        }
    }
}
