﻿namespace FC.UI.Commands
{
    using FC.BL.Constants;
    using FC.Core.Layers;
    using FC.Core.Models;
    using FC.Core.Utils;
    using FC.UI.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Обучение сети.
    /// </summary>
    public class TrainNetworkCommand : BaseTCommand<MainWindowVM>
    {
        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        protected override void Execute(MainWindowVM parameter)
        {
            if (!parameter.NormilizedData.Any())
            {
                MessageBox.Show("Отсутвуют данные для обучения!\n" +
                    "Загрузите данные.");

                return;
            }

            var alphaString = string.Empty;
            var epsilonString = string.Empty;
            var epochCountString = parameter.EpochCount;

            if (parameter.Alpha.Contains("."))
                alphaString = parameter.Alpha.Replace(".", ",");
            else
                alphaString = parameter.Alpha;

            if (!double.TryParse(alphaString, out var alpha))
            {
                MessageBox.Show("Параметры имеют неверный формат!");
                return;
            }

            if (parameter.Epsilon.Contains("."))
                epsilonString = parameter.Epsilon.Replace(".", ",");
            else
                epsilonString = parameter.Epsilon;

            if (!double.TryParse(epsilonString, out var epsilon))
            {
                MessageBox.Show("Параметры имеют неверный формат!");
                return;
            }

            if (!int.TryParse(epochCountString, out var epochCount))
            {
                MessageBox.Show("Параметры имеют неверный формат!");
                return;
            }

            var countOfLayerNeurons = int.Parse(parameter.CountOfHiddenLayerNeurons);
            var firstSetData = GetFirstStepData(parameter);

            var allSets = new Dictionary<int, List<double>>
            {
                { 0, firstSetData }
            };

            FillDictionaryByAnotherSets(parameter, allSets);

            var hiddenLayer = new HiddenLayer(firstSetData, countOfLayerNeurons);
            hiddenLayer.Initialize();

            var outputLyer = new OutputLayer(hiddenLayer.GetData());
            outputLyer.Initialize();

            var listOfNeurons = new List<Layer> { hiddenLayer, outputLyer };

            var configuration = new ConfigurationModel()
            {
                Alpha = alpha,
                Epsilon = epsilon,
                EpochCount = epochCount
            };

            var learningUtil = new LearningUtil(allSets,  listOfNeurons, configuration);
        }

        /// <summary>
        /// Заполнить словарь другими сетами.
        /// </summary>
        /// <param name="parameter">Параметер.</param>
        /// <param name="allSets">Словарь</param>
        private static void FillDictionaryByAnotherSets(MainWindowVM parameter,
            Dictionary<int, List<double>> allSets)
        {
            for (var index = 0; index < TrainingConstants.STEP_OFFSET; ++index)
            {
                var anotherSet = new List<double>();
                var step = parameter.NormilizedData.Count - TrainingConstants.STEP_OFFSET;

                for (var startIndex = index + 1; startIndex < step; ++startIndex)
                {
                    var nextIndex = index + 1;

                    if (nextIndex.Equals(step))
                        continue;

                    var delta = parameter.NormilizedData[nextIndex] -
                        parameter.NormilizedData[index];

                    anotherSet.Add(delta);
                }

                allSets.Add(index + 1, anotherSet);
            }
        }

        /// <summary>
        /// Получить первый сет для обучения.
        /// </summary>
        /// <param name="parameter">Параметер.</param>
        /// <returns>Возвращает первый сет.</returns>
        private static List<double> GetFirstStepData(MainWindowVM parameter)
        {
            var firstSetData = new List<double>();
            var step = parameter.NormilizedData.Count - TrainingConstants.STEP_OFFSET;

            for (var index = 0; index < step; ++index)
            {
                var nextIndex = index + 1;

                if (nextIndex.Equals(step))
                    continue;

                var delta = parameter.NormilizedData[nextIndex] - parameter.NormilizedData[index];
                firstSetData.Add(delta);
            }

            return firstSetData;
        }
    }
}
