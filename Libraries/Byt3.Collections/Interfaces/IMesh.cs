using System;
using System.Collections.Generic;

namespace Byt3.Collections.Interfaces
{
    public interface IMesh
    {
        List<IVec3> vertices { get; set; }
        List<IVec3> normals { get; set; }
        List<IVec2> uvs { get; set; }
        List<Tuple<int,int,int>> indices { get; set; }

        IMesh GetEmptyMesh();

    }
}
