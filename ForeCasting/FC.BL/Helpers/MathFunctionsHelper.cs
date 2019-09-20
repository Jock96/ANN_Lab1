namespace FC.BL.Helpers
{
    using System;

    /// <summary>
    /// Класс-помощник математических функций.
    /// </summary>
    public static class MathFunctionsHelper
    {
        /// <summary>
        /// Гиперболический тангенс.
        /// </summary>
        public static double HTanFunction(double x)
            => (Math.Exp(2 * x) - 1) / (Math.Exp(2 * x) + 1);

        /// <summary>
        /// Сигмоид.
        /// </summary>
        public static double SigmoidFunction(double x) => Math.Pow(1 + Math.Exp(-x), -1);

        /// <summary>
        /// Гиперболический тангенс (производная).
        /// </summary>
        public static double DerivativeHTanFunction(double x) => (1 - Math.Pow(x, 2));
    }
}
