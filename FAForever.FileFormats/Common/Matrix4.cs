using System.Runtime.InteropServices;

namespace FAForever.FileFormats.Common;

/// <summary>
/// A basic struct that represents a 4x4 row-orientated matrix.
/// </summary>
/// <param name="Row1"></param>
/// <param name="Row2"></param>
/// <param name="Row3"></param>
/// <param name="Row4"></param>
[System.Serializable]
[StructLayout(LayoutKind.Sequential)]
public record struct Matrix4(Vector4 Row1, Vector4 Row2, Vector4 Row3, Vector4 Row4);