namespace FC.Core.Utils
{
    using FC.BL.Enums;

    using System.Collections.Generic;

    using FC.Core.Layers;
    using System.Windows;

    /// <summary>
    /// Инструмент прогнозирования.
    /// </summary>
    public class ForeCastUtil
    {
        /// <summary>
        /// Данные для скрытого слоя.
        /// </summary>
        private Dictionary<int, List<double>> _hiddenLayerData;

        /// <summary>
        /// Данные для выходного слоя.
        /// </summary>
        private List<double> _outputLayerData;

        /// <summary>
        /// Данные.
        /// </summary>
        private List<double> _data;

        /// <summary>
        /// Инструмент прогнозирования.
        /// </summary>
        /// <param name="inputConfiguration">Конфигурация слоёв.</param>
        /// <param name="inputData">Входные данные для прогнозирования.</param>
        public ForeCastUtil(Dictionary<DataType, Dictionary<int, List<double>>> inputConfiguration,
            List<double> inputData)
        {
            _outputLayerData = inputConfiguration[DataType.OutputLayerData][0];
            _hiddenLayerData = inputConfiguration[DataType.HiddenLayerData];

            NormilizeData(inputData);
        }

        /// <summary>
        /// Прогнозировать смещение.
        /// </summary>
        /// <returns>Возвращает смещение валют.</returns>
        public double GetOffset()
        {
            var neuronCount = _hiddenLayerData.Count;

            if (neuronCount.Equals(0))
                MessageBox.Show("Операция для 0 нейронов не реализована!");

            var hiddenLayer = new HiddenLayer(_data, neuronCount);
            hiddenLayer.SwitchToForeCast(_hiddenLayerData);

            var outputLayer = new OutputLayer(hiddenLayer.GetData());
            outputLayer.SwitchToForeCast(_outputLayerData);

            return outputLayer.GetOutput;
        }

        /// <summary>
        /// Нормализация входных данных.
        /// </summary>
        /// <param name="inputData">Входные данные.</param>
        private void NormilizeData(List<double> inputData)
        {
            _data = new List<double>();

            for (var index = 0; index < inputData.Count; ++index)
            {
                var nextIndex = index + 1;

                if (nextIndex.Equals(inputData.Count))
                    continue;

                var value = inputData[nextIndex] - inputData[index];
                _data.Add(value);
            }
        }
    }
}
