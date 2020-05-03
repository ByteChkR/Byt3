using Byt3.Collections;
using Byt3.Engine.Core;
using OpenTK;

namespace Byt3.Engine.Demos.components
{
    public class RotatingComponent : AbstractComponent
    {
        protected override void Update(float deltaTime)
        {
            Owner.Rotate(new Vector3(1, 1, 0), MathF.PI / 4 * deltaTime);
        }
    }
}