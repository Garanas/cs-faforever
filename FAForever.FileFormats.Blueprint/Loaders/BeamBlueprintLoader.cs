using NLua;

namespace FAForever.FileFormats.Blueprint.Loaders;

public class BeamBlueprintLoader : AbstractBlueprintLoader<BeamBlueprint>
{
    private readonly BeamBlueprint _defaults = new();

    public override BeamBlueprint FromBlueprint(LuaTable luaTable)
    {
        var utils = new Utils();
        return new BeamBlueprint
        {
            Length = utils.GetFloat(luaTable, "Length", _defaults.Length),
            LifeTime = utils.GetFloat(luaTable, "LifeTime", _defaults.LifeTime),
            Thickness = utils.GetFloat(luaTable, "Thickness", _defaults.Thickness),
            TextureName = utils.GetString(luaTable, "TextureName", _defaults.TextureName),
            StartColor = utils.GetColorArgb(luaTable, "StartColor", _defaults.StartColor),
            EndColor = utils.GetColorArgb(luaTable, "EndColor", _defaults.EndColor),
            UShift = utils.GetFloat(luaTable, "UShift", _defaults.UShift),
            VShift = utils.GetFloat(luaTable, "VShift", _defaults.VShift),
            RepeatRate = utils.GetFloat(luaTable, "RepeatRate", _defaults.RepeatRate),

            // note the typo 'Blendmode' instead of 'BlendMode'
            BlendMode = utils.GetBlendMode(luaTable, "Blendmode", _defaults.BlendMode)
        };
    }
}