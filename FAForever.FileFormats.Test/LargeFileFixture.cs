namespace FAForever.FileFormats.Test;

public class LargeFileFixture: IDisposable
{
    private readonly Dictionary<string, byte[]> _fileCache = new();

    public LargeFileFixture(String[] files)
    {
        foreach (var file in files)
            LoadFile(file);
    }

    private void LoadFile(string path)
    {
        var bytes = File.ReadAllBytes(path);
        _fileCache[path] = bytes;
    }

    public Stream GetFileStream(string path)
    {
        if (!_fileCache.TryGetValue(path, out var data))
            throw new FileNotFoundException($"File not loaded: {path}");

        return new MemoryStream(data, writable: false);
    }

    public void Dispose()
    {
        _fileCache.Clear();
    }
}