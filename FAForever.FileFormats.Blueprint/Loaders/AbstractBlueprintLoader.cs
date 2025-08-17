using NLua;

namespace FAForever.FileFormats.Blueprint.Loaders;

public abstract class AbstractBlueprintLoader<T>
{
    /// <summary>
    /// Reads all relevant values for a single trail blueprint instance.
    /// </summary>
    /// <param name="luaTable"></param>
    /// <returns></returns>
    public abstract T FromBlueprint(LuaTable luaTable);

    /// <summary>
    /// Interprets the table as an array of blueprint tables. Converts all tables to a trail blueprint instance.
    /// </summary>
    /// <param name="luaTable"></param>
    /// <returns></returns>
    public List<T> FromBlueprints(LuaTable luaTable) =>
        luaTable.Values
            .OfType<LuaTable>()
            .Select(FromBlueprint)
            .ToList();

    /// <summary>
    /// Converts all tables to a trail blueprint instance. 
    /// </summary>
    /// <param name="luaTables"></param>
    /// <returns></returns>
    public List<T> FromBlueprints(List<LuaTable> luaTables) =>
        luaTables.Select(FromBlueprint).ToList();
}