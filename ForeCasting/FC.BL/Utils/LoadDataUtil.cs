namespace FC.BL.Utils
{
    using FC.BL.Constants;
    using FC.BL.Enums;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System;

    /// <summary>
    /// Инструмент загрузки данных.
    /// </summary>
    public static class LoadDataUtil
    {
        /// <summary>
        /// Загрузить данные об ошибке.
        /// </summary>
        /// <param name="directory">Директория.</param>
        /// <returns>Возвращает ошибку в формате 
        /// "double"(<see cref="System.Double"/>).</returns>
        public static double? LoadErrorData(string directory)
        {
            var file = Path.Combine(directory, 
                $"{FileNamesConstants.ERROR_INFO}" +
                $"{FileNamesConstants.DEFAULT_EXTENSION}");

            if (!File.Exists(file))
            {
                MessageBox.Show("В указаной директории не существует нужный файл!");
                return null;
            }

            using (var stream = File.OpenRead(file))
            {
                var array = new byte[stream.Length];
                stream.Read(array, 0, array.Length);

                var valueString = Encoding.Default.GetString(array);

                if (!double.TryParse(valueString, out var error))
                {
                    MessageBox.Show("Ошибка преобразования данных!");
                    return null;
                }

                return error;
            }
        }

        /// <summary>
        /// получить данные слоёв.
        /// </summary>
        /// <param name="directory">Путь до файлов с данными.</param>
        /// <returns>Возвращает словарь, где ключ - тип данных, значение - данные.</returns>
        public static Dictionary<DataType, Dictionary<int, List<double>>> LoadLayersData(
            string directory)
        {
            var layersDataDictionary = new Dictionary<DataType, Dictionary<int, List<double>>>();

            var files = Directory.GetFiles(directory);

            var outputLayerFile = string.Empty;
            var hiddenLayerFiles = new List<string>();

            foreach (var file in files)
            {
                if (file.Contains(FileNamesConstants.OUTPUT_LAYER))
                {
                    outputLayerFile = file;
                    continue;
                }

                if (file.Contains(FileNamesConstants.HIDDEN_LAYER))
                    hiddenLayerFiles.Add(file);
            }

            var outputData = LoadLayerData(outputLayerFile);

            if (outputData == null)
                return null;

            var outputLayerDataDictionary = new Dictionary<int, List<double>>
            {
                { 0,  outputData}
            };

            layersDataDictionary.Add(DataType.OutputLayerData, outputLayerDataDictionary);

            var hiddenLayerDataDictionary = new Dictionary<int, List<double>>();

            foreach (var file in hiddenLayerFiles)
            {
                var hiddenLayerData = LoadLayerData(file);

                if (hiddenLayerData == null)
                    return null;

                var fileName = Path.GetFileNameWithoutExtension(file);
                var indexOfNeuronString = fileName.Replace($"{FileNamesConstants.HIDDEN_LAYER}" +
                    $"{FileNamesConstants.NEURON_NAME}", string.Empty);

                var index= int.Parse(indexOfNeuronString);
                hiddenLayerDataDictionary.Add(index, hiddenLayerData);
            }

            layersDataDictionary.Add(DataType.HiddenLayerData, hiddenLayerDataDictionary);

            return layersDataDictionary;
        }

        /// <summary>
        /// Загрузить данные выходного слоя.
        /// </summary>
        /// <param name="outputLayerFile">Файл выходного слоя.</param>
        private static List<double> LoadLayerData(string outputLayerFile)
        {
            if (string.Equals(outputLayerFile, string.Empty))
            {
                MessageBox.Show("Не удалось найти файл с данными в указанной директории!");
                return null;
            }

            var values = new List<double>();

            using (var stream = File.OpenRead(outputLayerFile))
            {
                var array = new byte[stream.Length];
                stream.Read(array, 0, array.Length);

                var valueString = Encoding.Default.GetString(array);
                var indexOfSeparator = 0;

                do
                {
                    indexOfSeparator = valueString.IndexOf(FileNamesConstants.SEPARATOR);

                    if (indexOfSeparator == -1)
                    {
                        if (!double.TryParse(valueString, out var lastValue))
                        {
                            MessageBox.Show("Ошибка преобразования данных!");
                            return null;
                        }

                        values.Add(lastValue);
                        continue;
                    }

                    var value = valueString.Remove(indexOfSeparator);

                    if (!double.TryParse(value, out var convertedValue))
                    {
                        MessageBox.Show("Ошибка преобразования данных!");
                        return null;
                    }

                    values.Add(convertedValue);
                    valueString = valueString.Remove(0, value.Length + 1);
                } while (indexOfSeparator != -1);
            }

            return values;
        }
    }
}
