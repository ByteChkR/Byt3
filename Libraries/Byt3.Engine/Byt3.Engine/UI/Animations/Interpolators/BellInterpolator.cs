using Byt3.Engine.Core;

namespace Byt3.Engine.UI.Animations.Interpolators
{
    /// <summary>
    /// Bell Interpolator implementation
    /// </summary>
    public class BellInterpolator : Interpolator
    {
        public Interpolator Smoothness { get; set; } = new StaticInterpolator();

        /// <summary>
        /// Implements the Bell Interpolation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override float _GetValue(float input)
        {
            return Interpolations.BellCurve(input, Smoothness.GetValue(input));
        }
    }
}