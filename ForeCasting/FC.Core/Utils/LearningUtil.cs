namespace FC.Core.Utils
{
    using FC.BL.Enums;
    using FC.Core.Layers;
    using FC.Core.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using System.Windows;
    using FC.BL.Constants;

    /// <summary>
    /// Инструмент обучения нейронной сети.
    /// </summary>
    public class LearningUtil
    {
        /// <summary>
        /// Дата сеты.
        /// </summary>
        private Dictionary<int, List<double>> _dataSets;

        /// <summary>
        /// Ожидаемые значения.
        /// </summary>
        private List<double> _idealResults;

        /// <summary>
        /// Слои.
        /// </summary>
        private List<Layer> _layers;

        /// <summary>
        /// Конфигурация сети.
        /// </summary>
        private ConfigurationModel _configuration;

        /// <summary>
        /// Инструмент обучения нейронной сети.
        /// </summary>
        public LearningUtil(List<double> data, int countOfHiddenLayerNeurons,
            ConfigurationModel configuration)
        {
            _configuration = configuration;
            _idealResults = new List<double>();

            Initialize(data, countOfHiddenLayerNeurons);
        }

        /// <summary>
        /// Ининциализация обученияю
        /// </summary>
        /// <param name="data">Данные.</param>
        private void Initialize(List<double> data, int countOfLayerNeurons)
        {
            _dataSets = new Dictionary<int, List<double>>();

            // TODO: Создание датасетов.

            var countOfRepeats = data.Count - TrainingConstants.COUNT_OF_VALUES;

            for (var repeat = 0; repeat < countOfRepeats; ++repeat)
            {
                var currentData = data.GetRange(repeat, TrainingConstants.COUNT_OF_VALUES);
                var currentValues = new List<double>();

                for (var index = 0; index < currentData.Count; ++index)
                {
                    var nextIndex = index + 1;

                    if (nextIndex.Equals(currentData.Count))
                        continue;

                    var value = currentData[nextIndex] - currentData[index];
                    currentValues.Add(value);
                }

                _dataSets.Add(repeat, currentValues);

                var lastDayValue = data[repeat + TrainingConstants.COUNT_OF_VALUES];
                var lastCurrentValue = currentValues.Last();

                _idealResults.Add(lastDayValue - lastCurrentValue);
            }

            _configuration.IterationsInEpochCount = _idealResults.Count;

            _configuration.IdealResult = _idealResults[0];
            var hiddenLayer = new HiddenLayer(_dataSets[0], countOfLayerNeurons);
            hiddenLayer.Initialize();

            var outputLyer = new OutputLayer(hiddenLayer.GetData());
            outputLyer.Initialize();

            _layers = new List<Layer> { hiddenLayer, outputLyer };
        }

        /// <summary>
        /// Начать обучение.
        /// </summary>
        public void Start()
        {
            var hiddenLayer = (HiddenLayer)_layers.Find(layer =>
            layer.LayerType.Equals(LayerType.Hidden));

            var outputLayer = (OutputLayer)_layers.Find(layer =>
                    layer.LayerType.Equals(LayerType.Output));

            var errorDelta = 0d;

            for (var epochCount = 0; epochCount < _configuration.EpochCount; ++epochCount)
            {
                for (var iterationIndex = 0; iterationIndex < _configuration.IterationsInEpochCount;
                    ++iterationIndex)
                {
                    Backpropagation(hiddenLayer, outputLayer, iterationIndex);

                    errorDelta += Math.Pow((_configuration.IdealResult - outputLayer.GetOutput), 2);

                    UpdateData(hiddenLayer, outputLayer, iterationIndex);
                }
            }

            var errorByRootMSE = errorDelta / _configuration.EpochCount;

            var result = MessageBox.Show("Сеть обучена. \nХотите сохранить веса?",
                "Сохранение весов", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result.Equals(MessageBoxResult.Yes))
                SaveDataUtil.Save(_layers, errorByRootMSE);
        }

        /// <summary>
        /// Обновить данные.
        /// </summary>
        /// <param name="hiddenLayer">Скрытый слой.</param>
        /// <param name="outputLayer">Выходной слой.</param>
        /// <param name="iterationIndex">Индекс итерации.</param>
        private void UpdateData(HiddenLayer hiddenLayer, OutputLayer outputLayer, int iterationIndex)
        {
            var nextIndex = iterationIndex + 1;

            if (nextIndex.Equals(_configuration.IterationsInEpochCount))
                nextIndex = 0;

            _configuration.IdealResult = _idealResults[nextIndex];

            hiddenLayer.UpdateData(_dataSets[nextIndex]);
            outputLayer.UpdateData(hiddenLayer.GetData());
        }

        #region Обратное распространение.

        /// <summary>
        /// Индекс текущей итерации.
        /// </summary>
        /// <param name="hiddenLayer">Скрытый слой.</param>
        /// <param name="outputLayer">Выходной слой.</param>
        /// <param name="iterationIndex">Индекс текущей итерации.</param>
        private void Backpropagation(HiddenLayer hiddenLayer, OutputLayer outputLayer,
            int iterationIndex)
        {
            HiddenToOutputDeltasWork(outputLayer, hiddenLayer);
            HiddentToOutputWeightsWork(outputLayer, hiddenLayer);

            InputToHiddenWeightsWork(hiddenLayer, iterationIndex);
        }

        /// <summary>
        /// Обновить веса между входным слоем и скрытым.
        /// </summary>
        /// <param name="hiddenLayer">Скрытый слой.</param>
        /// <param name="currentIteration">Текущая итерация.</param>
        private void InputToHiddenWeightsWork(HiddenLayer hiddenLayer, int currentIteration)
        {
            var neuronToWeightsDictionary = new Dictionary<int, List<double>>();

            var currentInputs = _dataSets[currentIteration];
            var gradients = new Dictionary<int, List<double>>();

            var hiddenLayerNeurons = hiddenLayer.GetLayerNeurons();
            var hiddenLayerDeltas = hiddenLayerNeurons.Select(neuron => neuron.Delta).ToList();

            var index = 0;

            foreach (var delta in hiddenLayerDeltas)
            {
                var weightGradient = new List<double>();

                foreach (var input in currentInputs)
                {
                    var gradient = input * delta;
                    weightGradient.Add(gradient);
                }

                gradients.Add(index, weightGradient);
                index++;
            }

            index = 0;

            foreach (var neuron in hiddenLayerNeurons)
            {
                var weightsDelta = new List<double>();
                var weightIndex = 0;

                foreach (var weight in neuron.Weights)
                {
                    var weightDelta = _configuration.Epsilon * gradients[index][weightIndex] +
                        _configuration.Alpha * neuron.LastWeights[weightIndex];

                    weightsDelta.Add(weightDelta);
                    weightIndex++;
                }

                var newWeights = new List<double>();

                for (var i = 0; i < neuron.Weights.Count; ++i)
                {
                    newWeights.Add(neuron.Weights[i] + weightsDelta[i]);
                }

                neuronToWeightsDictionary.Add(index, newWeights);
                ++index;
            }

            hiddenLayer.UpdateWeightsOfNeuronsInLayer(neuronToWeightsDictionary);
        }

        /// <summary>
        /// Вычисление дельт выходного и скрытого слоя.
        /// </summary>
        /// <param name="outputLayer">Выходной слой.</param>
        /// <param name="hiddenLayer">Скрытый слой.</param>
        private void HiddenToOutputDeltasWork(OutputLayer outputLayer, HiddenLayer hiddenLayer)
        {
            // Получение и обновление дельты выходного слоя.

            var outputOfNeuron = outputLayer.GetOutput;

            var deltaOfOutputNeuron = GetOutputLayerNeuronDelta(outputOfNeuron);
            outputLayer.UpdateDeltaOfNeuronInLayer(deltaOfOutputNeuron);

            // Получение и обновление дельты скрытого слоя.

            var outputNeuron = outputLayer.GetNeuron;

            var hiddenLayerNeurons = hiddenLayer.GetData();
            var deltasOfHiddenLayerNeurons = new List<double>();

            var index = 0;

            foreach (var output in hiddenLayerNeurons)
            {
                var delta = GetHiddenLayerNeuronDelta(output, index, outputNeuron);

                deltasOfHiddenLayerNeurons.Add(delta);
                index++;
            }

            hiddenLayer.UpdateDeltas(deltasOfHiddenLayerNeurons);
        }

        /// <summary>
        /// Расчёт дельты для нейронов скрытого слоя.
        /// </summary>
        /// <param name="neuronOutput">Вывод нейрона.</param>
        /// <param name="indexInLayer">Индекс в скрытом слое.</param>
        /// <param name="outputNeuron">Нейрон выходного слоя.</param>
        /// <returns></returns>
        private double GetHiddenLayerNeuronDelta(
            double neuronOutput, int indexInLayer, NeuronModel outputNeuron) =>
            DerivativeActivationFunction(neuronOutput) *
            outputNeuron.Inputs[indexInLayer] * outputNeuron.Delta;

        /// <summary>
        /// Расчёт дельты выходного нейрона.
        /// </summary>
        /// <param name="neuronOutput">Вывод нейрона.</param>
        /// <returns>Возвращает дельту выходного нейрона.</returns>
        private double GetOutputLayerNeuronDelta(double neuronOutput) =>
            (_configuration.IdealResult - neuronOutput) * DerivativeActivationFunction(neuronOutput);

        /// <summary>
        /// Производная от функции активации (Гиперболический тангенс).
        /// </summary>
        /// <param name="neuronOutput">Вывод нейрона.</param>
        /// <returns>Вовзращает результат производной функции активации (сигмоид) нейрона.</returns>
        private double DerivativeActivationFunction(double neuronOutput) =>
            (1 - Math.Pow(neuronOutput, 2));

        /// <summary>
        /// Получение градиента между нейронами скрытого слоя и выходного, и обновление весов.
        /// </summary>
        /// <param name="outputLayer">Выходной слой.</param>
        /// <param name="hiddenLayer">Скрытый слой.</param>
        private void HiddentToOutputWeightsWork(OutputLayer outputLayer, HiddenLayer hiddenLayer)
        {
            var hiddenLayerOutputs = hiddenLayer.GetData();

            var hiddenLayerGradients = GetHiddenToOutputGradients(hiddenLayerOutputs,
                outputLayer.GetOutput);

            UpdateHiddenToOutputWeights(hiddenLayerGradients, outputLayer);
        }

        /// <summary>
        /// Получить список всех градиентов между скрытым и выходным слоем.
        /// </summary>
        /// <param name="outputs">Выходы нейронов скрытого слоя.</param>
        /// <param name="outputDelta">Дельта выходного нейрона.</param>
        /// <returns>Возвращает список градиентов для скрытого слоя.</returns>
        private List<double> GetHiddenToOutputGradients(List<double> outputs, double outputDelta)
        {
            var gradientsList = new List<double>();

            outputs.ForEach(output => gradientsList.Add(output * outputDelta));

            return gradientsList;
        }

        /// <summary>
        /// Обновить веса между скрытым и выходным слоем.
        /// </summary>
        /// <param name="gradients">Список градиентов между скрытым и выходным слоями.</param>
        /// <param name="outputLayer">Выходной слой.</param>
        private void UpdateHiddenToOutputWeights(List<double> gradients, OutputLayer outputLayer)
        {
            var updatedWeights = new List<double>();
            var weightsDeltas = new List<double>();
            var indexOfGradient = 0;

            foreach (var gradient in gradients)
            {
                var lastWeight = outputLayer.GetNeuron.
                    LastWeights[indexOfGradient];

                var weightDelta = _configuration.Epsilon * gradient +
                    _configuration.Alpha * lastWeight;

                weightsDeltas.Add(weightDelta);
                ++indexOfGradient;
            }

            for (var index = 0; index < outputLayer.GetNeuron.Weights.Count; ++index)
                updatedWeights.Add(outputLayer.GetNeuron.Weights[index] + weightsDeltas[index]);

            outputLayer.UpdateWeightsOfNeuronInLayer(updatedWeights);
        }

        #endregion

    }
}
