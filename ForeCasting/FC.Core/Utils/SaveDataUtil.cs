namespace FC.Core.Utils
{
    using FC.BL.Constants;
    using FC.BL.Enums;
    using FC.BL.Helpers;

    using FC.Core.Layers;

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Инструмент сохранения данных.
    /// </summary>
    public static class SaveDataUtil
    {
        /// <summary>
        /// Сохранить.
        /// </summary>
        /// <param name="layers">Список слоёв.</param>
        /// <param name="error">Ошибка.</param>
        public static void Save(List<Layer> layers, double error)
        {
            var resourcesDirectory = PathHelper.GetResourcesPath();
            var saveDirectory = $"{resourcesDirectory}{PathConstants.SAVE_PATH}";

            if (!Directory.Exists(saveDirectory))
                Directory.CreateDirectory(saveDirectory);

            var outputLayer = (OutputLayer)layers.Find(layer =>
                    layer.LayerType.Equals(LayerType.Output));

            var hiddenLayer = (HiddenLayer)layers.Find(layer =>
            layer.LayerType.Equals(LayerType.Hidden));

            OutputLayerSave(saveDirectory, outputLayer);
            HiddenLayerSave(saveDirectory, hiddenLayer);
            ErroInfoSave(error, saveDirectory);
        }

        #region Сохранение файлов.

        /// <summary>
        /// Сохранение скрытого слоя.
        /// </summary>
        /// <param name="saveDirectory">Директория сохранения.</param>
        /// <param name="hiddenLayer">Скрытый слой.</param>
        private static void HiddenLayerSave(string saveDirectory, HiddenLayer hiddenLayer)
        {
            var neurons = hiddenLayer.GetLayerNeurons();

            if (!neurons.Any())
                return;

            var indexOfNeuron = 0;

            foreach (var neuron in neurons)
            {
                var fileToSave = Path.Combine(saveDirectory,
                    $"{FileNamesConstants.HIDDEN_LAYER}{FileNamesConstants.NEURON_NAME}" +
                    $"{indexOfNeuron}{FileNamesConstants.DEFAULT_EXTENSION}");

                using (var stream = new StreamWriter(fileToSave))
                {
                    var weights = neuron.Weights;

                    var index = 1;

                    foreach (var weight in weights)
                    {
                        var separator = FileNamesConstants.SEPARATOR;

                        if (index.Equals(weights.Count))
                            separator = string.Empty;

                        stream.Write($"{weight}{separator}");
                        ++index;
                    }
                }

                ++indexOfNeuron;
            }
        }

        /// <summary>
        /// Сохранение выходного слоя.
        /// </summary>
        /// <param name="saveDirectory">Директория сохранения.</param>
        /// <param name="outputLayer">Выходной слой.</param>
        private static void OutputLayerSave(string saveDirectory, OutputLayer outputLayer)
        {
            var fileToSave = Path.Combine(saveDirectory,
                            $"{FileNamesConstants.OUTPUT_LAYER}" +
                            $"{FileNamesConstants.NEURON_NAME}{0}" +
                            $"{FileNamesConstants.DEFAULT_EXTENSION}");

            using (var stream = new StreamWriter(fileToSave))
            {
                var weights = outputLayer.GetNeuron.Weights;
                var index = 1;

                foreach (var weight in weights)
                {
                    var separator = FileNamesConstants.SEPARATOR;

                    if (index.Equals(weights.Count))
                        separator = string.Empty;

                    stream.Write($"{weight}{separator}");
                    ++index;
                }
            }
        }

        /// <summary>
        /// Сохранение ошибки.
        /// </summary>
        /// <param name="error">Ошибка.</param>
        /// <param name="saveDirectory">Директория для сохранения.</param>
        private static void ErroInfoSave(double error, string saveDirectory)
        {
            var fileToSave = Path.Combine(saveDirectory, $"{FileNamesConstants.ERROR_INFO}" +
                            $"{FileNamesConstants.DEFAULT_EXTENSION}");

            using (var stream = new StreamWriter(fileToSave))
            {
                stream.Write(error);
            }
        }

        #endregion
    }
}
