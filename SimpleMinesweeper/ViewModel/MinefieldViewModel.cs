using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using SimpleMinesweeper.Core;
using SimpleMinesweeper.CommonMVVM;

namespace SimpleMinesweeper.ViewModel
{
    public class MinefieldViewModel : ViewModelBase
    {
        #region Fields

        protected IGame game;
        private IDynamicGameFieldSize fieldContainer;
        private FieldState state;

        private ObservableCollection<List<CellViewModel>> cells;

        private double fieldHeightPx;
        private double fieldWidthPx;
        private int minesLeft;
        private int seconds;        
        
        #endregion

        #region Properties

        public FieldState State
        {
            get { return state; }
            set
            {
                state = value;
                NotifyPropertyChanged();
            }
        }

        public int GameTime
        {
            get
            {
                return seconds;
            }
            private set
            {
                seconds = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<List<CellViewModel>> Cells
        {
            get => cells;
            private set
            {
                cells = value;
                NotifyPropertyChanged();
            }
        }

        public double FieldHeightPx
        {
            get { return fieldHeightPx; }
            set
            {
                fieldHeightPx = value;
                NotifyPropertyChanged();
            }
        }

        public double FieldWidthPx
        {
            get { return fieldWidthPx; }
            set
            {
                fieldWidthPx = value;
                NotifyPropertyChanged();
            }
        }

        public int MinesLeft
        {
            get { return minesLeft; }
            set
            {
                minesLeft = value;
                NotifyPropertyChanged();
            }
        }
        
        #endregion
        
        #region Constructor

        public MinefieldViewModel(IGame game, IDynamicGameFieldSize fieldContainer)
        {
            this.game = game;
            this.game.GameField.OnStateChanged += Field_OnStateChanged;
            this.game.GameField.OnFilled += Field_OnFilled;
            this.game.GameField.OnFlagsCountChanged += Field_OnFlagsCountChanged;
            ReloadCommand = new RelayCommand(o => game.GameField.Fill());
            cells = new ObservableCollection<List<CellViewModel>>();

            this.fieldContainer = fieldContainer;

            this.game.Timer.OnTimerTick += GameTimer_OnTimerTick;
            
            ReloadCells();
        }

        #endregion

        #region Commands

        public RelayCommand ReloadCommand { get; }

        #endregion

        #region Event handlers

        protected void GameTimer_OnTimerTick(object sender, EventArgs e)
        {
            GameTime = game.Timer.Seconds;
        }

        private void Field_OnFlagsCountChanged(object sender, EventArgs e)
        {
            MinesLeft = game.GameField.MinesCount - game.GameField.FlagsCount;
        }

        private void Field_OnStateChanged(object sender, EventArgs e)
        {
            State = game.GameField.State;
        }

        private void Field_OnFilled(object sender, EventArgs e)
        {
            ReloadCells();
            ResizeField(fieldContainer.ContainetWidth, fieldContainer.ContainerHeight);
        }

        public void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var view = (IDynamicGameFieldSize)sender;
            ResizeField(view.ContainetWidth, view.ContainerHeight);
        }

        #endregion

        #region Cels logic

        private void ReloadCells()
        {            
            Cells.Clear();

            var modelCells = game.GameField.Cells;
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

            MinesLeft = game.GameField.MinesCount;
        }

        #endregion

        #region Resizing

        protected void ResizeField(double containerWidth, double containerHeight)
        {
            double newWidth = 0;
            double newHeight = 0;

            if (containerWidth > containerHeight)
                ScaleSide(containerWidth, containerHeight, game.GameField.Width, game.GameField.Height, out newWidth, out newHeight);
            else
                ScaleSide(containerHeight, containerWidth, game.GameField.Height, game.GameField.Width, out newHeight, out newWidth);

            FieldWidthPx = newWidth;
            FieldHeightPx = newHeight;
        }

        private void ScaleSide(double containerSideAMax, double containerSideBMin, int fieldCellSideA, int fieldCellSideB,
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

        #endregion  
    }
}
