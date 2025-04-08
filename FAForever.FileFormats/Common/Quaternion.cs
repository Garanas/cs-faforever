using System.Runtime.InteropServices;

namespace FAForever.FileFormats.Common;

/// <summary>
/// A basic record that represents a quaternion.
/// </summary>
/// <param name="W"></param>
/// <param name="X"></param>
/// <param name="Y"></param>
/// <param name="Z"></param>
[System.Serializable]
[StructLayout(LayoutKind.Sequential)]
public record struct Quaternion( float W, float X, float Y, float Z);