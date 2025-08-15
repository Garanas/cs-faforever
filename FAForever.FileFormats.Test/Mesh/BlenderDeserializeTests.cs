using FAForever.FileFormats.Mesh;
using FAForever.FileFormats.Mesh.Serialization;

namespace FAForever.FileFormats.Test.Mesh;

public class BlenderDeserializeTests
{
    private const string _url0302_lod0 = "Assets/Meshes/Blender/url0302_lod0.scm";
    private const string _url0302_lod1 = "Assets/Meshes/Blender/url0302_lod1.scm";
    private const string _xnb0101_lod0 = "Assets/Meshes/Blender/xnb0101_LOD0.scm";

    private readonly LargeFileFixture _fixture = new LargeFileFixture([_url0302_lod0, _url0302_lod1, _xnb0101_lod0]);

    [Theory]
    [InlineData(_xnb0101_lod0)]
    public void Nomads(String scmFile)
    {
        using var stream = _fixture.GetFileStream(scmFile);

        BinaryMeshSerializer serializer = new BinaryMeshSerializer();
        MeshFile mesh = serializer.DeserializeSupremeCommanderModelFile(stream);
    }

    [Theory]
    [InlineData(_url0302_lod0)]
    [InlineData(_url0302_lod1)]
    public void Madmax(String scmFile)
    {
        using var stream = _fixture.GetFileStream(scmFile);

        BinaryMeshSerializer serializer = new BinaryMeshSerializer();
        MeshFile mesh = serializer.DeserializeSupremeCommanderModelFile(stream);
    }
}