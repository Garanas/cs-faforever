using FAForever.FileFormats.Blueprint.Loaders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NLua;

namespace FAForever.FileFormats.Blueprint;

public record FilePath(string Value)
{
    public static implicit operator string(FilePath path) => path.Value;
    public static explicit operator FilePath(string path) => new(path);
}

public record FileContent(string Value)
{
    public static implicit operator string(FileContent content) => content.Value;
    public static explicit operator FileContent(string content) => new(content);
}

/// <summary>
/// Handles loading and parsing of blueprint files.
/// </summary>
/// <remarks>
/// This class uses Microsoft.Extensions.Logging.ILogger for logging operations.
/// If no logger is provided, logging will be disabled using NullLogger.
/// </remarks>
public class BlueprintLoader
{
    private readonly ILogger _logger;

    public BlueprintLoader(ILogger? logger = null)
    {
        _logger = logger ?? NullLogger.Instance;
    }

    /// <summary>
    /// Creates a Lua environment with all the globals defined that are necessary to read blueprint files.
    /// </summary>
    /// <returns></returns>
    private Lua CreateLuaEnvironment()
    {
        Lua state = new();

        // To add syntax highlighting to this chunk of Lua code, use the following in the Rider IDE:
        // - ALT + Enter -> Mark as injected language or reference -> Choose Lua

        state.DoString(
            """
            Blueprints = { 
                Beam ={},
                Emitter = {},
                Mesh = {},
                Projectile = {},
                Prop = {},
                Trail = {},
                Unit = {}
            }

            function BeamBlueprint(blueprint)
                table.insert(Blueprints.Beam, blueprint)
            end

            function EmitterBlueprint(blueprint)
                table.insert(Blueprints.Emitter, blueprint)
            end

            function MeshBlueprint(blueprint)
                table.insert(Blueprints.Mesh, blueprint)
            end

            function ProjectileBlueprint(blueprint)
                table.insert(Blueprints.Projectile, blueprint)
            end

            function PropBlueprint(blueprint)
                table.insert(Blueprints.Prop, blueprint)
            end

            function TrailEmitterBlueprint(blueprint)
                table.insert(Blueprints.Trail, blueprint)
            end

            function UnitBlueprint(blueprint)
                table.insert(Blueprints.Unit, blueprint)
            end
            """);

        return state;
    }

    /// <summary>
    /// Loads all blueprints that are defined in the list of file content.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public AllBlueprints LoadBlueprints(IEnumerable<FileContent> content)
    {
        AllBlueprints blueprints = new();

        Lua state = CreateLuaEnvironment();
        foreach (var code in content)
        {
            try
            {
                state.DoString(code);
            }
            catch (Exception e)
            {
            }
        }

        if (state["Blueprints"] is not LuaTable luaBlueprints) throw new Exception("Blueprints table not found");

        if (luaBlueprints["Trail"] is LuaTable trailBlueprints)
        {
            TrailBlueprintLoader trailBlueprintLoader = new();
            blueprints.Trails = trailBlueprintLoader.FromBlueprints(trailBlueprints);
        }

        if (luaBlueprints["Beam"] is LuaTable beamBlueprints)
        {
        }

        if (luaBlueprints["Emitter"] is LuaTable emitterBlueprints)
        {
        }

        if (luaBlueprints["Mesh"] is LuaTable meshBlueprints)
        {
        }

        if (luaBlueprints["Projectile"] is LuaTable projectileBlueprints)
        {
        }

        if (luaBlueprints["Prop"] is LuaTable propBlueprints)
        {
        }

        if (luaBlueprints["Unit"] is LuaTable unitBlueprints)
        {
        }

        return blueprints;
    }

    /// <summary>
    /// Loads all blueprints that are defined in the list of files.
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<AllBlueprints> LoadBlueprints(IEnumerable<FilePath> paths)
    {
        var contents = await Task.WhenAll(
            paths.Select(async path =>
                new FileContent(await File.ReadAllTextAsync(path))));

        return LoadBlueprints(contents);
    }
}