namespace FC.Core.Layers
{
    using System;

    using FC.BL.Enums;
    using System.Collections.Generic;
    using FC.Core.Models;
    using FC.Core.Extensions;

    /// <summary>
    /// Класс выходного слоя.
    /// </summary>
    public class OutputLayer : Layer
    {
        /// <summary>
        /// Тип слоя.
        /// </summary>
        public override LayerType LayerType => LayerType.Output;

        /// <summary>
        /// Входные данные.
        /// </summary>
        private List<double> _inputData;

        /// <summary>
        /// Выход слоя.
        /// </summary>
        private double _output;

        /// <summary>
        /// Нейрон выходного слоя.
        /// </summary>
        private NeuronModel _outputNeuron;

        /// <summary>
        /// Класс выходного слоя.
        /// </summary>
        /// <param name="inputData">Входные данные.</param>
        public OutputLayer(List<double> inputData)
        {
            _inputData = inputData;
        }

        /// <summary>
        /// Инициализировать для прогноза.
        /// </summary>
        /// <param name="weightsDictionary">Веса.</param>
        public void SwitchToForeCast(List<double> weights)
        {
            _outputNeuron = new NeuronModel(_inputData, weights);
            _output = _outputNeuron.Output;
        }

        /// <summary>
        /// Инициализировать выходной слой.
        /// </summary>
        public override void Initialize()
        {
            var weights = new List<double>();
            var emptyWeights = new List<double>();

            var random = new Random();

            for (var wIndex = 0; wIndex < _inputData.Count; ++wIndex)
            {
                weights.Add(random.NextDouble());
                emptyWeights.Add(0d);
            }

            _outputNeuron = new NeuronModel(_inputData, weights)
            {
                LastWeights = emptyWeights
            };

            _output = _outputNeuron.Output;
        }

        /// <summary>
        /// Вернуть вывод.
        /// </summary>
        /// <returns>Возвращает выходное значение слоя.</returns>
        public double GetOutput => _output;

        /// <summary>
        /// Вернуть нейрон.
        /// </summary>
        public NeuronModel GetNeuron => _outputNeuron;

        /// <summary>
        /// Обновить дельту нейрона на слое.
        /// </summary>
        /// <param name="delta">Дельта</param>
        public void UpdateDeltaOfNeuronInLayer(double delta) => _outputNeuron.Delta = delta;

        /// <summary>
        /// Обновить веса нейрона на слое.
        /// </summary>
        /// <param name="updatedWeights">Обновлённые веса.</param>
        public void UpdateWeightsOfNeuronInLayer(List<double> updatedWeights) 
            => _outputNeuron.UpdateWeights(updatedWeights);

        /// <summary>
        /// Обновить входные значения.
        /// </summary>
        /// <param name="inputs">Входные значения.</param>
        public void UpdateData(List<double> inputs)
        {
            _outputNeuron = new NeuronModel(inputs, _outputNeuron.Weights)
            {
                LastWeights = _outputNeuron.LastWeights
            };
        }
    }
}
