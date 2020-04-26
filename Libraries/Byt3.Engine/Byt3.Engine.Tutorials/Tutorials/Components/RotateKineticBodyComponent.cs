using Byt3.Engine.Core;
using Byt3.Engine.Physics;
using OpenTK;

namespace Byt3.Engine.Tutorials.Tutorials.Components
{
    public class RotateKineticBodyComponent : AbstractComponent
    {
        private Collider c;

        protected override void Awake()
        {
            c = Owner.GetComponent<Collider>();
        }

        protected override void Update(float deltaTime)
        {
            //Rotating the Gameobject
            //Note: Since the collider is kinematic it has infinite mass
            //      So no force can be applied to move it.
            //      As a workaround we move the two systems manually(EngineRendering and EnginePhysics)
            Byt3.Engine.Physics.BEPUutilities.Quaternion v = Quaternion.FromAxisAngle(new Vector3(1, 1, 1), deltaTime);
            c.PhysicsCollider.Orientation *= v;
            Owner.Rotate(new Vector3(1, 1, 1), deltaTime);
        }
    }
}