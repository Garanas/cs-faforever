using System.Runtime.InteropServices;
using FAForever.FileFormats.Common;

namespace FAForever.FileFormats.Mesh;

/// <summary>
/// Represents a single bone in the mesh hierarchy, containing transformation data relative to its parent
/// and metadata such as name and parent index.
/// </summary>
/// <param name="Name">
/// The name of the bone. This corresponds to a string referenced via offset in the original file.
/// </param>
/// <param name="RestPoseInverse">
/// The inverse bind pose matrix of the bone relative to the mesh's local origin. This is a 4×4 row-major matrix,
/// used to transform vertices from model space to bone space.
/// </param>
/// <param name="Position">
/// The position of the bone relative to its parent, represented as a 3D vector (X, Y, Z).
/// </param>
/// <param name="Rotation">
/// The rotation of the bone relative to its parent, represented as a quaternion (W, X, Y, Z).
/// </param>
/// <param name="BoneParent">
/// The index of the parent bone in the bone array. A value of -1 typically indicates a root bone.
/// </param>
/// <param name="Reserved1">
/// Reserved bytes that are unused in Supreme Commander: Forged Alliance.
/// </param>
/// <param name="Reserved2">
/// Reserved bytes that are unused in Supreme Commander: Forged Alliance.
/// </param>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public record struct MeshBone(
    string Name,
    Matrix4 RestPoseInverse,
    Vector3 Position,
    Quaternion Rotation,
    int BoneParent,
    int Reserved1,
    int Reserved2);