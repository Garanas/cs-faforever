using System.Runtime.InteropServices;

namespace FAForever.FileFormats.Common;

/// <summary>
/// A basic record struct that represents a vector with 3 fields.
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
/// <param name="Z"></param>
[System.Serializable]
[StructLayout(LayoutKind.Sequential)]
public record struct Vector3(float X, float Y, float Z);