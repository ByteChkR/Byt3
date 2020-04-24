using Byt3.Engine.Core;

namespace Byt3.Engine.Tutorials.Tutorials.Components
{
    public class TimeoutComponent : AbstractComponent
    {
        private float tMax;
        private float t;
        public TimeoutComponent(float timeout)
        {
            tMax = timeout;
        }

        protected override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (t >= tMax)
            {
                GameEngine.Instance.Exit();
            }
            else
            {
                t += deltaTime;
            }
        }
    }
}