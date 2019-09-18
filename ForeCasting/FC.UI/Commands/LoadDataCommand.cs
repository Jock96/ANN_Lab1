namespace FC.UI.Commands
{
    using FC.BL.Constants;
    using FC.BL.Helpers;
    using FC.BL.Utils;
    using FC.UI.ViewModels;

    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Text;
    using System.Windows;

    /// <summary>
    /// Загрузить данные.
    /// </summary>
    public class LoadDataCommand : BaseTCommand<MainWindowVM>
    {
        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        protected override void Execute(MainWindowVM parameter)
        {
            var fullPath = $"{PathHelper.GetResourcesPath()}{PathConstants.DATA_PATH}";

            var fileDialog = new OpenFileDialog
            {
                Filter = @"(*.txt)|*.txt",
                InitialDirectory = fullPath
            };

            var file = fileDialog.ShowDialog();

            if (!file.HasValue || string.Equals(string.Empty, fileDialog.FileName))
                return;

            try
            {
                using (var stream = fileDialog.OpenFile())
                {
                    var array = new byte[stream.Length];
                    stream.Read(array, 0, array.Length);

                    var valueString = Encoding.Default.GetString(array);
                    var dataList = DataConverterUtil.ConvertStringToDataList(valueString);

                    parameter.ChartData = dataList;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Не удалось импортировать файл!" +
                    $"\nОшибка: {exception.ToString()}");
            }
        }
    }
}
