namespace FAForever.FileFormats.Blueprint;

/// <summary>
/// A blueprint for a trail emitter.
/// </summary>
public record TrailBlueprint
{
    /// <summary>
    /// Lifetime of the trail emitter. If set to -1 the emitter will create a trail indefinitely.
    /// </summary>
    public float LifeTime { get; init; } = -1f;

    /// <summary>
    /// Length of the trail in ogrids. A single ogrid is equivalent to the size of a wall.
    /// </summary>
    public float TrailLength { get; init; } = 4f;

    /// <summary>
    /// The 'width' of the trail.
    /// </summary>
    public float Size { get; init; } = 0.05f;

    /// <summary>
    /// Sort order of rendering the trail in comparison to other trails and effects. This allows you to control the order that renders the effects. As an example, if you want a laser (a trail) to always render on top of condense (a regular emitter) that originates from the laser.
    /// </summary>
    public float SortOrder { get; init; } = 0;

    /// <summary>
    /// Blending mode determines how the trail is blended with the background.
    /// </summary>
    public BlendMode BlendMode { get; init; } = BlendMode.Add;

    /// <summary>
    /// How often the texture is repeated. The lower the value, the less the texture is repeated.
    /// </summary>
    public float TextureRepeatRate { get; init; } = 0.5f;

    /// <summary>
    /// The distance from the camera at which the trail will stop being rendered.
    /// </summary>
    public float RenderCutoff { get; init; } = 200f;

    /// <summary>
    /// If true, only emits when visible to the user.
    /// </summary>
    public bool EmitIfVisible { get; init; } = false;

    /// <summary>
    /// If true, once visible to the user, it catches up missing parts of the trail.
    /// </summary>
    public bool CatchupEmit { get; init; } = false;

    /// <summary>
    /// Speed at which the U component of the UV coordinates translates when looking up the texture. Shifts horizontally from the perspective of a texture and across the width of the beam.
    /// </summary>
    public float UShift { get; init; } = 0f;

    /// <summary>
    /// Speed at which the V component of the UV coordinates translates when looking up the texture. Shifts vertically from the perspective of a texture and across the length of the beam.
    /// </summary>
    public float VShift { get; init; } = 0f;

    /// <summary>
    /// The texture to use for the trail.
    /// </summary>
    public string RepeatTexture { get; init; } = string.Empty;


    /// <summary>
    /// The texture to use as a ramp to influence transparency and color. The position in the trail with respect to its length is mapped to the ramp texture for sampling. The left side of the ramp texture is when the particle is just created. The right side of the ramp texture is when the particle is at the tail of the trail.
    /// </summary>
    public string RampTexture { get; init; } = string.Empty;

    /// <summary>
    /// ???
    /// </summary>
    public string RampTextureName { get; init; } = string.Empty;
}