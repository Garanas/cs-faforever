using FAForever.FileFormats.Common;

namespace FAForever.FileFormats.Blueprint;

public record BeamBlueprint
{
    /// <summary>
    /// Length of the beam in ogrids. A single ogrid is equivalent to the size of a wall. The length is only relevant when the beam is created in isolation.
    /// </summary>
    public float Length { get; init; } = -1;

    /// <summary>
    /// Lifetime of the beam.
    /// </summary>
    public float LifeTime { get; init; } = -1;

    /// <summary>
    /// Width of the beam in ogrids. A single ogrid is equivalent to the size of a wall.
    /// </summary>
    public float Thickness { get; init; } = -1;

    /// <summary>
    /// The texture of the beam.
    /// </summary>
    public string TextureName { get; init; } = string.Empty;

    /// <summary>
    /// Initial color of the beam. Interpolates to (1,1,1,1) towards the center of the beam.
    /// </summary>
    public ColorArgb StartColor { get; init; } = ColorArgb.White;

    /// <summary>
    /// Final color of the beam. Interpolates from (1,1,1,1) start at the center of the beam towards the end of the beam.
    /// </summary>
    public ColorArgb EndColor { get; init; } = ColorArgb.White;

    /// <summary>
    /// Speed at which the U component of the UV coordinates translates when looking up the texture. Shifts horizontally from the perspective of a texture and across the width of the beam.
    /// </summary>
    public float UShift { get; init; } = 0f;

    /// <summary>
    /// Speed at which the V component of the UV coordinates translates when looking up the texture. Shifts vertically from the perspective of a texture and across the length of the beam.
    /// </summary>
    public float VShift { get; init; } = 0f;

    /// <summary>
    /// How often the texture is repeated. The lower the value, the less the texture is repeated.
    /// </summary>
    public float RepeatRate { get; init; } = 0.5f;

    /// <summary>
    /// Blending mode determines how the beam blends with the background.
    /// </summary>
    public BlendMode BlendMode { get; init; } = BlendMode.AlphaBlend;
}