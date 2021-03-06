﻿using System.Collections.Generic;
using Byt3.Engine.IO;
using OpenTK;

namespace Byt3.Engine.DataTypes
{
    public class MeshBuilder
    {
        private readonly List<uint> indices = new List<uint>();
        private readonly List<Vertex> vertices = new List<Vertex>();

        private static int MeshIndex;

        public void AddVertex(Vertex v)
        {
            vertices.Add(v);
            indices.Add((uint) indices.Count);
        }

        public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            AddVertex(new Vertex {Position = v1});
            AddVertex(new Vertex {Position = v2});
            AddVertex(new Vertex {Position = v3});
        }


        public Mesh ToMesh()
        {
            MeshLoader.setupMesh(indices.ToArray(), vertices.ToArray(), out int vao, out int vbo, out int ebo);
            long bytes = sizeof(uint) * indices.Count + Vertex.VERTEX_BYTE_SIZE * vertices.Count;
            return new Mesh(ebo, vbo, vao, indices.Count, bytes, false, "MeshFromMeshBuilder+" + MeshIndex++);
        }
    }
}