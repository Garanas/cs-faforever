namespace FAForever.FileFormats.Mesh.Serialization;

public interface ISupremeCommanderMeshSerializer
{
    public Stream SerializeSupremeCommanderModelFile (SupremeCommanderMeshFile mesh);
    
    public SupremeCommanderMeshFile DeserializeSupremeCommanderModelFile(Stream stream);

}