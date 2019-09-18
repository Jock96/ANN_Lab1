namespace FC.UI.ViewModels
{
    using FC.UI.Commands;
    using System.Collections.Generic;

    /// <summary>
    /// Вью-модель окна.
    /// </summary>
    public class MainWindowVM : BaseVM
    {
        /// <summary>
        /// Вью-модель окна.
        /// </summary>
        public MainWindowVM()
        {
            LoadDataCommand = new LoadDataCommand();
            TrainNetworkCommand = new TrainNetworkCommand();
            LoadWeightsCommand = new LoadWeightsCommand();
            SaveWeightsCommand = new SaveWeightsCommand();
            ForeCastCommand = new ForeCastCommand();
        }


        /// <summary>
        /// Данные для графика.
        /// </summary>
        private List<double> _chartData;

        /// <summary>
        /// Данные для графика.
        /// </summary>
        public List<double> ChartData
        {
            get => _chartData;
            set
            {
                _chartData = value;
                OnPropertyChanged();
            }
        }

        #region Команды.

        /// <summary>
        /// Загрузить данные.
        /// </summary>
        public LoadDataCommand LoadDataCommand { get; set; }

        /// <summary>
        /// Обучить сеть.
        /// </summary>
        public TrainNetworkCommand TrainNetworkCommand { get; set; }
        
        /// <summary>
        /// Загрузить веса.
        /// </summary>
        public LoadWeightsCommand LoadWeightsCommand { get; set; }

        /// <summary>
        /// Сохранить веса.
        /// </summary>
        public SaveWeightsCommand SaveWeightsCommand { get; set; }

        /// <summary>
        /// Предсказать
        /// </summary>
        public ForeCastCommand ForeCastCommand { get; set; }

        #endregion
    }
}
