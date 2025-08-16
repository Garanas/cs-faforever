using System.Runtime.InteropServices;

namespace FAForever.FileFormats.Mesh;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public record struct MeshTriangle(short Index1, short Index2, short Index3);