namespace FC.Core.Utils
{
    using FC.Core.Layers;
    using FC.Core.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Инструмент обучения нейронной сети.
    /// </summary>
    public class LearningUtil
    {
        /// <summary>
        /// Дата сеты.
        /// </summary>
        private Dictionary<int, List<double>> _dataSets;

        /// <summary>
        /// Слои.
        /// </summary>
        private List<Layer> _layers;

        /// <summary>
        /// Конфигурация сети.
        /// </summary>
        private ConfigurationModel _configuration;

        /// <summary>
        /// Инструмент обучения нейронной сети.
        /// </summary>
        public LearningUtil(Dictionary<int, List<double>> dataSets, List<Layer> layers,
            ConfigurationModel configuration)
        {
            _dataSets = dataSets;
            _layers = layers;
            _configuration = configuration;
        }

        public void Start()
        {

        }
    }
}
