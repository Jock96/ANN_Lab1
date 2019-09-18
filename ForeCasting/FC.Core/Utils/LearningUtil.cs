namespace FC.Core.Utils
{
    using FC.Core.Layers;
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
        /// Инструмент обучения нейронной сети.
        /// </summary>
        public LearningUtil(Dictionary<int, List<double>> dataSets, List<Layer> layers)
        {
            _dataSets = dataSets;
            _layers = layers;
        }

        public void Start()
        {

        }
    }
}
