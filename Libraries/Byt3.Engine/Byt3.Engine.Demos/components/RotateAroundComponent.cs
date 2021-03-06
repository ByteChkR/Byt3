﻿using Byt3.Collections;
using Byt3.Engine.Core;
using OpenTK;

namespace Byt3.Engine.Demos.components
{
    public class RotateAroundComponent : AbstractComponent
    {
        public float Slow = 1f;

        protected override void Update(float deltaTime)
        {
            Vector4 pos = new Vector4(Owner.GetLocalPosition());
            pos *= Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), MathF.PI / 4 * deltaTime * Slow);
            Owner.SetLocalPosition(new Vector3(pos));
        }
    }
}