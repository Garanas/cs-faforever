using System.Runtime.InteropServices;

namespace FAForever.FileFormats.Mesh;

/// <summary>
/// Section offsets in the file header point to the start of the data for that section (ie, the first byte AFTER
/// the section's identifying FOURCC) Padding characters are added to the end of each section to ensure that 
/// the next section is 16-byte aligned. Ommitted sections are indicated by an offset of 0. 
/// <para>
/// All offsets are relative to the start of the file.
/// </para>
/// </summary>
/// <param name="Marker"></param>
/// <param name="Version">Version number of the SCM file.</param>
/// <param name="BoneOffset">Offset at which the bone data starts. Offset is relative to the start of the stream.</param>
/// <param name="WeightedBoneCount">The number of bones that influence vertices.</param>
/// <param name="VertexOffset">Offset at which the vertex data starts. Offset is relative to the start of the stream.</param>
/// <param name="ExtraVertexOffset">Offset at which the extra vertex data starts. This is not used in Supreme Commander or Supreme Commander: Forged alliance.</param>
/// <param name="VertexCount">Number of vertices</param>
/// <param name="IndexOffset">Offset at which the triangle index data starts. Offset is relative to the start of the stream.</param>
/// <param name="IndexCount">Number of indices.</param>
/// <param name="InformationOffset">Offset at which the additional information starts. Offset is relative to the start of the stream.</param>
/// <param name="InfoSizeInBytes"></param>
/// <param name="BoneCount">Number of bones.</param>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public record struct MeshFileHeader(
    int Marker,
    int Version,
    int BoneOffset,
    int WeightedBoneCount,
    int VertexOffset,
    int ExtraVertexOffset,
    int VertexCount,
    int IndexOffset,
    int IndexCount,
    int InformationOffset,
    int InfoSizeInBytes,
    int BoneCount);