using System;
using System.Collections.Generic;
using System.Drawing;
using Byt3.Engine.AI;
using Byt3.Engine.Core;
using Byt3.Engine.DataTypes;
using Byt3.Engine.IO;
using Byt3.Engine.Physics;
using Byt3.Engine.Physics.BEPUphysics.Entities.Prefabs;
using Byt3.Engine.Physics.BEPUutilities;
using Byt3.Engine.Rendering;
using OpenTK.Input;
using Vector2 = OpenTK.Vector2;
using Vector3 = OpenTK.Vector3;

namespace Byt3.Engine.Tutorials.Tutorials.Components
{
    public class AStarTestComponent : AbstractComponent
    {
        private readonly Texture greenTex = TextureLoader.ColorToTexture(Color.Green); //Walkable Node
        private readonly Texture purpleTex = TextureLoader.ColorToTexture(Color.Purple); //Selected Path
        private readonly Texture redTex = TextureLoader.ColorToTexture(Color.Red); //"Wall" Node
        private AiNode endNode;
        private AiNode[,] Nodes; //Nodes we use


        private List<AiNode> path;
        private AiNode startNode;

        protected override void Awake()
        {
            Nodes = GenerateNodeGraph(16, 16); //Creating the Node Graph


            Vector3
                offset = new Vector3(-16, 0,
                    -16); //Camera is looking down on Vector3.Zero. So we move the map under there
            Random rnd = new Random();
            for (int i = 0; i < Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < Nodes.GetLength(1); j++)
                {
                    GameObject box;
                    bool isBlocked = false;

                    if (rnd.Next(0, 256) < 32) //1/8 Walls
                    {
                        isBlocked = true;
                        box = CreateBox(new Vector3(i, 0, j) * 2 + offset, redTex);
                    }
                    else
                    {
                        box = CreateBox(new Vector3(i, 0, j) * 2 + offset, greenTex);
                    }

                    Nodes[i, j].Walkable = !isBlocked;
                    box.AddComponent(Nodes[i, j]); //Adding the AI Node to a Gameobject(so it can get the position)
                }
            }
        }

        private GameObject CreateBox(Vector3 position, Texture tex)
        {
            GameObject box = new GameObject(position, "Box"); //Creating a new Empty GameObject
            LitMeshRendererComponent lmr = new LitMeshRendererComponent( //Creating a Renderer Component
                DefaultFilepaths.DefaultLitShader, //The OpenGL Shader used(Unlit and Lit shaders are provided)
                Prefabs.Cube, //The Mesh that is going to be used by the MeshRenderer
                tex, //Diffuse Texture to put on the mesh
                1); //Render Mask (UI = 1 << 30)

            //Adding a collider for raycasting
            box.AddComponent(new Collider(new Box(Byt3.Engine.Physics.BEPUutilities.Vector3.Zero, 2, 2, 2), "raycast"));

            box.AddComponent(lmr); //Attaching the Renderer to the GameObject
            Owner.Scene.Add(box); //Adding the Object to the Scene.
            return box;
        }

        private AiNode[,] GenerateNodeGraph(int width, int length)
        {
            AiNode[,] nodes = new AiNode[width, length];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    AiNode node = new AiNode(true); //Just Filling the map
                    nodes[i, j] = node;
                }
            }

            //Connecting Every node with its surrounding nodes including diagonals
            // N N N
            // N 0 N
            // N N N
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

        protected override void Update(float deltaTime)
        {
            if (ObjectUnderMouse(Owner.LocalPosition, out KeyValuePair<Collider, RayHit> hit)
            ) //We Check where we clicked on
            {
                AiNode node = hit.Key.Owner.GetComponent<AiNode>();
                if (node != null)
                {
                    if (Input.GetKey(Key.S)) //Setting the Start Point
                    {
                        LitMeshRendererComponent lmr =
                            node.Owner.GetComponent<LitMeshRendererComponent>();
                        ApplyTexture(lmr, purpleTex);
                        if (startNode != null && startNode != node)
                        {
                            ApplyTexture(startNode.Owner.GetComponent<LitMeshRendererComponent>(),
                                startNode.Walkable ? greenTex : redTex);
                        }

                        startNode = node;
                    }
                    else if (Input.GetKey(Key.E)) //Setting the End Point
                    {
                        LitMeshRendererComponent lmr =
                            node.Owner.GetComponent<LitMeshRendererComponent>();
                        ApplyTexture(lmr, purpleTex);
                        if (endNode != null && endNode != node)
                        {
                            ApplyTexture(endNode.Owner.GetComponent<LitMeshRendererComponent>(),
                                endNode.Walkable ? greenTex : redTex);
                        }

                        endNode = node;
                    }
                }
            }

            //When Start and end Point is defined and space is pressed we calculate the path
            if (startNode != null && endNode != null && Input.GetKey(Key.Space))
            {
                if (path != null) //First Clean the Old path, and reset the textures
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        LitMeshRendererComponent lmr =
                            path[i].Owner.GetComponent<LitMeshRendererComponent>();
                        ApplyTexture(lmr, path[i].Walkable ? greenTex : redTex);
                    }
                }

                //This line is doing the A*
                path = AStarResolver.FindPath(startNode, endNode, out bool foundPath);


                if (foundPath) //If there exists a path from start to end, we will make it visible
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        LitMeshRendererComponent lmr =
                            path[i].Owner.GetComponent<LitMeshRendererComponent>();
                        ApplyTexture(lmr, purpleTex);
                    }
                }
            }

            //Escape Resets the Nodes color and removes the starting points
            if (Input.GetKey(Key.Escape))
            {
                if (startNode != null)
                {
                    ApplyTexture(startNode.Owner.GetComponent<LitMeshRendererComponent>(),
                        startNode.Walkable ? greenTex : redTex);
                }

                if (endNode != null)
                {
                    ApplyTexture(endNode.Owner.GetComponent<LitMeshRendererComponent>(),
                        endNode.Walkable ? greenTex : redTex);
                }

                startNode = endNode = null;
                if (path != null)
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        LitMeshRendererComponent lmr =
                            path[i].Owner.GetComponent<LitMeshRendererComponent>();
                        ApplyTexture(lmr, path[i].Walkable ? greenTex : redTex);
                    }
                }

                path = null;
            }
        }


        //Taken from Raycasting Example

        #region Raycast

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


        private static bool ObjectUnderMouse(Vector3 cameraPosition, out KeyValuePair<Collider, RayHit> hit)
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

        #endregion
    }
}