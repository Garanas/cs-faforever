
using System.Text;

namespace FAForever.FileFormats.Mesh.Serialization;

using FAForever.FileFormats.Common;

/// <summary>
/// Provides serialization and deserialization support for SCM version 5 mesh files used in Supreme Commander.
///
/// <para>
/// Multi-byte data is written in little-endian ("Intel") format.
/// </para>
///
/// <para>
/// An SCM file contains 5 required and 2 optional sections. Each section begins with a FOURCC identifier:
/// </para>
///
/// <list type="table">
///   <listheader>
///     <term>FOURCC</term>
///     <description>Contents</description>
///   </listheader>
///   <item>
///     <term>'MODL'</term>
///     <description>Header info</description>
///   </item>
///   <item>
///     <term>'NAME'</term>
///     <description>List of bone name strings</description>
///   </item>
///   <item>
///     <term>'SKEL'</term>
///     <description>Array of bone data</description>
///   </item>
///   <item>
///     <term>'VTXL'</term>
///     <description>Array of basic vertex data</description>
///   </item>
///   <item>
///     <term>'TRIS'</term>
///     <description>Array of triangle indices</description>
///   </item>
///   <item>
///     <term>'VEXT'</term>
///     <description>Array of extra vertex data (optional)</description>
///   </item>
///   <item>
///     <term>'INFO'</term>
///     <description>List of null-terminated information strings (optional)</description>
///   </item>
/// </list>
///
/// <para>
/// Section offsets in the file header point to the beginning of section data—specifically, to the first byte after the FOURCC.
/// Padding is added to the end of each section to ensure 16-byte alignment. A value of 0 indicates that the section is omitted.
/// </para>
///
/// <para>
/// <b>All offsets are relative to the start of the file.</b>
/// </para>
/// </summary>
public class BinarySupremeCommanderMeshSerializer: ISupremeCommanderMeshSerializer
{
    /// <summary>
    /// All sections of the file are aligned to guarantee the size of the section is a multiple of 32 bytes.
    /// </summary>
    private const int PADDING_SIZE = 32;

    /// <summary>
    /// The byte that is used for padding.
    /// </summary>
    private const byte PADDING_BYTE = 0xC5;
    
    /// <summary>
    /// Represents the string 'MODL'.
    /// </summary>
    private const int MAGIC_FILE_HEADER = 1279545165;
    
    /// <summary>
    /// Computes the remaining padding of a section.
    /// </summary>
    /// <param name="streamPosition">The current position of the stream.</param>
    /// <returns></returns>
    public int ComputePadding(long streamPosition)
    {
        var paddingInBytes = (int)(PADDING_SIZE - (streamPosition  % PADDING_SIZE));
        if (paddingInBytes > PADDING_SIZE - 1) return 0;
        return paddingInBytes;
    }

    /// <summary>
    /// Retrieves the 4-byte identifier keyword stored in the padding of the previous section.
    /// </summary>
    /// <param name="reader">The binary reader positioned after the identifier keyword.</param>
    /// <returns>The 4-character ASCII keyword.</returns>
    public string RetrieveIdentifier(BinaryReader reader)
    {
        reader.BaseStream.Position -= 4;
        var identifier = reader.ReadBytes(4);
        var keyword = Encoding.ASCII.GetString(identifier);
        return keyword;
    }
    
    /// <summary>
    /// Reads bytes until it finds a null byte starting at the given offset. Sets the stream to the original offset before returning, as if nothing changed.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public string DeserializeNullTerminatedStringAtOffset(BinaryReader reader, long offset)
    {
        var currentOffset = reader.BaseStream.Position;
        reader.BaseStream.Position = offset;
        var result = DeserializeNullTerminatedString(reader);
        reader.BaseStream.Position = currentOffset;
        return result;
    }
    
    /// <summary>
    /// Reads bytes until it finds a null byte. Advances the stream with the size of the string.
    /// </summary>
    /// <returns></returns>
    public string DeserializeNullTerminatedString(BinaryReader reader)
    {
        // the implementation is complicated because it favors performance over 
        // readability. We're dealing with null terminated strings. That means we
        // do not know how long a string is until we've processed it as a whole.
        // 
        // There are various ways to solve this problem:
        // 1. Read and store each character in a (byte) array until we find the null character ('\0').
        // 2. Read and store each character in a string builder until we find the null character ('\0').
        // 3. Read the stream twice: once to determine the length of the string, and a second time to read the string in one go.
        // 
        // We chose (3) because it is the most performant solution. To prevent 
        // access to the heap, we allocate a buffer on the stack. From an allocation
        // point of view that is not much different from sharing a reference to a byte 
        // array and/or a string builder, but this approach is thread safe and it 
        // performs better in terms of computing time. 
        
        // determine the length
        long start = reader.BaseStream.Position;
        while (reader.ReadByte() != 0);
        long end = reader.BaseStream.Position;

        // read it into a buffer that lives on the stack
        int diff = (int)(end - start - 1);
        Span<byte> buffer = stackalloc byte[(int)(end - start - 1)];
        reader.BaseStream.Position = start;
        for (int k = 0; k < diff; k++) {
            buffer[k] = reader.ReadByte();
        }

        // reset the stream
        reader.BaseStream.Position = end;

        // interpret the buffer
        return Encoding.UTF8.GetString(buffer);
    }

    /// <summary>
    /// Reads a Supreme Commander Mesh (SCM) file from a stream. Does not dispose the stream.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public SupremeCommanderMeshFile DeserializeSupremeCommanderModelFile(Stream stream)
    {
        BinaryReader reader = new BinaryReader(stream);
        
        SupremeCommanderMeshFileHeader supremeCommanderMeshFileHeader = DeserializeMeshFileHeader(reader);

        // check magic header
        if (supremeCommanderMeshFileHeader.Marker != MAGIC_FILE_HEADER)
            throw new Exception("Invalid magic file header. Expected 'MODL'");
        
        // check file version
        if (supremeCommanderMeshFileHeader.Version != 5)
            throw new Exception($"Unsupported file version: {supremeCommanderMeshFileHeader.Version}, expected version 5.");
        
        // deserialize the file
        List<SupremeCommanderMeshBone> bones = DeserializeBones(reader, supremeCommanderMeshFileHeader, supremeCommanderMeshFileHeader.BoneCount);
        List<SupremeCommanderMeshVertex> vertices = DeserializeVertices(reader, supremeCommanderMeshFileHeader);
        List<SupremeCommanderMeshTriangle> triangles = DeserializeIndices(reader, supremeCommanderMeshFileHeader);
        List<string> information = DeserializeInformation(reader, supremeCommanderMeshFileHeader);
        
        return new SupremeCommanderMeshFile(bones.AsReadOnly(), vertices.AsReadOnly(), triangles.AsReadOnly(), information.AsReadOnly());
    }

    /// <summary>
    /// Reads all bone data from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="supremeCommanderMeshFileHeader"></param>
    /// <param name="boneCount"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public List<SupremeCommanderMeshBone> DeserializeBones(BinaryReader reader, SupremeCommanderMeshFileHeader supremeCommanderMeshFileHeader, int boneCount)
    {
        // check on file corruption
        reader.BaseStream.Position = supremeCommanderMeshFileHeader.BoneOffset;
        if (RetrieveIdentifier(reader) != "SKEL")
            throw new Exception("Corrupted file: expected the section for triangle indices to start with the keyword 'SKEL'.");

        var bonesData = new List<SupremeCommanderMeshBone>();
        
        for (int j = 0; j < boneCount; j++)
            bonesData.Add(DeserializeBone(reader));

        return bonesData;
    }
    
    /// <summary>
    /// Reads all vertices from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="supremeCommanderMeshFileHeader"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public List<SupremeCommanderMeshVertex> DeserializeVertices(BinaryReader reader, SupremeCommanderMeshFileHeader supremeCommanderMeshFileHeader)
    {
        // check on file corruption
        reader.BaseStream.Position = supremeCommanderMeshFileHeader.VertexOffset;
        if (RetrieveIdentifier(reader) != "VTXL")
            throw new Exception("Corrupted file: expected the section for triangle indices to start with the keyword 'VTXL'.");
        
        var vertexData = new List<SupremeCommanderMeshVertex>();
        
        for (int j = 0; j < supremeCommanderMeshFileHeader.VertexCount; j++)
            vertexData.Add(DeserializeVertex(reader));
        
        return vertexData;
    }
    
    /// <summary>
    /// Reads all indices from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="supremeCommanderMeshFileHeader"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public List<SupremeCommanderMeshTriangle> DeserializeIndices(BinaryReader reader, SupremeCommanderMeshFileHeader supremeCommanderMeshFileHeader)
    {
        // check on file corruption
        reader.BaseStream.Position = supremeCommanderMeshFileHeader.IndexOffset;
        if (RetrieveIdentifier(reader) != "TRIS")
            throw new Exception("Corrupted file: expected the section for triangle indices to start with the keyword 'TRIS'.");

        var triangleData = new List<SupremeCommanderMeshTriangle>();
        for (int j = 0; j < supremeCommanderMeshFileHeader.IndexCount / 3; j++)
            triangleData.Add(DeserializeTriangle(reader));
        
        return triangleData;
    }

    /// <summary>
    /// Reads the information from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="supremeCommanderMeshFileHeader"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public List<string> DeserializeInformation(BinaryReader reader, SupremeCommanderMeshFileHeader supremeCommanderMeshFileHeader)
    {
        // check on file corruption
        reader.BaseStream.Position = supremeCommanderMeshFileHeader.InformationOffset;
        if (RetrieveIdentifier(reader) != "INFO")
            throw new Exception("Corrupted file: expected the section for information to start with the keyword 'INFO'.");
        
        // deserialize the information
        var builder = new StringBuilder();
        List<string> information = new List<string>();
        while (reader.BaseStream.Position < supremeCommanderMeshFileHeader.InformationOffset + supremeCommanderMeshFileHeader.InfoSizeInBytes)
        {
            information.Add(DeserializeNullTerminatedString(reader));
        }

        return information;
    }
    
    /// <summary>
    /// Reads the file header from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public SupremeCommanderMeshFileHeader DeserializeMeshFileHeader(BinaryReader reader)
    {
        var magicHeader = reader.ReadInt32();
        var version = reader.ReadInt32();
        var boneOffset = reader.ReadInt32();
        var weightedBoneCount = reader.ReadInt32();
        var vertexOffset = reader.ReadInt32();
        var extraVertexOffset = reader.ReadInt32();
        var vertexCount = reader.ReadInt32();
        var indexOffset = reader.ReadInt32();
        var indexCount = reader.ReadInt32();
        var infoOffset = reader.ReadInt32();
        var infoSizeInBytes = reader.ReadInt32();
        var boneCount = reader.ReadInt32();
        
        
        return new SupremeCommanderMeshFileHeader(
            Marker: magicHeader, 
            Version: version, 
            BoneOffset: boneOffset, 
            WeightedBoneCount: weightedBoneCount, 
            VertexOffset: vertexOffset, 
            ExtraVertexOffset: extraVertexOffset, 
            VertexCount: vertexCount, 
            IndexOffset: indexOffset, 
            IndexCount: indexCount, 
            InformationOffset: infoOffset, 
            InfoSizeInBytes: infoSizeInBytes, 
            BoneCount: boneCount);
    }

    /// <summary>
    /// Reads one instance of bone data from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public SupremeCommanderMeshBone DeserializeBone(BinaryReader reader)
    {
        var someMatrix = DeserializeMatrix4(reader);
        var position = DeserializeVector3(reader);
        var rotation = DeserializeQuaternion(reader);
        
        var nameOffset = reader.ReadInt32();
        string name = DeserializeNullTerminatedStringAtOffset(reader, nameOffset);
        
        var boneParent = reader.ReadInt32();
        
        var reserved1 = reader.ReadInt32();
        var reserved2  = reader.ReadInt32();

        return new SupremeCommanderMeshBone(
            Name: name, 
            RestPoseInverse: someMatrix, 
            Position: position, 
            Rotation: rotation, 
            BoneParent: boneParent,
            Reserved1: reserved1,
            Reserved2: reserved2);
    }

    /// <summary>
    /// Reads a vertex from the stream. 
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public SupremeCommanderMeshVertex DeserializeVertex(BinaryReader reader)
    {
        var position = DeserializeVector3(reader);
        var tangent = DeserializeVector3(reader);
        var normal = DeserializeVector3(reader);
        var binormal = DeserializeVector3(reader);
        
        var texCoords1 = DeserializeVector2(reader);
        var texCoords2 = DeserializeVector2(reader);
        
        var boneId1 = reader.ReadByte();
        var boneId2 = reader.ReadByte();
        var boneId3 = reader.ReadByte();
        var boneId4 = reader.ReadByte();
        
        return new SupremeCommanderMeshVertex(
            Position: position, 
            Normal: normal, 
            Tangent: tangent, 
            Binormal: binormal,
            TexCoord1: texCoords1,
            TexCoord2: texCoords2, 
            BoneId1: boneId1,
            BoneId2: boneId2, 
            BoneId3: boneId3, 
            BoneId4: boneId4);
    }

    /// <summary>
    /// Reads three triangle indices from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public SupremeCommanderMeshTriangle DeserializeTriangle(BinaryReader reader)
    {
        var index1 = reader.ReadInt16();
        var index2 = reader.ReadInt16();
        var index3 = reader.ReadInt16();
        
        return new SupremeCommanderMeshTriangle(index1, index2, index3);
    }

    /// <summary>
    /// Reads a 4x4 matrix from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public Matrix4 DeserializeMatrix4(BinaryReader reader)
    {
        var row1 = DeserializeVector4(reader);
        var row2 = DeserializeVector4(reader);
        var row3 = DeserializeVector4(reader);
        var row4 = DeserializeVector4(reader);

        return new Matrix4(row1, row2, row3, row4);
    }

    /// <summary>
    /// Reads a quaternion from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public Quaternion DeserializeQuaternion(BinaryReader reader)
    {
        var w = reader.ReadSingle();
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        return new Quaternion(w, x, y, z);
    }

    /// <summary>
    /// Reads four floats from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public Vector4 DeserializeVector4(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        var w = reader.ReadSingle();
        return new Vector4(x, y, z, w);
    }
    
    /// <summary>
    /// Reads three floats from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public Vector3 DeserializeVector3(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Reads two floats from the stream.
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public Vector2 DeserializeVector2(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        return new Vector2(x, y);
    }

    /// <summary>
    /// Computes the number of bones that influence vertices.
    /// </summary>
    public int ComputedUsedBones(IReadOnlyList<SupremeCommanderMeshVertex> vertices)
    {
        var usedBones = new HashSet<int>();
        foreach (var vertex in vertices)
        {
            usedBones.Add(vertex.BoneId1);
            usedBones.Add(vertex.BoneId2);
            usedBones.Add(vertex.BoneId3);
            usedBones.Add(vertex.BoneId4);
        }
        
        return usedBones.Count;
    }

    public void AddIdentifier(BinaryWriter writer, string identifier)
    {
        writer.BaseStream.Position = writer.BaseStream.Length - 4;
        writer.Write(identifier[0]);
        writer.Write(identifier[1]);
        writer.Write(identifier[2]);
        writer.Write(identifier[3]);
    }

    public void AddPadding(BinaryWriter writer)
    {
        int computedPadding = ComputePadding(writer.BaseStream.Position);
        
        // guarantee that there's room for the identifier
        if (computedPadding <= 4)
            computedPadding += PADDING_SIZE;
        
        for (int i = 0; i < computedPadding; i++)
            writer.Write(PADDING_BYTE);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="stream"></param>
    /// <exception cref="NotImplementedException"></exception>
    public Stream SerializeSupremeCommanderModelFile(SupremeCommanderMeshFile mesh)
    {
        const int START_DATA_OFFSET = 2 * PADDING_SIZE;
        
        // static information
        int usedBoneCount = ComputedUsedBones(mesh.Vertices);
        
        // serialize all the properties, we need the offsets for the header section of the file.
        using MemoryStream boneNameStream = new MemoryStream();
        using MemoryStream boneDataStream = new MemoryStream();
        SerializeBones(mesh.Bones, new BinaryWriter(boneDataStream), new BinaryWriter(boneNameStream), START_DATA_OFFSET);
        
        using MemoryStream vertexStream = new MemoryStream();
        SerializeVertices(mesh.Vertices, new BinaryWriter(vertexStream));
        
        using MemoryStream indexStream = new MemoryStream();
        SerializeTriangles(mesh.Triangles, new BinaryWriter(indexStream));
        
        using MemoryStream informationStream = new MemoryStream();
        SerializeInformation(mesh.Information, new BinaryWriter(informationStream));
        
        // construct the serialized file
        MemoryStream destination = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(destination);
        writer.Write(MAGIC_FILE_HEADER);
        writer.Write(5);
        
        int boneDataOffset = (int)(START_DATA_OFFSET + boneNameStream.Length);
        writer.Write(boneDataOffset);
        writer.Write(usedBoneCount);
        
        int vertexDataOffset = (int)(boneDataOffset + boneDataStream.Length);
        writer.Write((int)vertexDataOffset);
        writer.Write((int)(0));
        writer.Write((int)mesh.Vertices.Count);
        
        int indexDataOffset = (int)(vertexDataOffset + vertexStream.Length);
        writer.Write((int)indexDataOffset);
        writer.Write((int)(mesh.Triangles.Count * 3));
        
        int informationDataOffset = (int)(indexDataOffset + indexStream.Length);
        writer.Write((int)informationDataOffset);
        writer.Write((int)informationStream.Length); // this is not an offset, but the size of the information in bytes
        
        writer.Write(mesh.Bones.Count);
        
        // all sections are aligned, including the header section
        AddPadding(writer);
        
        // add bone name section
        AddIdentifier(writer, "NAME");
        boneNameStream.Position = 0;
        boneNameStream.CopyTo(destination);
        
        // add bone data section
        AddIdentifier(writer, "SKEL");
        boneDataStream.Position = 0;
        boneDataStream.CopyTo(destination);
        
        // add vertex section
        AddIdentifier(writer, "VTXL");
        vertexStream.Position = 0;
        vertexStream.CopyTo(destination);
        
        // add triangle index section
        AddIdentifier(writer, "TRIS");
        indexStream.Position = 0;
        indexStream.CopyTo(destination);
        
        // add information section
        AddIdentifier(writer, "INFO");
        informationStream.Position = 0;
        informationStream.CopyTo(destination);
        
        // annnddd.... we're done!
        return destination;
    }

    public void SerializeInformation(IReadOnlyList<string> information, BinaryWriter writer)
    {
        foreach (var info in information)
        {
            // write out the information. We intentionally do not use the string override as that appears to add a byte.
            foreach (var c in info)
                writer.Write(c);
            writer.Write((byte) 0);
        }
    }

    public void SerializeTriangles(IReadOnlyList<SupremeCommanderMeshTriangle> triangles, BinaryWriter writer)
    {
        foreach (var triangle in triangles)
            SerializeTriangle(triangle, writer);
        
        // all sections are expected to be aligned
        AddPadding(writer);
    }

    public void SerializeTriangle(SupremeCommanderMeshTriangle triangle, BinaryWriter writer)
    {
        writer.Write(triangle.Index1);
        writer.Write(triangle.Index2);
        writer.Write(triangle.Index3);
    }

    public void SerializeBones(IReadOnlyList<SupremeCommanderMeshBone> bones, BinaryWriter boneDataWriter, BinaryWriter boneNameWriter, int headerOffset)
    {
        foreach (var bone in bones)
        {
            // offset that we need to store at the bone data
            long boneNameOffset = boneNameWriter.BaseStream.Position + headerOffset;
            
            // write out the bone name. We intentionally do not use the string override as that appears to add a byte.
            foreach (var c in bone.Name)
                boneNameWriter.Write(c);
            
            boneNameWriter.Write((byte) 0);
            
            // write out the bone data
            SerializeBone(bone, boneDataWriter, (int) boneNameOffset);
        }
        
        // all sections are expected to be aligned
        AddPadding(boneDataWriter);
        AddPadding(boneNameWriter);
    }
    
    public void SerializeBone(SupremeCommanderMeshBone bone, BinaryWriter writer, int nameOffset)
    {
        SerializeMatrix4(bone.RestPoseInverse, writer);
        SerializeVector3(bone.Position, writer);
        SerializeQuaternion(bone.Rotation, writer);
        
        writer.Write(nameOffset);
        writer.Write(bone.BoneParent);
        writer.Write(bone.Reserved1);
        writer.Write(bone.Reserved2);
    }

    public void SerializeVertices(IReadOnlyList<SupremeCommanderMeshVertex> vertices, BinaryWriter writer)
    {
        foreach (var vertex in vertices)
            SerializeVertex(vertex, writer);
        
        // all sections are expected to be aligned
        AddPadding(writer);
    }
    
    public void SerializeVertex(SupremeCommanderMeshVertex supremeCommanderMeshVertex, BinaryWriter writer)
    {
        SerializeVector3(supremeCommanderMeshVertex.Position, writer);
        SerializeVector3(supremeCommanderMeshVertex.Tangent, writer);
        SerializeVector3(supremeCommanderMeshVertex.Normal, writer);
        SerializeVector3(supremeCommanderMeshVertex.Binormal, writer);
        
        SerializeVector2(supremeCommanderMeshVertex.TexCoord1, writer);
        SerializeVector2(supremeCommanderMeshVertex.TexCoord2, writer);
        
        writer.Write(supremeCommanderMeshVertex.BoneId1);
        writer.Write(supremeCommanderMeshVertex.BoneId2);
        writer.Write(supremeCommanderMeshVertex.BoneId3);
        writer.Write(supremeCommanderMeshVertex.BoneId4);
    }

    public void SerializeMatrix4(Matrix4 matrix, BinaryWriter writer)
    {
        SerializeVector4(matrix.Row1, writer);
        SerializeVector4(matrix.Row2, writer);
        SerializeVector4(matrix.Row3, writer);
        SerializeVector4(matrix.Row4, writer);
    }
    
    public void SerializeQuaternion(Quaternion vector, BinaryWriter writer)
    {
        writer.Write(vector.W);
        writer.Write(vector.X);
        writer.Write(vector.Y);
        writer.Write(vector.Z);
    }
    
    public void SerializeVector4(Vector4 vector, BinaryWriter writer)
    {
        writer.Write(vector.X);
        writer.Write(vector.Y);
        writer.Write(vector.Z);
        writer.Write(vector.W);
    }

    public void SerializeVector3(Vector3 vector, BinaryWriter writer)
    {
        writer.Write(vector.X);
        writer.Write(vector.Y);
        writer.Write(vector.Z);
    }

    public void SerializeVector2(Vector2 vector, BinaryWriter writer)
    {
        writer.Write(vector.X);
        writer.Write(vector.Y);
    }

}