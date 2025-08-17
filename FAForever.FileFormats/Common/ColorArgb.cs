namespace FAForever.FileFormats.Common;

public record ColorArgb(float A, float R, float G, float B)
{
    public static ColorArgb White => new(1, 1, 1, 1);
}