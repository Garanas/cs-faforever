using System.Runtime.InteropServices;

namespace FAForever.FileFormats.Common;

/// <summary>
/// A basic record struct that represents a vector with 4 fields.
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
/// <param name="Z"></param>
/// <param name="W"></param>
[System.Serializable]
[StructLayout(LayoutKind.Sequential)]
public record Vector4(float X, float Y, float Z, float W);