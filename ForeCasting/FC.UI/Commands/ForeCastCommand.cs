namespace FC.UI.Commands
{
    using FC.BL.Constants;
    using FC.BL.Helpers;
    using FC.BL.Utils;
    using FC.Core.Utils;
    using FC.UI.ViewModels;
    using LiveCharts.Wpf;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System;

    /// <summary>
    /// Предсказать.
    /// </summary>
    public class ForeCastCommand : BaseTCommand<MainWindowVM>
    {
        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        protected override void Execute(MainWindowVM parameter)
        {
            if (!parameter.Data.Any())
            {
                MessageBox.Show("Отсутвуют данные для прогноза!\n" +
                    "Загрузите данные.");

                return;
            }

            var result = MessageBox.Show("Использовать стандартную директорию весов?",
                "Загрузка весов", MessageBoxButton.YesNo, MessageBoxImage.Question);

            var directory = $"{PathHelper.GetResourcesPath()}{PathConstants.SAVE_PATH}";

            if (result.Equals(MessageBoxResult.No))
            {
                MessageBox.Show("Операция не реализована!");
                return;
            }

            var error = LoadDataUtil.LoadErrorData(directory);

            if (error == null)
                return;

            var errorInPercentString = DataConverterUtil.ToPercentString((double)error);

            parameter.PercentLabelVisibility = Visibility.Visible;
            parameter.ErrorByPercentString = $"Ошибка в предсказании: {errorInPercentString}";

            var layersData = LoadDataUtil.LoadLayersData(directory);

            var startIndex = parameter.Data.Count - TrainingConstants.COUNT_OF_VALUES;
            var preparedData = parameter.Data.GetRange(startIndex, TrainingConstants.COUNT_OF_VALUES);

            var foreCastingUtil = new ForeCastUtil(layersData, preparedData);
            var offset = foreCastingUtil.GetOffset();

            SetMaxOffsetLine(parameter, error, offset);
            SetMinOffsetLine(parameter, error, offset);
            SetOffsetLine(parameter, offset);
        }

        /// <summary>
        /// Основная линия прогноза.
        /// </summary>
        private void SetOffsetLine(MainWindowVM parameter, double offset)
        {
            var lastValue = parameter.Data.Last();
            var minOffsetList = new List<double>();

            minOffsetList.AddRange(parameter.Data);
            minOffsetList.Add(lastValue + offset);

            var lineValues = new LiveCharts.ChartValues<double>(minOffsetList);

            parameter.Lines.Add(new LineSeries()
            {
                Title = "Fore Cast:",
                LineSmoothness = 0,
                Values = lineValues
            });
        }

        /// <summary>
        /// Линия минимального смещения.
        /// </summary>
        private static void SetMinOffsetLine(MainWindowVM parameter, double? error, double offset)
        {
            var lastValue = parameter.Data.Last();
            var minOffset = (double)((1 - error) * offset);

            var minOffsetList = new List<double>();

            minOffsetList.AddRange(parameter.Data);
            minOffsetList.Add(lastValue + minOffset);

            var lineValues = new LiveCharts.ChartValues<double>(minOffsetList);

            parameter.Lines.Add(new LineSeries()
            {
                Title = "MinOffset:",
                LineSmoothness = 0,
                Values = lineValues
            });

            parameter.MinValue = minOffsetList.Min() - DataConstants.OFFSET;
        }

        /// <summary>
        /// Линия максимального смещения.
        /// </summary>
        private static void SetMaxOffsetLine(MainWindowVM parameter, double? error, double offset)
        {
            var lastValue = parameter.Data.Last();
            var maxOffset = (double)((error + 1) * offset);

            var maxOffsetList = new List<double>();

            maxOffsetList.AddRange(parameter.Data);
            maxOffsetList.Add(lastValue + maxOffset);

            var lineValues = new LiveCharts.ChartValues<double>(maxOffsetList);

            parameter.Lines.Add(new LineSeries()
            {
                Title = "MaxOffset:",
                LineSmoothness = 0,
                Values = lineValues
            });

            parameter.MaxValue = maxOffsetList.Max() + DataConstants.OFFSET;
        }
    }
}
