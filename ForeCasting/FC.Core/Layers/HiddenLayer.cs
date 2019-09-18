namespace FC.Core.Layers
{
    using System;

    using FC.BL.Enums;

    /// <summary>
    /// Класс скрытого слоя.
    /// </summary>
    public class HiddenLayer : Layer
    {
        /// <summary>
        /// Тип слоя.
        /// </summary>
        public override LayerType LayerType => LayerType.Hidden;

        /// <summary>
        /// Класс скрытого слоя.
        /// </summary>
        public HiddenLayer()
        {
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
