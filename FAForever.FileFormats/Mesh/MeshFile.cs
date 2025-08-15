using System.Collections.ObjectModel;

namespace FAForever.FileFormats.Mesh;

/// <param name="Bones">Bone data of the mesh.</param>
/// <param name="Vertices">Vertex data of the mesh.</param>
/// <param name="Triangles">Triangle indices of the mesh.</param>
/// <param name="Information">Other, unstructured information of the mesh.</param>
[Serializable]
public record MeshFile(
    ReadOnlyCollection<MeshBone> Bones,
    ReadOnlyCollection<MeshVertex> Vertices,
    ReadOnlyCollection<MeshTriangle> Triangles,
    ReadOnlyCollection<string> Information);