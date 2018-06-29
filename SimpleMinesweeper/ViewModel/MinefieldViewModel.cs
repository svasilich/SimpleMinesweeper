using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using SimpleMinesweeper.Core;
using System.Globalization;

namespace SimpleMinesweeper.ViewModel
{
    public class MinefieldViewModel : INotifyPropertyChanged
    {
        protected IMinefield field;
        private IDynamicGameFieldSize gameWindow;

        //TODO: Эти параметры должны уйти в класс настроек.
        private int width = 30;
        private int height = 16;
        private int mineCount = 99;

        private FieldState state;
        public FieldState State
        {
            get { return state; }
            set
            {
                state = value;

                switch (State)
                {
                    case FieldState.InGame:
                        gameTimer.Start();
                        break;

                    case FieldState.NotStarted:
                        gameTimer.Reset();
                        break;

                    default:
                        gameTimer.Stop();
                        break;
                }

                NotifyPropertyChanged();
            }
        }

        private void GameTimer_OnTimerTick(object sender, EventArgs e)
        {
            GameTime = gameTimer.Seconds;
        }

        protected IGameTimer gameTimer;

        public int GameTime
        {
            get
            {
                return gameTimer.Seconds;
            }
            private set
            {
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<List<CellViewModel>> cells;
        public ObservableCollection<List<CellViewModel>> Cells
        {
            get => cells;
            private set
            {
                cells = value;
                NotifyPropertyChanged();
            }
        }

        public ReloadFieldCommand ReloadCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private double fieldHeightPx;
        public double FieldHeightPx
        {
            get { return fieldHeightPx; }
            set
            {
                fieldHeightPx = value;
                NotifyPropertyChanged();
            }
        }

        private double fieldWidthPx;
        public double FieldWidthPx
        {
            get { return fieldWidthPx; }
            set
            {
                fieldWidthPx = value;
                NotifyPropertyChanged();
            }
        }

        private int minesLeft;
        public int MinesLeft
        {
            get { return minesLeft; }
            set
            {
                minesLeft = value;
                NotifyPropertyChanged();
            }
        }

        public MinefieldViewModel(IMinefield minefield, IDynamicGameFieldSize gameWindow)
        {
            field = minefield;
            field.OnStateChanged += Field_OnStateChanged;
            field.OnFilled += Field_OnFilled;
            field.OnFlagsCountChanged += Field_OnFlagsCountChanged;
            ReloadCommand = new ReloadFieldCommand(field, width, height, mineCount);

            cells = new ObservableCollection<List<CellViewModel>>();

            this.gameWindow = gameWindow;

            gameTimer = new GameTimer();
            gameTimer.OnTimerTick += GameTimer_OnTimerTick;

            MinesLeft = mineCount;
        }

        private void Field_OnFlagsCountChanged(object sender, EventArgs e)
        {
            MinesLeft = field.MinesCount - field.FlagsCount;
        }

        private void Field_OnStateChanged(object sender, EventArgs e)
        {
            State = field.State;
        }

        private void Field_OnFilled(object sender, EventArgs e)
        {
            ReloadCells();
            ResizeField(gameWindow.ContainetWidth, gameWindow.ContainerHeight);
        }

        private void ReloadCells()
        {
            var cells = Cells;
            cells.Clear();

            var modelCells = field.Cells;
            foreach (var row in modelCells)
            {
                List<CellViewModel> list = new List<CellViewModel>();
                Cells.Add(list);
                foreach (var cell in row)
                {
                    CellViewModel cvm = new CellViewModel(cell);
                    list.Add(cvm);
                }
            }

            Cells = cells;
        }

        private void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var view = (IDynamicGameFieldSize)sender;
            ResizeField(view.ContainetWidth, view.ContainerHeight);
        }

        protected void ResizeField(double containerWidth, double containerHeight)
        {
            double newWidth = 0;
            double newHeight = 0;
            
            if (containerWidth > containerHeight)
                ScaleSide(containerWidth, containerHeight, field.Length, field.Height, out newWidth, out newHeight);
            else
                ScaleSide(containerHeight, containerWidth, field.Height, field.Length, out newHeight, out newWidth);

            FieldWidthPx = newWidth;
            FieldHeightPx = newHeight;
        }

        private void ScaleSide(double containerSideAMax, double containerSideBMin,  int fieldCellSideA, int fieldCellSideB, 
            out double fieldSizePxSideA, out double fieldSizePxB)
        {
            if (fieldCellSideA > fieldCellSideB)
            {
                fieldSizePxSideA = containerSideAMax;
                fieldSizePxB = containerSideAMax * fieldCellSideB / fieldCellSideA;

                if (fieldSizePxB > containerSideBMin)
                {
                    fieldSizePxSideA = containerSideBMin * fieldCellSideA / fieldCellSideB;
                    fieldSizePxB = containerSideBMin;
                }
            }
            else
            {
                fieldSizePxSideA = containerSideBMin * fieldCellSideA / fieldCellSideB; ;
                fieldSizePxB = containerSideBMin;
            }
        }
    }
    
    public interface IDynamicGameFieldSize
    {
        double ContainerHeight { get; }
        double ContainetWidth { get; }
    }

    public class ReloadFieldCommand : ICommand
    {
        private IMinefield field;
        private int width;
        private int height;
        private int mineCount;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ReloadFieldCommand(IMinefield field, int width, int height, int mineCount)
        {
            this.field = field;
            this.width = width;
            this.height = height;
            this.mineCount = mineCount;
        }

        public bool CanExecute(object parameter)
        {
            return true;// field.State != FieldState.NotStarted;
        }

        public void Execute(object parameter)
        {
            field.Fill(height, width, mineCount);
        }
    }

    public class TimerSecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int tiks = (int)value;
            if (tiks >= 60000) // 100 * 60
                tiks = 999 * 60 + 59; // "999:59"

            int seconds = tiks % 60;
            int minutes = tiks / 60;
            string test = string.Format("{0:d2}:{1:d2}", minutes, seconds);
            return string.Format("{0:d2}:{1:d2}", minutes, seconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MinesLeftTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int left = (int)value;
            if (left >= 0)
                return Colors.YellowGreen;

            return Colors.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
