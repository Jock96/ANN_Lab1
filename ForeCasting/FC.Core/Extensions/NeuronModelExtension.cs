namespace FC.Core.Extensions
{
    using FC.Core.Models;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Класс расширений для операций с моделями нейронов.
    /// </summary>
    public static class NeuronModelExtension
    {
        /// <summary>
        /// Обновление весов нейрона.
        /// </summary>
        /// <param name="neuronModel">Модель нейрона.</param>
        /// <param name="weights">Новые веса.</param>
        public static void UpdateWeights(this NeuronModel neuronModel, List<double> weights)
        {
            if (neuronModel.Weights.Count != weights.Count)
            {
                MessageBox.Show("Не соответствие по количеству весов!", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            neuronModel.LastWeights = neuronModel.Weights;
            neuronModel.Weights = weights;
        }
    }
}
