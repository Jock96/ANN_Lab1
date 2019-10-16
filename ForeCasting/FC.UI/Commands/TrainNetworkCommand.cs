namespace FC.UI.Commands
{
    using FC.Core.Models;
    using FC.Core.Utils;

    using FC.UI.ViewModels;

    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Обучение сети.
    /// </summary>
    public class TrainNetworkCommand : BaseTCommand<MainWindowVM>
    {
        /// <summary>
        /// Выполнить.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        protected override void Execute(MainWindowVM parameter)
        {
            if (!parameter.Data.Any())
            {
                MessageBox.Show("Отсутвуют данные для обучения!\n" +
                    "Загрузите данные.");

                return;
            }

            var countOfLayerNeurons = int.Parse(parameter.CountOfHiddenLayerNeurons);

            //if (countOfLayerNeurons.Equals(0))
            //{
            //    MessageBox.Show("Операция для 0 нейронов не реализована.");

            //    return;
            //}

            var alphaString = string.Empty;
            var epsilonString = string.Empty;
            var epochCountString = parameter.EpochCount;

            if (parameter.Alpha.Contains("."))
                alphaString = parameter.Alpha.Replace(".", ",");
            else
                alphaString = parameter.Alpha;

            if (!double.TryParse(alphaString, out var alpha))
            {
                MessageBox.Show("Параметры имеют неверный формат!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if (parameter.Epsilon.Contains("."))
                epsilonString = parameter.Epsilon.Replace(".", ",");
            else
                epsilonString = parameter.Epsilon;

            if (!double.TryParse(epsilonString, out var epsilon))
            {
                MessageBox.Show("Параметры имеют неверный формат!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if (!int.TryParse(epochCountString, out var epochCount))
            {
                MessageBox.Show("Параметры имеют неверный формат!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            var configuration = new ConfigurationModel()
            {
                Alpha = alpha,
                Epsilon = epsilon,
                EpochCount = epochCount
            };

            var learningUtil = new LearningUtil(parameter.Data, countOfLayerNeurons, configuration);
            learningUtil.Start();
        }
    }
}
