namespace FC.Core.Layers
{
    using System;

    using FC.BL.Enums;

    /// <summary>
    /// Класс выходного слоя.
    /// </summary>
    public class OutputLayer : Layer
    {
        /// <summary>
        /// Тип слоя.
        /// </summary>
        public override LayerType LayerType => LayerType.Output;

        /// <summary>
        /// Класс выходного слоя.
        /// </summary>
        public OutputLayer()
        {
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
