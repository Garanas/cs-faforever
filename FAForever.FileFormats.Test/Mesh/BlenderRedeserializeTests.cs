using FAForever.FileFormats.Mesh;
using FAForever.FileFormats.Mesh.Serialization;

namespace FAForever.FileFormats.Test.Mesh;

public class BlenderRedeserializeTests
{
    private const string _url0302_lod0 = "Assets/Meshes/Blender/url0302_lod0.scm";
    private const string _url0302_lod1 = "Assets/Meshes/Blender/url0302_lod1.scm";
    private const string _xnb0101_lod0 = "Assets/Meshes/Blender/xnb0101_LOD0.scm";

    private readonly LargeFileFixture _fixture = new LargeFileFixture([_url0302_lod0, _url0302_lod1, _xnb0101_lod0]);

    [Theory]
    [InlineData(_xnb0101_lod0)]
    public void Nomads(String scmFile)
    {
        BinaryMeshSerializer serializer = new BinaryMeshSerializer();

        using var fileStream = _fixture.GetFileStream(scmFile);
        MeshFile expectedMesh = serializer.DeserializeSupremeCommanderModelFile(fileStream);
        using var intermediateStream = serializer.SerializeSupremeCommanderModelFile(expectedMesh);
        intermediateStream.Position = 0;
        MeshFile actualMesh = serializer.DeserializeSupremeCommanderModelFile(intermediateStream);

        // compare the mesh
        Assert.True(expectedMesh.Bones.SequenceEqual(actualMesh.Bones));
        Assert.True(expectedMesh.Vertices.SequenceEqual(actualMesh.Vertices));
        Assert.True(expectedMesh.Triangles.SequenceEqual(actualMesh.Triangles));
        Assert.True(expectedMesh.Information.SequenceEqual(actualMesh.Information));
    }

    [Theory]
    [InlineData(_url0302_lod0)]
    [InlineData(_url0302_lod1)]
    public void Madmax(String scmFile)
    {
        BinaryMeshSerializer serializer = new BinaryMeshSerializer();

        using var fileStream = _fixture.GetFileStream(scmFile);
        MeshFile expectedMesh = serializer.DeserializeSupremeCommanderModelFile(fileStream);
        using var intermediateStream = serializer.SerializeSupremeCommanderModelFile(expectedMesh);
        intermediateStream.Position = 0;
        MeshFile actualMesh = serializer.DeserializeSupremeCommanderModelFile(intermediateStream);

        // compare the mesh
        Assert.True(expectedMesh.Bones.SequenceEqual(actualMesh.Bones));
        Assert.True(expectedMesh.Vertices.SequenceEqual(actualMesh.Vertices));
        Assert.True(expectedMesh.Triangles.SequenceEqual(actualMesh.Triangles));
        Assert.True(expectedMesh.Information.SequenceEqual(actualMesh.Information));
    }
}