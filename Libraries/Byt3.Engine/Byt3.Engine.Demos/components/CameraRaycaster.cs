using System.Collections.Generic;
using Byt3.Engine.Core;
using Byt3.Engine.Physics;
using Byt3.Engine.Physics.BEPUutilities;
using OpenTK.Input;
using Vector3 = OpenTK.Vector3;

namespace Byt3.Engine.Demos.components
{
    public class CameraRaycaster : AbstractComponent
    {
        private readonly int cast;
        private readonly GameObject looker;
        private readonly GameObject sphereTargetMarker;


        public CameraRaycaster(GameObject targetmarker, GameObject looker)
        {
            cast = LayerManager.NameToLayer("raycast");
            sphereTargetMarker = targetmarker;
            this.looker = looker;
        }

        protected override void Update(float deltaTime)
        {
            Ray r = ConstructRayFromMousePosition();
            bool ret = PhysicsEngine.RayCastFirst(r, 1000, cast,
                out KeyValuePair<Collider, RayHit> arr);
            if (ret)
            {
                Vector3 pos = arr.Value.Location;
                pos.Y = looker.LocalPosition.Y;
                sphereTargetMarker.SetLocalPosition(pos);
                looker.LookAt(sphereTargetMarker);
            }
        }

        protected override void OnKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.B)
            {
                Ray r = ConstructRayFromMousePosition();
                bool ret = PhysicsEngine.RayCastFirst(r, 1000, cast,
                    out KeyValuePair<Collider, RayHit> arr);
                if (ret)
                {
                    Vector3 pos = arr.Value.Location;
                    pos.Y += looker.LocalPosition.Y;
                    sphereTargetMarker.SetLocalPosition(pos);
                    looker.LookAt(sphereTargetMarker);
                }
            }
        }

        private Ray ConstructRayFromMousePosition()
        {
            Vector2 mpos = GameEngine.Instance.MousePosition;
            Vector3 mousepos = GameEngine.Instance.ConvertScreenToWorldCoords((int) mpos.X, (int) mpos.Y);
            return new Ray(Owner.GetLocalPosition(), (mousepos - Owner.GetLocalPosition()).Normalized());
        }
    }
}