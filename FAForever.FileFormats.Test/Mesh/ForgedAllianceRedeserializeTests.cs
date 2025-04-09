using FAForever.FileFormats.Mesh;
using FAForever.FileFormats.Mesh.Serialization;

namespace FAForever.FileFormats.Test.Mesh;

public class ForgedAllianceRedeserializeTests
{
    private const string _ual0101LOD0 = "Assets/Meshes/ForgedAlliance/ual0101_lod0.scm";
    private const string _ual0101LOD1 = "Assets/Meshes/ForgedAlliance/ual0101_lod1.scm";
    private const string _uel0401LOD0 = "Assets/Meshes/ForgedAlliance/uel0401_lod0.scm";
    private const string _uel0401LOD1 = "Assets/Meshes/ForgedAlliance/uel0401_lod1.scm";
    private const string _xsl0001LOD0 = "Assets/Meshes/ForgedAlliance/xsl0001_lod0.scm";
    private const string _xsl0001LOD1 = "Assets/Meshes/ForgedAlliance/xsl0001_lod1.scm";

    private readonly LargeFileFixture _fixture = new LargeFileFixture([
        _ual0101LOD0, _ual0101LOD1, _uel0401LOD0, _uel0401LOD1, _xsl0001LOD0, _xsl0001LOD1
    ]);

    [Theory]
    [InlineData(_ual0101LOD0)]
    [InlineData(_ual0101LOD1)]
    public void Aeon(string scmFile)
    {
        BinarySupremeCommanderMeshSerializer serializer = new BinarySupremeCommanderMeshSerializer();

        using var fileStream = _fixture.GetFileStream(scmFile);
        SupremeCommanderMeshFile expectedMesh = serializer.DeserializeSupremeCommanderModelFile(fileStream);
        using var intermediateStream = serializer.SerializeSupremeCommanderModelFile(expectedMesh);
        intermediateStream.Position = 0;
        SupremeCommanderMeshFile actualMesh = serializer.DeserializeSupremeCommanderModelFile(intermediateStream);

        // compare the mesh
        Assert.True(expectedMesh.Bones.SequenceEqual(actualMesh.Bones));
        Assert.True(expectedMesh.Vertices.SequenceEqual(actualMesh.Vertices));
        Assert.True(expectedMesh.Triangles.SequenceEqual(actualMesh.Triangles));
        Assert.True(expectedMesh.Information.SequenceEqual(actualMesh.Information));
    }

    [Theory]
    [InlineData(_uel0401LOD0)]
    [InlineData(_uel0401LOD1)]
    public void UEF(string scmFile)
    {
        BinarySupremeCommanderMeshSerializer serializer = new BinarySupremeCommanderMeshSerializer();

        using var fileStream = _fixture.GetFileStream(scmFile);
        SupremeCommanderMeshFile expectedMesh = serializer.DeserializeSupremeCommanderModelFile(fileStream);
        using var intermediateStream = serializer.SerializeSupremeCommanderModelFile(expectedMesh);
        intermediateStream.Position = 0;
        SupremeCommanderMeshFile actualMesh = serializer.DeserializeSupremeCommanderModelFile(intermediateStream);

        // compare the mesh
        Assert.True(expectedMesh.Bones.SequenceEqual(actualMesh.Bones));
        Assert.True(expectedMesh.Vertices.SequenceEqual(actualMesh.Vertices));
        Assert.True(expectedMesh.Triangles.SequenceEqual(actualMesh.Triangles));
        Assert.True(expectedMesh.Information.SequenceEqual(actualMesh.Information));
    }

    [Theory]
    [InlineData(_xsl0001LOD0)]
    [InlineData(_xsl0001LOD1)]
    public void Seraphim(string scmFile)
    {
        BinarySupremeCommanderMeshSerializer serializer = new BinarySupremeCommanderMeshSerializer();

        using var fileStream = _fixture.GetFileStream(scmFile);
        SupremeCommanderMeshFile expectedMesh = serializer.DeserializeSupremeCommanderModelFile(fileStream);
        using var intermediateStream = serializer.SerializeSupremeCommanderModelFile(expectedMesh);
        intermediateStream.Position = 0;
        SupremeCommanderMeshFile actualMesh = serializer.DeserializeSupremeCommanderModelFile(intermediateStream);

        // compare the mesh
        Assert.True(expectedMesh.Bones.SequenceEqual(actualMesh.Bones));
        Assert.True(expectedMesh.Vertices.SequenceEqual(actualMesh.Vertices));
        Assert.True(expectedMesh.Triangles.SequenceEqual(actualMesh.Triangles));
        Assert.True(expectedMesh.Information.SequenceEqual(actualMesh.Information));
    }
}