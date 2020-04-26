using System;
using Byt3.Engine.Core;

namespace Byt3.Engine.Demos.components
{
    public class GeneralTimer : AbstractComponent
    {
        private readonly float destroyTime;
        private readonly Action onTrigger;
        private float time;

        public GeneralTimer(float destroyTime, Action onTrigger)
        {
            this.destroyTime = destroyTime;
            this.onTrigger = onTrigger;
        }


        protected override void Update(float deltaTime)
        {
            time += deltaTime;
            if (time >= destroyTime)
            {
                onTrigger?.Invoke();
            }
        }
    }
}