namespace FC.Core.Layers
{
    using System;

    using FC.BL.Enums;
    using System.Collections.Generic;
    using FC.Core.Models;
    using FC.Core.Extensions;

    /// <summary>
    /// Класс скрытого слоя.
    /// </summary>
    public class HiddenLayer : Layer
    {
        /// <summary>
        /// Тип слоя.
        /// </summary>
        public override LayerType LayerType => LayerType.Hidden;

        /// <summary>
        /// Входные данные.
        /// </summary>
        private List<double> _inputData;

        /// <summary>
        /// Количество нейронов в слое.
        /// </summary>
        private int _countOfNeurons;

        /// <summary>
        /// Выходные данные.
        /// </summary>
        private List<double> _output;

        /// <summary>
        /// Нейроны слоя.
        /// </summary>
        private List<NeuronModel> _neurons;

        /// <summary>
        /// Класс скрытого слоя.
        /// </summary>
        public HiddenLayer(List<double> inputData, int countOfNeurons)
        {
            _inputData = inputData;
            _countOfNeurons = countOfNeurons;
        }

        /// <summary>
        /// Инициализация слоя.
        /// </summary>
        public override void Initialize()
        {
            _neurons = new List<NeuronModel>();
            _output = new List<double>();

            if (_countOfNeurons.Equals(0))
            {
                _output = _inputData;
                return;
            }

            var neurons = new List<NeuronModel>();

            for (var index = 0; index < _countOfNeurons; ++index)
            {
                var weights = new List<double>();
                var emptyWeights = new List<double>();

                var random = new Random();

                for (var wIndex = 0; wIndex < _inputData.Count; ++wIndex)
                {
                    weights.Add(random.NextDouble());
                    emptyWeights.Add(0d);
                }

                var neuron = new NeuronModel(_inputData, weights)
                {
                    LastWeights = emptyWeights
                };

                neurons.Add(neuron);
            }

            neurons.ForEach(neuron => _output.Add(neuron.Output));
            _neurons.AddRange(neurons);
        }

        /// <summary>
        /// Обновление дельт нейронов.
        /// </summary>
        /// <param name="deltas">Список дельт.</param>
        public void UpdateDeltas(List<double> deltas) =>
            _neurons.ForEach(neuron =>
            neuron.Delta = deltas[_neurons.IndexOf(neuron)]);

        /// <summary>
        /// Получить данные слоя.
        /// </summary>
        /// <returns>Возвращает список выходных значений.</returns>
        public List<double> GetData() => _output;

        /// <summary>
        /// Получить все нейроны слоя.
        /// </summary>
        /// <returns></returns>
        public List<NeuronModel> GetLayerNeurons() => _neurons;

        /// <summary>
        /// Обновить веса у нейронов на слое.
        /// </summary>
        /// <param name="indexToWeightsDictionary">Индекс нейрона и его веса.</param>
        public void UpdateWeightsOfNeuronsInLayer(
            Dictionary<int, List<double>> indexToWeightsDictionary)
        {
            var index = 0;

            foreach (var neuron in _neurons)
            {
                var weights = indexToWeightsDictionary[index];
                neuron.UpdateWeights(weights);

                ++index;
            }
        }

        /// <summary>
        /// Обновление данных.
        /// </summary>
        /// <param name="dataSet">Датасет.</param>
        public void UpdateData(List<double> dataSet)
        {
            var updatedNeurons = new List<NeuronModel>();

            foreach (var neuron in _neurons)
            {
                var newNeuron = new NeuronModel(dataSet, neuron.Weights)
                {
                    LastWeights = neuron.LastWeights
                };

                updatedNeurons.Add(neuron);
            }

            _neurons = updatedNeurons;
        }
    }
}
