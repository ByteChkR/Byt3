using Byt3.Engine.Core;

namespace Byt3.Engine.Demos.components
{
    public class LookAtComponent : AbstractComponent
    {
        private GameObject target;
        public bool IsLooking => target != null;

        public void SetTarget(GameObject target)
        {
            this.target = target;
        }

        protected override void Update(float deltaTime)
        {
            if (target != null)
            {
                Owner.LookAt(target);
            }
        }
    }
}