﻿using Byt3.Collections;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Rendering;
using Byt3.Engine.UI;
using OpenTK;

namespace Byt3.Engine.Demos.components
{
    public class UiMovingComponent : UiImageRendererComponent
    {
        private float time;

        public UiMovingComponent(Texture texture, bool worldSpace, float alpha, ShaderProgram shader) : base(
            texture, worldSpace, alpha, shader)
        {
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update(float deltaTime)
        {
            time += deltaTime;


            Position = new Vector2(MathF.Sin(time * 2), MathF.Cos(time * 2));
            float x = MathF.Abs(MathF.Sin(time * 2)) * 0.3f + 0.1f;
            float y = MathF.Abs(MathF.Cos(time * 2)) * 0.3f + 0.1f;
            Scale = new Vector2(x, y);
        }
    }
}