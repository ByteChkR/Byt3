using System.Collections.Generic;
using System.Drawing;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.IO;
using Byt3.Engine.Physics;
using Byt3.Engine.Physics.BEPUutilities;
using Byt3.Engine.Rendering;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;

namespace Byt3.Engine.Tutorials.Tutorials.Components
{
    public class MouseTrackerComponent : AbstractComponent
    {
        private LitMeshRendererComponent Last = null;
        private Texture LastTex = null;
        private Texture HitTex = TextureLoader.ColorToTexture(Color.Green);

        protected override void Awake()
        {
        }

        protected override void Update(float deltaTime)
        {
            if (ObjectUnderMouse(Owner.LocalPosition, out KeyValuePair<Collider, RayHit> hit))
            {
                LitMeshRendererComponent lmr = hit.Key.Owner.GetComponent<LitMeshRendererComponent>();
                if (Last == null)
                {
                    Last = lmr;
                    LastTex = GetTexture(lmr);

                    ApplyTexture(lmr, HitTex);
                }
                else if (lmr != Last)
                {
                    ApplyTexture(Last, LastTex);

                    Last = lmr;
                    LastTex = GetTexture(lmr);

                    ApplyTexture(lmr, HitTex);
                }
            }
            else if (Last != null)
            {
                ApplyTexture(Last, LastTex);
                Last = null;
                LastTex = null;
            }
        }

        private Texture GetTexture(LitMeshRendererComponent lmr)
        {
            for (int i = 0; i < lmr.Textures.Length; i++)
            {
                if (lmr.Textures[i].TexType == TextureType.Diffuse || lmr.Textures[i].TexType == TextureType.None)
                {
                    return lmr.Textures[i];
                }
            }

            return null;
        }

        private void ApplyTexture(LitMeshRendererComponent lmr, Texture tex)
        {
            for (int i = 0; i < lmr.Textures.Length; i++)
            {
                if (lmr.Textures[i].TexType == TextureType.Diffuse || lmr.Textures[i].TexType == TextureType.None)
                {
                    lmr.Textures[i] = tex;
                    return;
                }
            }
        }

        public static bool ObjectUnderMouse(Vector3 cameraPosition, out KeyValuePair<Collider, RayHit> hit)
        {
            Ray r = ConstructRayFromMousePosition(cameraPosition);
            bool ret = PhysicsEngine.RayCastFirst(r, 1000, LayerManager.NameToLayer("raycast"),
                out hit); //Here we are doing the raycast.

            return ret;
        }

        private static Ray ConstructRayFromMousePosition(Vector3 localPosition)
        {
            Vector2 mpos = GameEngine.Instance.MousePosition;
            Vector3 mousepos = GameEngine.Instance.ConvertScreenToWorldCoords((int) mpos.X, (int) mpos.Y);
            return new Ray(localPosition, (mousepos - localPosition).Normalized());
        }
    }
}