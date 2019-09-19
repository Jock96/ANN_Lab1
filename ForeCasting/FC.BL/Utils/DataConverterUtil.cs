namespace FC.BL.Utils
{
    using FC.BL.Constants;

    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Инструмент преобразования данных.
    /// </summary>
    public static class DataConverterUtil
    {
        /// <summary>
        /// Преобразовать строку в список данных.
        /// </summary>
        /// <param name="dataString">Стркоа данных.</param>
        /// <returns>Возвращает список данных.</returns>
        public static List<double> ConvertStringToDataList(string dataString)
        {
            var data = new List<double>();
            var indexOfSeparator = -1;

            var breakFlag = false;

            do
            {
                indexOfSeparator = dataString.IndexOf(DataConstants.SEPARATOR);

                if (indexOfSeparator.Equals(-1))
                {
                    if (dataString.Contains("."))
                        dataString = dataString.Replace(".", ",");

                    if (!double.TryParse(dataString, out var lastValue))
                        MessageBox.Show("Не удалось преобразовать данные!", "Ошибка", 
                            MessageBoxButton.OK, MessageBoxImage.Error);

                    data.Add(lastValue);

                    breakFlag = true;
                    continue;
                }

                var valueString = dataString.Remove(indexOfSeparator);
                var preparedValueString = valueString;

                if (valueString.Contains("."))
                    preparedValueString = valueString.Replace(".", ",");

                if (!double.TryParse(preparedValueString, out var value))
                    MessageBox.Show("Не удалось преобразовать данные!", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);

                data.Add(value);

                dataString = dataString.Replace($"{valueString}" +
                    $"{DataConstants.SEPARATOR}", string.Empty);

            } while (!breakFlag);

            return data;
        }

        /// <summary>
        /// Нормализовать данные.
        /// </summary>
        /// <param name="data">Данные.</param>
        /// <returns>Возвращает нормализованные данные.</returns>
        public static List<double> Normilize(List<double> data)
        {
            var normilizedData = new List<double>();

            data.ForEach(value => normilizedData.Add(1 / value));

            return normilizedData;
        }

        /// <summary>
        /// Перевети в проценты.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает точное значение в процентах.</returns>
        public static int ToPercent(double value) => (int)(value * 100);

        /// <summary>
        /// Перевести в процентную строку.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает строковое представление значения в процентах.</returns>
        public static string ToPercentString(double value) => $"{ToPercent(value)}%";
    }
}
