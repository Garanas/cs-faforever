using NLua;

namespace FAForever.FileFormats.Blueprint.Loaders;

public class TrailBlueprintLoader : IBlueprintLoader<TrailBlueprint>
{
    private readonly TrailBlueprint _defaults = new();

    /// <summary>
    /// Reads all relevant values for a single trail blueprint instance.
    /// </summary>
    /// <param name="luaTable"></param>
    /// <returns></returns>
    public TrailBlueprint FromBlueprint(LuaTable luaTable)
    {
        var utils = new Utils();
        return new TrailBlueprint
        {
            LifeTime = utils.GetFloat(luaTable, "LifeTime", _defaults.LifeTime),
            TrailLength = utils.GetFloat(luaTable, "TrailLength", _defaults.TrailLength),
            Size = utils.GetFloat(luaTable, "Size", _defaults.Size),
            SortOrder = utils.GetFloat(luaTable, "SortOrder", _defaults.SortOrder),
            BlendMode = utils.GetBlendMode(luaTable, "BlendMode", _defaults.BlendMode),
            TextureRepeatRate = utils.GetFloat(luaTable, "TextureRepeatRate", _defaults.TextureRepeatRate),
            RenderCutoff = utils.GetFloat(luaTable, "LODCutoff", _defaults.RenderCutoff),
            EmitIfVisible = utils.GetBool(luaTable, "EmitIfVisible", _defaults.EmitIfVisible),
            CatchupEmit = utils.GetBool(luaTable, "CatchupEmit", _defaults.CatchupEmit),
            UShift = utils.GetFloat(luaTable, "UShift", _defaults.UShift),
            VShift = utils.GetFloat(luaTable, "VShift", _defaults.VShift),
            RepeatTexture = utils.GetString(luaTable, "RepeatTexture", _defaults.RepeatTexture),
            RampTexture = utils.GetString(luaTable, "RampTexture", _defaults.RampTexture),
            RampTextureName = utils.GetString(luaTable, "RampTextureName", _defaults.RampTextureName)
        };
    }

    /// <summary>
    /// Interprets the table as an array of blueprint tables. Converts all tables to a trail blueprint instance.
    /// </summary>
    /// <param name="luaTable"></param>
    /// <returns></returns>
    public List<TrailBlueprint> FromBlueprints(LuaTable luaTable) =>
        luaTable.Values
            .OfType<LuaTable>()
            .Select(FromBlueprint)
            .ToList();

    /// <summary>
    /// Converts all tables to a trail blueprint instance. 
    /// </summary>
    /// <param name="luaTables"></param>
    /// <returns></returns>
    public List<TrailBlueprint> FromBlueprints(List<LuaTable> luaTables) =>
        luaTables.Select(FromBlueprint).ToList();
}