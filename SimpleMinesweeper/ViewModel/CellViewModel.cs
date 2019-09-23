using System;
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

        public bool GameIsOver
        {
            get
            {
                if (modelCell == null)
                    return false;
                else
                    return modelCell.Owner.State == FieldState.GameOver;
            }
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

        #endregion

        #region Commands logic

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
            NotifyPropertyChanged(nameof(GameIsOver));
        }

        #endregion
    }    
}
