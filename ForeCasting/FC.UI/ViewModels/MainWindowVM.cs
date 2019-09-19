namespace FC.UI.ViewModels
{
    using FC.Core.Models;
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

            var configuration = new ConfigurationModel();

            _epochCount = configuration.EpochCount.ToString();
            _epsilon = configuration.Epsilon.ToString();
            _alpha = configuration.Alpha.ToString();
        }

        #region Настройки нейронной сети.

        /// <summary>
        /// Момент.
        /// </summary>
        private string _alpha;

        /// <summary>
        /// Момент.
        /// </summary>
        public string Alpha
        {
            get => _alpha;
            set
            {
                _alpha = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Скорость обучения.
        /// </summary>
        private string _epsilon;

        /// <summary>
        /// Скорость обучения.
        /// </summary>
        public string Epsilon
        {
            get => _epsilon;
            set
            {
                _epsilon = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Количество эпох.
        /// </summary>
        private string _epochCount;

        /// <summary>
        /// Количество эпох.
        /// </summary>
        public string EpochCount
        {
            get => _epochCount;
            set
            {
                _epochCount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Количество нейронов скрытого слоя.
        /// </summary>
        public List<string> CountOfHiddenLayerNeuronsList { get; } = new List<string> { "30", "10", "0" };

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

        #endregion

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
