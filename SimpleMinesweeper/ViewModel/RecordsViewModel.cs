using System;
using System.Linq;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using SimpleMinesweeper.Core;
using SimpleMinesweeper.Core.GameRecords;
using SimpleMinesweeper.CommonMVVM;

namespace SimpleMinesweeper.ViewModel
{
    public class RecordsViewModel : ViewModelBase
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

            CleareRecordsCommand = new RelayCommand(CleareRecordsExecute);
            CloseRecordsCommand = new RelayCommand(CloseRecordsCommandExecute);
        }

        #endregion

        #region Commands
        public RelayCommand CleareRecordsCommand { get; }
        public RelayCommand CloseRecordsCommand { get; }

        private void CleareRecordsExecute(object parameter)
        {
            var result = MessageBox.Show("Будет очищена таблица рекордов. Продолжить?",
                "Рекорды", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.OK)
                ClearRecords();
        }

        private void ClearRecords()
        {
            records.Clear();
            records.Save();
        }

        private void CloseRecordsCommandExecute(object parameter)
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
            NewbieName = ri?.Player ?? "Аноним";
            NewbieTime = ri?.Time ?? 0;

            ri = recordsList.FirstOrDefault(x => x.GameType == GameType.Advanced);
            AdvancedName = ri?.Player ?? "Аноним";
            AdvancedTime = ri?.Time ?? 0;

            ri = recordsList.FirstOrDefault(x => x.GameType == GameType.Professional);
            ProfessionalName = ri?.Player ?? "Аноним";
            ProfessionalTime = ri?.Time ?? 0;
        }

        #endregion
    }

    #region Converter types
        
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

    #endregion
}
