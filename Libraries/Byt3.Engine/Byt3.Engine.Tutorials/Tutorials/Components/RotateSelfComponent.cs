using Byt3.Engine.Core;
using OpenTK;

namespace Byt3.Engine.Tutorials.Tutorials.Components
{
    public class RotateSelfComponent : AbstractComponent
    {
        protected override void Update(float deltaTime)
        {
            Owner.Rotate(Vector3.UnitY, MathHelper.DegreesToRadians(45f) * deltaTime);
        }
    }
}