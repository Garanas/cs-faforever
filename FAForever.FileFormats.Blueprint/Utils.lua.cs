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

    public BlendMode GetBlendMode(LuaTable table, string key, BlendMode defaultValue)
    {
        if (table[key] is long value && Enum.IsDefined(typeof(BlendMode), value))
            return (BlendMode)value;
        return defaultValue;
    }
}