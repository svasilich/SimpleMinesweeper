using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

using SimpleMinesweeper.Core;
using SimpleMinesweeper.CommonMVVM;

namespace SimpleMinesweeper.ViewModel
{    
    public class CellViewModel : ViewModelBase
    {
        #region Fields

        ICell modelCell;

        #endregion

        #region Properties
        public CellState State
        {
            get => modelCell.State;
        }

        public bool Mined
        {
            get => modelCell.Mined;
        }

        public bool ShowNearby
        {
            get => modelCell.State == CellState.Opened && !modelCell.Mined;
        }

        public int MinesNearby
        {
            get => modelCell.MinesNearby;
            set => modelCell.MinesNearby = value;
        }

        #endregion

        #region Constructor

        public CellViewModel(ICell cell)
        {
            modelCell = cell;
            modelCell.OnStateChanged += ModelCell_OnStateChanged;
            modelCell.OnMinedChanged += ModelCell_OnMinedChanged;

            OpenCellCommand = new RelayCommand(o => modelCell.Open());
            SetFlagCellCommand = new RelayCommand(o => modelCell.SetFlag(), CanSetFlagCommandExecute);
        }

        #endregion

        #region Commands
        public RelayCommand OpenCellCommand { get; }

        public RelayCommand SetFlagCellCommand { get; }


        private bool CanSetFlagCommandExecute(object parameter)
        {
            return modelCell.State == CellState.Flagged ||
                modelCell.State == CellState.NotOpened;
        }
        #endregion

        #region Event handlers
        private void ModelCell_OnMinedChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged(nameof(Mined));
            NotifyPropertyChanged(nameof(ShowNearby));
        }

        private void ModelCell_OnStateChanged(object sender, EventArgs e)
        {
            NotifyPropertyChanged(nameof(State));
            NotifyPropertyChanged(nameof(ShowNearby));
        }
        #endregion
    }    

    #region Converter types
    public class NearbyColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int nearby = (int)value;

            Color textColor;
            switch (nearby)
            {
                case 1:
                    textColor = Colors.Blue;
                    break;
                case 2:
                    textColor =  Colors.Green;
                    break;
                case 3:
                    textColor =  Colors.Red;
                    break;
                case 4:
                    textColor =  Colors.DarkBlue;
                    break;
                case 5:
                    textColor =  Colors.Brown;
                    break;
                case 6:
                    textColor = Color.FromRgb(255, 128, 0);
                    break;
                case 7:
                    textColor =  Colors.Black;
                    break;
                case 8:
                    textColor =  Colors.DarkGray;
                    break;
                default:
                    textColor =  Colors.White;
                    break;
            }

            return new SolidColorBrush(textColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }

    public class NearbyTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int nearby = (int)value;

            if (nearby > 0 && nearby <= 8)
                return nearby;

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
