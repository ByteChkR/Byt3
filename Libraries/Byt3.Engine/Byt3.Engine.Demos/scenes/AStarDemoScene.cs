﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Byt3.Engine.AI;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.Debug;
using Byt3.Engine.Demos.components;
using Byt3.Engine.IO;
using Byt3.Engine.Physics;
using Byt3.Engine.Physics.BEPUphysics.Entities.Prefabs;
using Byt3.Engine.Rendering;
using OpenTK;

namespace Byt3.Engine.Demos.scenes
{
    public class AStarDemoScene : AbstractScene
    {
        private Texture beginTex;
        private Texture blockTex;
        private Texture endTex;
        private AiNode[,] nodes;
        private List<AiNode> path;
        private Texture tex;

        protected override void InitializeScene()
        {
            int rayLayer = LayerManager.RegisterLayer("raycast", new Layer(1, 2));
            int hybLayer = LayerManager.RegisterLayer("hybrid", new Layer(1, 1 | 2));
            int physicsLayer = LayerManager.RegisterLayer("physics", new Layer(1, 1));
            LayerManager.DisableCollisions(rayLayer, physicsLayer);

            BasicCamera mainCamera =
                new BasicCamera(
                    Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(75f),
                        GameEngine.Instance.Width / (float) GameEngine.Instance.Height, 0.01f, 1000f), Vector3.Zero);

            object mc = mainCamera;

            EngineConfig.LoadConfig("assets/configs/camera_astardemo.xml", ref mc);

            GameObject sourceCube = new GameObject(new Vector3(0, 0, 0), "Light Source");
            GameObject hackCube = new GameObject(new Vector3(0, 8, -50), "Workaround");
            Add(sourceCube);
            Add(hackCube);
            sourceCube.AddComponent(new LightComponent());


            mainCamera.AddComponent(new CameraRaycaster(sourceCube, hackCube));

            GameObject bgObj = new GameObject(new Vector3(0, -3, -32), "BG") {Scale = new Vector3(32, 1, 32)};

            Collider groundCol = new Collider(new Box(Vector3.Zero, 64, 1, 64), hybLayer);
            Texture bgTex = TextureLoader.ColorToTexture(Color.Yellow);
            bgTex.TexType = TextureType.Diffuse;
            bgObj.AddComponent(groundCol);

            tex = TextureLoader.ColorToTexture(Color.Green);
            beginTex = TextureLoader.ColorToTexture(Color.Blue);
            endTex = TextureLoader.ColorToTexture(Color.Red);
            blockTex = TextureLoader.ColorToTexture(Color.DarkMagenta);
            tex.TexType = beginTex.TexType = endTex.TexType = blockTex.TexType = TextureType.Diffuse;

            DebugConsoleComponent c = DebugConsoleComponent.CreateConsole().GetComponent<DebugConsoleComponent>();

            c.AddCommand("repath", ResetPaths);
            c.AddCommand("rp", ResetPaths);

            Add(c.Owner);

            bgObj.AddComponent(new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader, Prefabs.Cube, bgTex, 1));
            Add(bgObj);
            Add(mainCamera);
            SetCamera(mainCamera);

            Random rnd = new Random();
            nodes = GenerateNodeGraph(64, 64);
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                for (int j = 0; j < nodes.GetLength(1); j++)
                {
                    if (rnd.Next(0, 6) == 0)
                    {
                        nodes[i, j].Walkable = false;
                        nodes[i, j].Owner
                            .AddComponent(new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader,
                                Prefabs.Sphere, blockTex, 1));
                    }

                    nodes[i, j].Owner.Scale *= 0.3f;

                    Add(nodes[i, j].Owner);
                }
            }
        }


        private string ResetPaths(string[] args)
        {
            if (path != null)
            {
                foreach (AiNode aiNode in path)
                {
                    if (aiNode.Walkable)
                    {
                        aiNode.Owner.RemoveComponent<MeshRendererComponent>();
                    }
                }

                path.Clear();
            }

            Random rnd = new Random();
            AiNode startNode = nodes[rnd.Next(0, 64), rnd.Next(0, 64)];
            AiNode endNode = nodes[rnd.Next(0, 64), rnd.Next(0, 64)];
            while (!startNode.Walkable)
            {
                startNode = nodes[rnd.Next(0, 64), rnd.Next(0, 64)];
            }

            while (!endNode.Walkable)
            {
                endNode = nodes[rnd.Next(0, 64), rnd.Next(0, 64)];
            }

            path = AStarResolver.FindPath(startNode, endNode, out bool found);

            for (int i = 0; i < path.Count; i++)
            {
                path[i].Owner
                    .AddComponent(new LitMeshRendererComponent(DefaultFilepaths.DefaultLitShader, Prefabs.Cube, tex,
                        1));
            }

            startNode.Owner.GetComponent<LitMeshRendererComponent>().Textures[0] = beginTex;
            endNode.Owner.GetComponent<LitMeshRendererComponent>().Textures[0] = endTex;
            return "Success: " + found;
        }

        private AiNode[,] GenerateNodeGraph(int width, int length)
        {
            AiNode[,] nodes = new AiNode[width, length];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    GameObject obj = new GameObject("NodeW" + i + "L:" + j)
                    {
                        LocalPosition = new Byt3.Engine.Physics.BEPUutilities.Vector3(i - width / 2, 0, -j)
                    };
                    AiNode node = new AiNode(true);
                    obj.AddComponent(node);
                    nodes[i, j] = node;
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    AiNode current = nodes[i, j];
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int s = -1; s <= 1; s++)
                        {
                            if (i + k < 0 || i + k >= width || j + s < 0 || j + s >= length || k == 0 && s == 0)
                            {
                                continue;
                            }

                            current.AddConnection(nodes[i + k, j + s]);
                        }
                    }
                }
            }

            return nodes;
        }
    }
}