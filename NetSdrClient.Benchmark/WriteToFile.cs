using BenchmarkDotNet.Attributes;

namespace NetSdrClient.Benchmark;

/// <summary>
/// not sure if it is valid test, but results are almost the same
/// </summary>
public class WriteToFile
{
    private const int NumberOfCycles = 100;
    private const int N = 5000;
    private readonly byte[] data;

    public WriteToFile()
    {
        data = new byte[N];
        new Random().NextBytes(data);
    }

    [Benchmark]
    public void FileStream()
    {
        using var fileStream = new FileStream("FileStream.bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        for (int i = 0; i < NumberOfCycles; i++)
        {
            fileStream.Write(data, 0, data.Length);
        }
    }

    [Benchmark]
    public void FileStreamBinaryWriter()
    {
        using var fileStream = new FileStream("FileStreamBinaryWriter.bin", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        using var binaryWriter = new BinaryWriter(fileStream);
        for (int i = 0; i < NumberOfCycles; i++)
        {
            binaryWriter.Write(data, 0, data.Length);
        }
    }
}
