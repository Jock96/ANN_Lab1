﻿namespace FC.UI.ViewModels
{
    using FC.Core.Models;
    using FC.UI.Commands;
    using LiveCharts;
    using LiveCharts.Wpf;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

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
            ForeCastCommand = new ForeCastCommand();

            _percentLabelVisibility = Visibility.Hidden;

            _countOfHiddenLayerNeurons = CountOfHiddenLayerNeuronsList.First();

            var configuration = new ConfigurationModel();

            _epochCount = configuration.EpochCount.ToString();
            _epsilon = configuration.Epsilon.ToString();
            _alpha = configuration.Alpha.ToString();

            _maxValue = 1;
            _minValue = 0;
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
        /// Минимальное значение графика.
        /// </summary>
        private double _minValue;

        /// <summary>
        /// Минимальное значение графика.
        /// </summary>
        public double MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Максимальное значение графика.
        /// </summary>
        private double _maxValue;

        /// <summary>
        /// Максимальное значение графика.
        /// </summary>
        public double MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Линии графика.
        /// </summary>
        public SeriesCollection _lines;

        /// <summary>
        /// Линии графика.
        /// </summary>
        public SeriesCollection Lines
        {
            get => _lines;
            set
            {
                _lines = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Флаг прогноза.
        /// </summary>
        private Visibility _percentLabelVisibility;

        /// <summary>
        /// Флаг прогноза.
        /// </summary>
        public Visibility PercentLabelVisibility
        {
            get => _percentLabelVisibility;
            set
            {
                _percentLabelVisibility = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Строковое представление процента ошибки.
        /// </summary>
        private string _errorByPercentString;

        /// <summary>
        /// Строковое представление процента ошибки.
        /// </summary>
        public string ErrorByPercentString
        {
            get => _errorByPercentString;
            set
            {
                _errorByPercentString = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Данные.
        /// </summary>
        private List<double> _normilizedData = new List<double>();

        /// <summary>
        /// Данные.
        /// </summary>
        public List<double> Data
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
        /// Предсказать
        /// </summary>
        public ForeCastCommand ForeCastCommand { get; set; }

        #endregion
    }
}
