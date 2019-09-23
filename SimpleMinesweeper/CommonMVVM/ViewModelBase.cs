using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleMinesweeper.CommonMVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Logic
        protected void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion

    }
}
