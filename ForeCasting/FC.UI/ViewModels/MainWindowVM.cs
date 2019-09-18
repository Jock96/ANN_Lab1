namespace FC.UI.ViewModels
{
    using FC.UI.Commands;
    using LiveCharts;
    using LiveCharts.Wpf;
    using System.Collections.Generic;
    using System.Linq;

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

            _countOfHiddenLayerNeurons = CountOfHiddenLayerNeuronsList.First();
        }

        /// <summary>
        /// Количество нейронов скрытого слоя.
        /// </summary>
        public List<string> CountOfHiddenLayerNeuronsList { get; } = new List<string> { "0", "10", "30" };

        /// <summary>
        /// Количество нейронов скрытого слоя.
        /// </summary>
        private string _countOfHiddenLayerNeurons;

        /// <summary>
        /// Количество нейронов скрытого слоя.
        /// </summary>
        public string CountOfHiddenLayerNeurons
        {
            get => _countOfHiddenLayerNeurons;
            set
            {
                _countOfHiddenLayerNeurons = value;
                OnPropertyChanged();
            }
        }

        #region Данные графика.

        /// <summary>
        /// Линия графика.
        /// </summary>
        private ChartValues<double> _lineValues;

        /// <summary>
        /// Линия графика.
        /// </summary>
        public ChartValues<double> LineValues
        {
            get => _lineValues;
            set
            {
                _lineValues = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Нормализованные данные.
        /// </summary>
        private List<double> _normilizedData = new List<double>();

        /// <summary>
        /// Нормализованные данные.
        /// </summary>
        public List<double> NormilizedData
        {
            get => _normilizedData;
            set
            {
                _normilizedData = value;
                OnPropertyChanged();
            }
        }

        #endregion

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
