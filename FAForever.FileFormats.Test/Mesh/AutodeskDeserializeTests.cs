using FAForever.FileFormats.Mesh;
using FAForever.FileFormats.Mesh.Serialization;

namespace FAForever.FileFormats.Test.Mesh;

public class AutodeskDeserializeTests
{
    private const string _srl0310LOD0 = "Assets/Meshes/3DSMax/srl0310_lod0.scm";
    private const string _srl0310LOD1 = "Assets/Meshes/3DSMax/srl0310_lod1.scm";
    private const string _srl0310LOD2 = "Assets/Meshes/3DSMax/srl0310_lod2.scm";
    private const string _srl0310LOD3 = "Assets/Meshes/3DSMax/srl0310_lod3.scm";

    private readonly LargeFileFixture _fixture =
        new LargeFileFixture([_srl0310LOD0, _srl0310LOD1, _srl0310LOD2, _srl0310LOD3]);

    [Theory]
    [InlineData(_srl0310LOD0)]
    [InlineData(_srl0310LOD1)]
    [InlineData(_srl0310LOD2)]
    [InlineData(_srl0310LOD3)]
    public void Pulsar(String scmFile)
    {
        using var stream = _fixture.GetFileStream(scmFile);

        BinaryMeshSerializer serializer = new BinaryMeshSerializer();
        MeshFile mesh = serializer.DeserializeSupremeCommanderModelFile(stream);
    }
}