namespace FAForever.FileFormats.Mesh.Serialization;

public interface IMeshSerializer
{
    public Stream SerializeSupremeCommanderModelFile(MeshFile mesh);

    public MeshFile DeserializeSupremeCommanderModelFile(Stream stream);
}