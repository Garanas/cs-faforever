namespace FAForever.FileFormats.Mesh.Serialization;

public static class MeshSerializerFactory
{
    /// <summary>
    /// Tries to retrieve the correct serializer based on the extension of the file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="serializer"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static bool TryGetSerializer(string filePath, out IMeshSerializer? serializer)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();

        serializer = extension switch
        {
            ".scm" => new BinaryMeshSerializer(),
            _ => null
        };

        return serializer is not null;
    }

    /// <summary>
    /// Determine the correct serializer based on the extension of the file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static IMeshSerializer GetSerializer(string filePath)
    {
        var succeeded = TryGetSerializer(filePath, out var serializer);
        if (succeeded && serializer is not null) return serializer;

        throw new NotSupportedException($"Unsupported format: {filePath}");
    }

    /// <summary>
    /// Tries to determine the correct serializer based on the first few bytes of the stream. 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="serializer"></param>
    /// <returns></returns>
    public static bool TryGetSerializer(Stream stream, out IMeshSerializer? serializer)
    {
        // read the first four bytes
        var buffer = new byte[4];
        var originalPosition = stream.Position;
        stream.Seek(0, SeekOrigin.Begin);
        var bytesRead = stream.Read(buffer, 0, 4);
        stream.Position = originalPosition;

        // interpret them
        serializer = bytesRead switch
        {
            BinaryMeshSerializer.MagicFileHeader => new BinaryMeshSerializer(),
            _ => null
        };

        return serializer is not null;
    }

    /// <summary>
    /// Determine the correct serializer based on the first few bytes of the stream. 
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static IMeshSerializer GetSerializer(Stream stream)
    {
        var succeeded = TryGetSerializer(stream, out var serializer);
        if (succeeded && serializer is not null) return serializer;

        throw new NotSupportedException();
    }
}