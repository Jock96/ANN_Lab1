﻿namespace FC.Core.Models
{
    /// <summary>
    /// Модель конфигурации.
    /// </summary>
    public class ConfigurationModel
    {
        /// <summary>
        /// Количество эпох.
        /// </summary>
        public int EpochCount { get; set; } = 200;

        /// <summary>
        /// Идеальный результат.
        /// </summary>
        public double IdealResult { get; set; } = 1;

        /// <summary>
        /// Скорость обучения.
        /// </summary>
        public double Epsilon { get; set; } = 14;

        /// <summary>
        /// Момент.
        /// </summary>
        public double Alpha { get; set; } = 7;

        /// <summary>
        /// Количество итераций в одной эпохе.
        /// </summary>
        public int IterationsInEpochCount { get; set; } = 1;
    }
}
