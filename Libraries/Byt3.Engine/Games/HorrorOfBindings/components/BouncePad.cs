using Byt3.Engine.Core;
using Byt3.Engine.Physics;
using Byt3.Engine.Physics.BEPUphysics.CollisionTests;
using Byt3.Engine.Physics.BEPUphysics.NarrowPhaseSystems.Pairs;
using Byt3.Engine.Physics.BEPUutilities;

namespace HorrorOfBindings.components
{
    public class BouncePad : AbstractComponent
    {
        protected override void OnContactCreated(Collider other, CollidablePairHandler handler, ContactData contact)
        {
            if ((other.Owner.Name == "Player" || other.Owner.Name == "Enemy") && handler.Contacts.Count == 1
            ) //Only if thats the first contact
            {
                Vector3 force = Vector3.UnitY * 5000;
                other.PhysicsCollider.ApplyLinearImpulse(ref force);
            }
        }
    }
}