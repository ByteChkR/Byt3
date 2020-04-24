using Byt3.Engine.Core;

namespace Byt3.Engine.UI.Animations.Interpolators
{
    /// <summary>
    /// Spherical Interpolator Interpolator implementation
    /// </summary>
    public class SphericalInterpolator : Interpolator
    {
        /// <summary>
        /// Implements the Spherical Interpolation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override float _GetValue(float input)
        {
            return Interpolations.Slerp(input);
        }
    }
}