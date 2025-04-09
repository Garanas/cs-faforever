namespace FAForever.FileFormats.Test;

public static class TestUtils
{
    /// <summary>
    /// Asserts that two streams are equivalent byte-by-byte. Restores the original position in the stream.
    /// </summary>
    /// <param name="expectedStream"></param>
    /// <param name="actualStream"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static void AssertStreams(Stream expectedStream, Stream actualStream)
    {
        if (!expectedStream.CanRead || !actualStream.CanRead)
            Assert.Fail("Stream is not readable.");

        var expectedPosition = expectedStream.Position;
        var actualPosition = actualStream.Position;

        actualStream.Position = 0;
        expectedStream.Position = 0;

        const int bufferSize = 16;
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
        } while (expectedBytesRead > 0);

        expectedStream.Position = expectedPosition;
        actualStream.Position = actualPosition;
    }
}