namespace FC.Core.Layers
{
    using FC.BL.Enums;

    /// <summary>
    /// Слой нейронной сети.
    /// </summary>
    public abstract class Layer
    {
        /// <summary>
        /// Тип слоя.
        /// </summary>
        public abstract LayerType LayerType { get; }

        /// <summary>
        /// Инициализация слоя.
        /// </summary>
        public abstract void Initialize();
    }
}
