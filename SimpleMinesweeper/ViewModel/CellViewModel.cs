﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;
using SimpleMinesweeper.Core;
using System.Globalization;

namespace SimpleMinesweeper.ViewModel
{    
    public class CellViewModel : INotifyPropertyChanged
    {
        ICell modelCell;

        public CellViewModel(ICell cell)
        {
            modelCell = cell;
            modelCell.OnStateChanged += ModelCell_OnStateChanged;
            modelCell.OnMinedChanged += ModelCell_OnMinedChanged;

            OpenCellCommand = new OpenCellCommand(modelCell);
            SetFlagCellCommand = new SetFlagCellCommand(modelCell);
        }

        public OpenCellCommand OpenCellCommand { get; }
        public SetFlagCellCommand SetFlagCellCommand { get; }

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

        #region Поддержка INotifyPropertyChanged 

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }

    public class OpenCellCommand : ICommand
    {
        private ICell cell;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            cell.Open();
        }

        public OpenCellCommand(ICell cell)
        {
            this.cell = cell;
        }
    }

    public class SetFlagCellCommand : ICommand
    {
        ICell cell;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public SetFlagCellCommand(ICell cell)
        {
            this.cell = cell;
        }

        public bool CanExecute(object parameter)
        {
            return cell.State == CellState.Flagged ||
                cell.State == CellState.NotOpened;
        }

        public void Execute(object parameter)
        {
            cell.SetFlag();
        }
    }

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
}
