using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeper.ViewModel
{
    class MinefieldViewModel : INotifyPropertyChanged
    {
        IMinefield field;

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

            cells = new List<List<CellViewModel>>();
        }

        private void Field_OnFilled(object sender, EventArgs e)
        {
            ReloadCells();
        }

        private void Field_OnStateChanged(object sender, EventArgs e)
        {
            State = field.State;   
        }

        private List<List<CellViewModel>> cells;
        public List<List<CellViewModel>> Cells
        {
            get => cells;
            private set
            {
                cells = value;
                NotifyPropertyChanged();
            }
        }
        
        private void ReloadCells()
        {
            cells.Clear();

            var modelCells = field.Cells;
            foreach (var row in modelCells)
            {
                List<CellViewModel> list = new List<CellViewModel>();
                cells.Add(list);
                foreach (var cell in row)
                {
                    CellViewModel cvm = new CellViewModel(cell);
                    list.Add(cvm);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
