using NLua;

namespace FAForever.FileFormats.Blueprint.Loaders;

public interface IBlueprintLoader<T>
{
    /// <summary>
    /// Reads all relevant values for a single trail blueprint instance.
    /// </summary>
    /// <param name="luaTable"></param>
    /// <returns></returns>
    T FromBlueprint(LuaTable luaTable);

    /// <summary>
    /// Interprets the table as an array of blueprint tables. Converts all tables to a trail blueprint instance.
    /// </summary>
    /// <param name="luaTable"></param>
    /// <returns></returns>
    List<T> FromBlueprints(LuaTable luaTable);

    /// <summary>
    /// Converts all tables to a trail blueprint instance. 
    /// </summary>
    /// <param name="luaTables"></param>
    /// <returns></returns>
    List<TrailBlueprint> FromBlueprints(List<LuaTable> luaTables);
}