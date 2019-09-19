namespace FC.UI.Commands
{
    using FC.BL.Constants;
    using FC.BL.Helpers;
    using FC.BL.Utils;

    using FC.UI.ViewModels;
    using System.IO;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Предсказать.
    /// </summary>
    public class ForeCastCommand : BaseTCommand<MainWindowVM>
    {
        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        protected override void Execute(MainWindowVM parameter)
        {
            if (!parameter.NormilizedData.Any())
            {
                MessageBox.Show("Отсутвуют данные для прогноза!\n" +
                    "Загрузите данные.");

                return;
            }

            var result = MessageBox.Show("Использовать стандартную директорию весов?",
                "Загрузка весов", MessageBoxButton.YesNo, MessageBoxImage.Question);

            var directory = $"{PathHelper.GetResourcesPath()}{PathConstants.SAVE_PATH}";

            if (result.Equals(MessageBoxResult.No))
            {
                MessageBox.Show("Операция не реализована!");
                return;
            }

            var error = LoadDataUtil.LoadErrorData(directory);

            if (error == null)
                return;

            var errorInPercentString = DataConverterUtil.ToPercentString((double)error);

            parameter.PercentLabelVisibility = Visibility.Visible;
            parameter.ErrorByPercentString = $"Ошибка в предсказании: {errorInPercentString}";

            var layersData = LoadDataUtil.LoadLayersData(directory);
        }
    }
}
