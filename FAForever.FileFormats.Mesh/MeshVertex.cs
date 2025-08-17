using System.Runtime.InteropServices;
using FAForever.FileFormats.Common;

namespace FAForever.FileFormats.Mesh;

/// <summary>
/// Up to 4-bone skinning can be supported using additional indices (in conjunction with bone weights in the optional VertexExtra array)
/// <para>
/// Skinned meshes are not used in SupCom 1.0 so only mBoneIndex[0] is expected to contain a valid index.
/// </para>
/// </summary>
/// <param name="Position">Position of the vertex relative to the local origin of the mesh.</param>
/// <param name="Normal">Normals in tangent space.</param>
/// <param name="Tangent">Tangents in tangent space.</param>
/// <param name="Binormal">Binormals in tangent space.</param>
/// <param name="TexCoord1">A set of UV coordinates.</param>
/// <param name="TexCoord2">A set of UV coordinates.</param>
/// <param name="BoneId1"></param>
/// <param name="BoneId2"></param>
/// <param name="BoneId3"></param>
/// <param name="BoneId4"></param>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public record struct MeshVertex(
    Vector3 Position,
    Vector3 Normal,
    Vector3 Tangent,
    Vector3 Binormal,
    Vector2 TexCoord1,
    Vector2 TexCoord2,
    byte BoneId1,
    byte BoneId2,
    byte BoneId3,
    byte BoneId4);