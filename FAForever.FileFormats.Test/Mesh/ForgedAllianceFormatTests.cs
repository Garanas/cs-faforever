using FAForever.FileFormats.Mesh.Serialization;
using FAForever.FileFormats.Mesh;


namespace FAForever.FileFormats.Test.Mesh;

public class ForgedAllianceFormatTests
{
    private const string _ual0101LOD0 = "Assets/Meshes/ForgedAlliance/ual0101_lod0.scm";
    private const string _ual0101LOD1 = "Assets/Meshes/ForgedAlliance/ual0101_lod1.scm";
    private const string _uel0401LOD0 = "Assets/Meshes/ForgedAlliance/uel0401_lod0.scm";
    private const string _uel0401LOD1 = "Assets/Meshes/ForgedAlliance/uel0401_lod1.scm";
    private const string _xsl0001LOD0 = "Assets/Meshes/ForgedAlliance/xsl0001_lod0.scm";
    private const string _xsl0001LOD1 = "Assets/Meshes/ForgedAlliance/xsl0001_lod1.scm";
    
    private readonly LargeFileFixture _fixture = new LargeFileFixture([_ual0101LOD0, _ual0101LOD1, _uel0401LOD0, _uel0401LOD1, _xsl0001LOD0, _xsl0001LOD1]);
    
    private static void StreamsAreEqual(Stream expectedStream, Stream actualStream)
    {
        if (expectedStream == null || actualStream == null)
            throw new ArgumentNullException();

        if (!expectedStream.CanRead || !actualStream.CanRead)
            throw new InvalidOperationException("Streams must be readable.");
        
        Assert.Equal(expectedStream.Length, actualStream.Length);

        const int bufferSize = 128;
        var buffer1 = new byte[bufferSize];
        var buffer2 = new byte[bufferSize];

        int expectedBytesRead;
        do
        {
            expectedBytesRead = expectedStream.Read(buffer1, 0, bufferSize);
            var actualBytesRead = actualStream.Read(buffer2, 0, bufferSize);

            // compare number of byte read and the content of the buffers
            Assert.Equal(expectedBytesRead, actualBytesRead);
            Assert.True(buffer1.AsSpan(0, expectedBytesRead).SequenceEqual(buffer2.AsSpan(0, actualBytesRead)));
        }
        while (expectedBytesRead > 0);
    }
    
    [Theory]
    [InlineData(_ual0101LOD0)]
    [InlineData(_ual0101LOD1)]
    public void AeonUnits(String scmFile)
    {
        using var stream = _fixture.GetFileStream(scmFile);
        BinarySupremeCommanderMeshSerializer serializer = new BinarySupremeCommanderMeshSerializer();
        SupremeCommanderMeshFile mesh = serializer.DeserializeSupremeCommanderModelFile(stream);
    }
    
    [Theory]
    [InlineData(_uel0401LOD0)]
    [InlineData(_uel0401LOD1)]
    public void UEFUnits(String scmFile)
    {
        using var stream = _fixture.GetFileStream(scmFile);
        
        BinarySupremeCommanderMeshSerializer serializer = new BinarySupremeCommanderMeshSerializer();
        SupremeCommanderMeshFile mesh = serializer.DeserializeSupremeCommanderModelFile(stream);
    }
    
    [Theory]
    [InlineData(_xsl0001LOD0)]
    [InlineData(_xsl0001LOD1)]
    public void SeraphimUnits(String scmFile)
    {
        using var stream = _fixture.GetFileStream(scmFile);
        
        BinarySupremeCommanderMeshSerializer serializer = new BinarySupremeCommanderMeshSerializer();
        SupremeCommanderMeshFile mesh = serializer.DeserializeSupremeCommanderModelFile(stream);
    }

    [Theory]
    [InlineData(_ual0101LOD0)]
    [InlineData(_ual0101LOD1)]
    public void DeserializeAndSerializeAeonUnits(string scmFile)
    {
        using var expectedStream = _fixture.GetFileStream(scmFile);
        BinarySupremeCommanderMeshSerializer serializer = new BinarySupremeCommanderMeshSerializer();
        SupremeCommanderMeshFile mesh = serializer.DeserializeSupremeCommanderModelFile(expectedStream);
        
        var actualStream = serializer.SerializeSupremeCommanderModelFile(mesh);

        // compare the streams
        actualStream.Position = 0;
        expectedStream.Position = 0;
        StreamsAreEqual(expectedStream, actualStream);
    }
}