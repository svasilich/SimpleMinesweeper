using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeper.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для NewRecordWindow.xaml
    /// Здесь я поленился и не стал создавать отдельный класс ViewModel,
    /// так как это окно создано как вспомогательное.
    /// </summary>
    public partial class NewRecordWindow : Window, INotifyPropertyChanged
    {
        #region Fields
        private string winnerName;
        #endregion

        #region Properties

        public string WinnerName
        {
            get => winnerName;
            set
            {
                winnerName = value;
                NotifyPropertyChanged();
            }
        }

        public GameType GameType
        {
            // Поскольку значение типа игры в окне рекордов будет устанавливаться лишь один раз,
            // поддержку INotifyPropertyChanged для этого свойства можно не делать.
            get;
            set;
        }

        public int WinnerTime
        {
            // Как и тип игры, это значение будет устанавливаться лишь единожды.
            // Поэтому поддержку INotifyPropertyChanged здесь тоже не делаю.
            get;
            set;
        }

        #region Commands
        public ICommand OkButtonCommand { get; set; }

        public class OkCommand : ICommand
        {
            private NewRecordWindow window;
            public OkCommand(NewRecordWindow window)
            {
                this.window = window;
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object parameter)
            {
                string winnerName = parameter.ToString();
                return !string.IsNullOrWhiteSpace(winnerName);
            }

            public void Execute(object parameter)
            {
                window.DialogResult = true;
                window.Close();
            }
        }
        #endregion


        #endregion

        public NewRecordWindow(GameType gameType, int winnerTime)
        {
            OkButtonCommand = new OkCommand(this);
            GameType = gameType;
            WinnerTime = winnerTime;
            InitializeComponent();
            DataContext = this;
        }

        #region Поддержка INotifyPropertyChanged 
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion        
    }

    #region Converters
    public class GameTypeNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            GameType gameType = (GameType)value;
            string name = string.Empty;

            switch (gameType)
            {
                case GameType.Newbie:
                    name = "Новичёк";
                    break;
                case GameType.Advanced:
                    name = "Любитель";
                    break;
                case GameType.Professional:
                    name = "Профессионал";
                    break;
                case GameType.Custom:
                    name = "Своя игра";
                    break;
            }

            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
