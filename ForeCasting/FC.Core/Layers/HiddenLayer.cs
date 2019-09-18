namespace FC.Core.Layers
{
    using System;

    using FC.BL.Enums;
    using System.Collections.Generic;
    using FC.Core.Models;

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
            if (_countOfNeurons.Equals(0))
            {
                _output = _inputData;
                return;
            }

            var neurons = new List<NeuronModel>();

            for (var index = 0; index < _countOfNeurons; ++index)
            {
                var weights = new List<double>();
                var random = new Random();

                for (var wIndex = 0; wIndex < _inputData.Count; ++wIndex)
                {
                    weights.Add(random.NextDouble());
                }

                var neuron = new NeuronModel(_inputData, weights);
                neurons.Add(neuron);
            }

            neurons.ForEach(neuron => _output.Add(neuron.Output));
        }

        /// <summary>
        /// Получить данные слоя.
        /// </summary>
        /// <returns>Возвращает список выходных значений.</returns>
        public List<double> GetData() => _inputData;
    }
}
