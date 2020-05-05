using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Physics;
using Byt3.Engine.Physics.BEPUphysics.Entities.Prefabs;
using Byt3.Engine.Physics.BEPUutilities;
using Byt3.Engine.Rendering;

namespace HorrorOfBindings.components.Weapons
{
    public struct Bullet
    {
        public float BulletLaunchForce { get; set; }
        public float BulletMass { get; set; }
        public bool PhysicalBullets { get; set; }
        public Mesh BulletModel { get; set; }
        public Texture BulletTexture { get; set; }
        public ShaderProgram BulletShader { get; set; }

        public GameObject CreateBullet(GameObject nozzle)
        {
            Vector3 vel =
                new OpenTK.Vector3(-OpenTK.Vector4.UnitZ * nozzle.GetWorldTransform()) * BulletLaunchForce;
            OpenTK.Vector3 v = vel;

            GameObject bullet =
                new GameObject(nozzle.LocalPosition + (Vector3) v.Normalized(),
                    "BulletEnemy");
            bullet.Rotation = nozzle.Rotation;
            bullet.AddComponent(new LitMeshRendererComponent(BulletShader, BulletModel, BulletTexture, 1, false));
            bullet.AddComponent(new DestroyTimer(5));
            bullet.Scale = new Vector3(0.3f, 0.3f, 1);
            Collider coll = new Collider(new Box(Vector3.Zero, 0.3f, 0.3f, 1, BulletMass),
                LayerManager.NameToLayer("physics"));
            if (!PhysicalBullets)
            {
                coll.IsTrigger = true;
            }

            bullet.AddComponent(coll);
            coll.PhysicsCollider.ApplyLinearImpulse(ref vel);
            return bullet;
        }
    }
}