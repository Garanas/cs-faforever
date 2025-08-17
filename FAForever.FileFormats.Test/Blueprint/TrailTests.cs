using FAForever.FileFormats.Blueprint;

namespace FAForever.FileFormats.Test.Blueprint;

public class TrailTests
{
    public static TheoryData<string> GetTrailData()
    {
        var data = new TheoryData<string>();
        string trailsPath = Path.Combine("assets", "blueprints", "trails");
        foreach (string file in Directory.EnumerateFiles(trailsPath, "*.bp", SearchOption.AllDirectories))
        {
            data.Add(file.Replace(Path.DirectorySeparatorChar, '/'));
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(GetTrailData))]
    public void LoadBlueprint_ValidTrailBlueprint_LoadsSuccessfully(string blueprintPath)
    {
        // Arrange
        var loader = new BlueprintLoader();

        // Act
        var blueprints = loader.LoadBlueprints([new FilePath(blueprintPath)]);
    }
}