using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Data;
using SimpleMinesweeper.Core;
using System.Globalization;

namespace SimpleMinesweeper.ViewModel
{
    class OpenCellCommand : ICommand
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

    class SetFlagCellCommand : ICommand
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
    
    class CellViewModel : INotifyPropertyChanged
    {
        ICell modelCell;
        OpenCellCommand openCommand;
        SetFlagCellCommand setFlagCommand;

        public CellViewModel(ICell cell)
        {
            modelCell = cell;
            modelCell.OnStateChanged += ModelCell_OnStateChanged;
            modelCell.OnMinedChanged += ModelCell_OnMinedChanged;

            openCommand = new OpenCellCommand(modelCell);
            setFlagCommand = new SetFlagCellCommand(modelCell);
        }

        public OpenCellCommand OpenCellCommand => openCommand;
        public SetFlagCellCommand SetFlagCellCommand => setFlagCommand; 

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
}
