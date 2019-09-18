namespace FC.Core.Layers
{
    using FC.BL.Enums;

    using System;

    /// <summary>
    /// Класс входного слоя.
    /// </summary>
    public class InputLayer : Layer
    {
        /// <summary>
        /// Тип слоя.
        /// </summary>
        public override LayerType LayerType => LayerType.Input;

        /// <summary>
        /// Класс входного слоя.
        /// </summary>
        public InputLayer()
        { 
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
