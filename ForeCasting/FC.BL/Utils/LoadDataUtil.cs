namespace FC.BL.Utils
{
    using FC.BL.Constants;

    using System.IO;
    using System.Text;
    using System.Windows;

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
    }
}
