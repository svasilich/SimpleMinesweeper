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
using SimpleMinesweeper.Core;

namespace SimpleMinesweeper.ViewModel
{
    public class MinefieldViewModel : INotifyPropertyChanged
    {
        private IMinefield field;
        private IDynamicGameFieldSize gameWindow; 
        private ReloadFieldCommand reloadCommand;

        //TODO: Эти параметры должны уйти в класс настроек.
        private int width = 16;
        private int height = 30;
        private int mineCount = 99;

        private FieldState state;
        public FieldState State
        {
            get { return state; }
            set
            {
                state = value;
                NotifyPropertyChanged();
            }
        }

        public MinefieldViewModel(IMinefield minefield, IDynamicGameFieldSize gameWindow)
        {
            field = minefield;
            field.OnStateChanged += Field_OnStateChanged;
            field.OnFilled += Field_OnFilled;
            reloadCommand = new ReloadFieldCommand(field, width, height, mineCount);

            cells = new ObservableCollection<List<CellViewModel>>();

            this.gameWindow = gameWindow;
        }

        private void Field_OnFilled(object sender, EventArgs e)
        {
            ReloadCells();
            ResizeField(gameWindow.ContainetWidth, gameWindow.ContainerHeight);
        }

        private void Field_OnStateChanged(object sender, EventArgs e)
        {
            State = field.State;
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

        public ReloadFieldCommand ReloadCommand => reloadCommand;

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


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

        public void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var view = (IDynamicGameFieldSize)sender;
            ResizeField(view.ContainetWidth, view.ContainerHeight);
        }

        protected void ResizeField(double containerWidth, double containerHeight)
        {
            double newWidth = 0;
            double newHeight = 0;

            // Определить наибольшую сторону.
            if (containerWidth > containerHeight)
            {
                if (field.Length > field.Height)
                {
                    newWidth = containerWidth;
                    newHeight = containerWidth * field.Height / field.Length;

                    if (newHeight > containerHeight)
                    {
                        newWidth = containerHeight * field.Length / field.Height;
                        newHeight = containerHeight;
                    }
                }
                else
                {
                    newWidth = containerHeight * field.Length / field.Height;
                    newHeight = containerHeight;
                }
            }
            else
            {
                if (field.Height > field.Length)
                {
                    newWidth = containerHeight * field.Length / field.Height;
                    newHeight = containerHeight;

                    if (newWidth > containerWidth)
                    {
                        newWidth = containerWidth;
                        newHeight = containerWidth * field.Height / field.Length;
                    }
                }
                else
                {
                    newWidth = containerWidth;
                    newHeight = containerWidth * field.Height / field.Length;
                }
            }

            FieldWidthPx = newWidth;
            FieldHeightPx = newHeight;
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
