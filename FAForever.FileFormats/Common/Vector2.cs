using System.Runtime.InteropServices;

namespace FAForever.FileFormats.Common;

/// <summary>
/// A basic record struct that represents a vector with 2 fields.
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
[System.Serializable]
[StructLayout(LayoutKind.Sequential)]
public record struct Vector2(float X, float Y);