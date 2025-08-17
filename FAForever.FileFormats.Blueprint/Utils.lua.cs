using FAForever.FileFormats.Common;
using NLua;

namespace FAForever.FileFormats.Blueprint;

public class Utils
{
    public float GetFloat(LuaTable table, string key, float defaultValue)
    {
        if (table[key] is double value)
            return (float)value;
        return defaultValue;
    }

    public bool GetBool(LuaTable table, string key, bool defaultValue)
    {
        if (table[key] is bool value)
            return value;
        return defaultValue;
    }

    public string GetString(LuaTable table, string key, string defaultValue)
    {
        return table[key] as string ?? defaultValue;
    }

    public ColorArgb GetColorArgb(LuaTable table, string key, ColorArgb defaultValue)
    {
        return new ColorArgb(
            A: GetFloat(table, "A", defaultValue.A),
            R: GetFloat(table, "R", defaultValue.R),
            G: GetFloat(table, "G", defaultValue.G),
            B: GetFloat(table, "B", defaultValue.B));
    }

    public BlendMode GetBlendMode(LuaTable table, string key, BlendMode defaultValue)
    {
        if (table[key] is long value && Enum.IsDefined(typeof(BlendMode), value))
            return (BlendMode)value;
        return defaultValue;
    }
}