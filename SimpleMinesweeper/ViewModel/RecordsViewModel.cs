using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameRecords;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Input;
using System.Windows;

namespace SimpleMinesweeper.ViewModel
{
    public class RecordsViewModel : INotifyPropertyChanged
    {
        #region Fields

        private GameViewModel gameViewModel;
        private IRecords records;

        private string newbieName;
        private int? newbieTime;

        private string advancedName;
        private int? advancedTime;

        private string professionalName;
        private int? professionalTime;
        #endregion

        #region Properties
        public CleareRecordsCommand CleareRecordsCommand { get; }
        public CloseRecordsCommand CloseRecordsCommand { get; }

        public string NewbieName
        {
            get => newbieName;
            set
            {
                newbieName = PrepareNameValue(value);
                NotifyPropertyChanged();
            }
        }

        public int? NewbieTime {
            get => newbieTime;
            set
            {
                newbieTime = value;
                NotifyPropertyChanged();
            }
        }

        public string AdvancedName
        {
            get => advancedName;
            set
            {
                advancedName = PrepareNameValue(value);
                NotifyPropertyChanged();
            }
        }

        public int? AdvancedTime
        {
            get => advancedTime;
            set
            {
                advancedTime = value;
                NotifyPropertyChanged();
            }
        }

        public string ProfessionalName
        {
            get => professionalName;
            set
            {
                professionalName = PrepareNameValue(value);
                NotifyPropertyChanged();
            }
        }

        public int? ProfessionalTime
        {
            get => professionalTime;
            set
            {
                professionalTime = value;
                NotifyPropertyChanged();
            }
        }

        private string PrepareNameValue(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "-";
            return name;
        }
        #endregion

        #region Constructor

        public RecordsViewModel(GameViewModel gameViewModel)
        {
            this.gameViewModel = gameViewModel;
            records = gameViewModel.Game.Records;
            UpdateRecords();            

            records.OnRecordChanged += Records_OnRecordChanged;

            CleareRecordsCommand = new CleareRecordsCommand(this);
            CloseRecordsCommand = new CloseRecordsCommand(this);
        }

        #endregion

        #region public methods

        public void ClearRecords()
        {
            records.Clear();
        }

        public void CloseRecordsPage()
        {
            gameViewModel.LoadCurrentGamePage();
        }

        #endregion

        #region Event handlers

        private void Records_OnRecordChanged(object sender, EventArgs e)
        {           
            UpdateRecords();
        }

        #endregion

        #region Update records logic

        private void UpdateRecords()
        {
            var recordsList = records.GetRecords();
            IRecordItem ri = recordsList.FirstOrDefault(x => x.GameType == GameType.Newbie);
            NewbieName = ri?.Player ?? "Вася";
            NewbieTime = ri?.Time ?? 11;

            ri = recordsList.FirstOrDefault(x => x.GameType == GameType.Advanced);
            AdvancedName = ri?.Player ?? "Петя";
            AdvancedTime = ri?.Time ?? 66;

            ri = recordsList.FirstOrDefault(x => x.GameType == GameType.Professional);
            ProfessionalName = ri?.Player ?? "Коля";
            ProfessionalTime = ri?.Time ?? 999;
        }

        #endregion

        #region INotifyPropertyChanged    
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }

    public class RecordTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? seconds = (int?)value;

            if (seconds.HasValue)
                return seconds.ToString();

            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CleareRecordsCommand : ICommand
    {
        private RecordsViewModel rvm;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CleareRecordsCommand(RecordsViewModel recordsViewModel)
        {
            rvm = recordsViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var result = MessageBox.Show("Будет очищена таблица рекордов. Продолжить?",
                "Рекорды", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.OK)
                rvm.ClearRecords();
        }
    }

    public class CloseRecordsCommand : ICommand
    {
        private RecordsViewModel rvm;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CloseRecordsCommand(RecordsViewModel recordsViewModel)
        {
            rvm = recordsViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            rvm.CloseRecordsPage();
        }
    }

}
