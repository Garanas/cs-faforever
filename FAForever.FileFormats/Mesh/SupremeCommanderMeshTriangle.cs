using System.Runtime.InteropServices;

namespace FAForever.FileFormats.Mesh;

[System.Serializable]
[StructLayout(LayoutKind.Sequential)]
public record struct SupremeCommanderMeshTriangle(short Index1, short Index2, short Index3);