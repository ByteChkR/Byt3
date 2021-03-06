﻿using System;
using System.Collections.Generic;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.IO;
using Byt3.Engine.Physics;
using Byt3.Engine.Physics.BEPUphysics.Entities.Prefabs;
using Byt3.Engine.Rendering;
using OpenTK;

namespace Byt3.Engine.Demos.components
{
    public class PhysicsDemoComponent : AbstractComponent
    {
        private readonly Mesh box = MeshLoader.FileToMesh("assets/models/cube_flat.obj");

        private readonly List<GameObject> collider = new List<GameObject>();
        private readonly int game;
        private readonly Mesh sphere = MeshLoader.FileToMesh("assets/models/sphere_smooth.obj");

        public PhysicsDemoComponent()
        {
            game = LayerManager.NameToLayer("physics");
        }

        protected override void Awake()
        {
            base.Awake();
            DebugConsoleComponent comp = Owner.Scene.GetChildWithName("Console")
                .GetComponent<DebugConsoleComponent>();


            comp?.AddCommand("rain", cmd_SpawnColliders);
            comp?.AddCommand("gravity", cmd_SetGravity);
            comp?.AddCommand("reset", cmd_ResetCollider);
        }

        protected override void OnDestroy()
        {
        }


        public static string cmd_SetGravity(string[] args)
        {
            if (args.Length != 3)
            {
                return "Expected 3 component vector";
            }

            if (float.TryParse(args[0], out float x))
            {
                return "Wrong X Component";
            }

            if (float.TryParse(args[1], out float y))
            {
                return "Wrong Y Component";
            }

            if (float.TryParse(args[2], out float z))
            {
                return "Wrong Z Component";
            }

            return "Gravity Set to: ";
        }

        private string cmd_ResetCollider(string[] args)
        {
            int count = collider.Count;
            foreach (GameObject gameObject in collider)
            {
                gameObject.Destroy();
            }

            collider.Clear();
            string ret = "";
            return ret + "\nReloaded " + count + " Colliders";
        }

        public string cmd_SpawnColliders(string[] args)
        {
            if (args.Length != 1 || !int.TryParse(args[0], out int nmbrs))
            {
                return "Not a number.";
            }


            Random rnd = new Random();
            for (int i = 0; i < nmbrs; i++)
            {
                Vector3 pos = new Vector3((float) rnd.NextDouble(), 3 + (float) rnd.NextDouble(),
                    (float) rnd.NextDouble());
                pos -= Vector3.One * 0.5f;
                pos *= 50;


                GameObject obj = new GameObject(pos, "Sphere");
                float radius = 1f + (float) rnd.NextDouble();
                obj.AddComponent(new DestroyTimer(55));
                obj.Scale = new Vector3(radius / 2);
                if (rnd.Next(0, 2) == 1)
                {
                    obj.AddComponent(new MeshRendererComponent(DefaultFilepaths.DefaultUnlitShader, box.Copy(),
                        TextureLoader.FileToTexture("assets/textures/TEST.png"), 1));


                    Collider coll = new Collider(new Box(Vector3.Zero, radius, radius, radius, 1), game);
                    obj.AddComponent(coll);
                }
                else
                {
                    obj.AddComponent(new MeshRendererComponent(DefaultFilepaths.DefaultUnlitShader, sphere.Copy(),
                        TextureLoader.FileToTexture("assets/textures/TEST.png"), 1));
                    Collider coll = new Collider(new Sphere(Vector3.Zero, radius, 1), game);
                    obj.AddComponent(coll);
                }

                collider.Add(obj);
                Owner.Scene.Add(obj);
            }


            return nmbrs + " Objects Created.";
        }
    }
}