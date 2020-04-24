using Byt3.Engine.Core;

namespace Byt3.Engine.Demos.components
{
    public class DestroyTimer : AbstractComponent
    {
        private float destroyTime;
        private float time;

        public DestroyTimer(float destroyTime)
        {
            this.destroyTime = destroyTime;
        }


        protected override void Update(float deltaTime)
        {
            time += deltaTime;
            if (time >= destroyTime)
            {
                Owner.Destroy();
            }
        }
    }
}