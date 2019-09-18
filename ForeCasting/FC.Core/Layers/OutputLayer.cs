namespace FC.Core.Layers
{
    using System;

    using FC.BL.Enums;
    using System.Collections.Generic;
    using FC.Core.Models;

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
        /// Класс выходного слоя.
        /// </summary>
        /// <param name="inputData">Входные данные.</param>
        public OutputLayer(List<double> inputData)
        {
            _inputData = inputData;
        }

        /// <summary>
        /// Инициализировать выходной слой.
        /// </summary>
        public override void Initialize()
        {
            var weights = new List<double>();
            var random = new Random();

            for (var wIndex = 0; wIndex < _inputData.Count; ++wIndex)
                weights.Add(random.NextDouble());

            var neuron = new NeuronModel(_inputData, weights);

            _output = neuron.Output;
        }

        /// <summary>
        /// Вернуть вывод.
        /// </summary>
        /// <returns>Возвращает выходное значение слоя.</returns>
        public double GetOutput() => _output;
    }
}
