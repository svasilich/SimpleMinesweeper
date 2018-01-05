using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleMinesweeper.Core;

namespace SimpleMinesweeper
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
        }

        private void Field_OnStateChanged(object sender, EventArgs e)
        {
            State = field.State;   
        }

        // Релизация обозреваемой коллекции клеток.
        // Реализация ReloadCells

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
