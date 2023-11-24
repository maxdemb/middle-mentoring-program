using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using FileProcessor.DTO;
using RabbitMQ.Client;

namespace FileProcessor.DataCaptureService;

public class FileService
{

    private readonly string _folderPath;
    private string _fullFilePath;
    private string _fileName;
    private string _fileExtension;
    private FileDto _file;

    public FileService(string folderPath)
    {
        _folderPath = folderPath;   
    }

    public void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        _fullFilePath = e.FullPath;
        if (!_fullFilePath.StartsWith(_folderPath))
        {
            throw new Exception("Invalid path");
        }

        _fileName = e.FullPath.Substring(_folderPath.Length + 1);

        _fileExtension = _fileName.SkipWhile(x => x != '.').Skip(1).ToString();

        Console.WriteLine("File added: " + _fileName);
    }

    public void ReadFile(object sender, FileSystemEventArgs e)
    {
        var fileInfo = new FileInfo(_fullFilePath);
        int fileSizeBytes = (int) fileInfo.Length;

        var fileBytes = File.ReadAllBytes(_fullFilePath);

        _file = new FileDto
        {
            Name = _fileName,
            Bytes = fileBytes
        };

        GetChunks(fileSizeBytes, _file);
    }

    public void GetChunks(int fileSizeBytes, FileDto file)
    {
        var sendPerOneTimeBytesNum = 1000;
        var chunkAmount = fileSizeBytes / sendPerOneTimeBytesNum;


        for (var chunkNum = 0; chunkNum < chunkAmount ; chunkNum++)
        {
            var sentBytesNum = chunkNum * sendPerOneTimeBytesNum;

            var chunkBytes = file.Bytes.Skip(sentBytesNum).Take(sendPerOneTimeBytesNum);

            var chunk = new FileChunkDto(_fileName, chunkNum, chunkAmount, chunkBytes.ToArray());

            SendChunk(chunk);
        }

        // last chunk:
        var lastChunkBytes = file.Bytes.TakeLast(fileSizeBytes - (sendPerOneTimeBytesNum * (chunkAmount - 1)));
        var lastChunk = new FileChunkDto(_fileName, chunkAmount, chunkAmount, lastChunkBytes);

        SendChunk(lastChunk);

        Console.WriteLine(JsonSerializer.Serialize(lastChunk));
    }

    private void SendChunk(FileChunkDto chunk)
    {
        var factory = new ConnectionFactory
        {
            Uri = new("amqp://admin:SomeNewRabbit2020@localhost:5672")
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var exchangeName = "fileProcessorEx"; // fileProcessorEx

        channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false, null);

        var messageBytes = JsonSerializer.SerializeToUtf8Bytes(chunk);

        channel.BasicPublish(exchangeName, "txt", null, messageBytes);

        connection.Close();
        channel.Close();
    }
}
