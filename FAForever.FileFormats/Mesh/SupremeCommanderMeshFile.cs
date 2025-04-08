using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace FAForever.FileFormats.Mesh;

/// <param name="Bones">Bone data of the mesh.</param>
/// <param name="Vertices">Vertex data of the mesh.</param>
/// <param name="Triangles">Triangle indices of the mesh.</param>
/// <param name="Information">Other, unstructured information of the mesh.</param>
[System.Serializable]
public record SupremeCommanderMeshFile(
    ReadOnlyCollection<SupremeCommanderMeshBone> Bones, 
    ReadOnlyCollection<SupremeCommanderMeshVertex> Vertices, 
    ReadOnlyCollection<SupremeCommanderMeshTriangle> Triangles, 
    ReadOnlyCollection<string> Information);
