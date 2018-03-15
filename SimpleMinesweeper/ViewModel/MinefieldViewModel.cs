using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeper.ViewModel
{
    class MinefieldViewModel : INotifyPropertyChanged
    {
        IMinefield field;
        private ReloadFieldCommand reloadCommand;

        private int width = 10;
        private int height = 10;
        private int mineCount = 10;

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

        public MinefieldViewModel(IMinefield minefield)
        {
            field = minefield;
            field.OnStateChanged += Field_OnStateChanged;
            field.OnFilled += Field_OnFilled;
            reloadCommand = new ReloadFieldCommand(field, width, height, mineCount);

            cells = new ObservableCollection<List<CellViewModel>>();
        }

        private void Field_OnFilled(object sender, EventArgs e)
        {
            ReloadCells();
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
    }
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
