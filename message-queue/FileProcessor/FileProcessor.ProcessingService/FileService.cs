using System.Security.Cryptography.X509Certificates;
using FileProcessor.DTO;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Collections;

namespace FileProcessor.ProcessingService;

public class FileService
{
    private readonly string _folderPath;
    private List<FileDto> _files;
 
    public FileService(string folderPath)
    {
        _folderPath = folderPath;
        _files = new List<FileDto>();
    }

    public void HandleFileChunkReceived(object sender, BasicDeliverEventArgs args)
    {

        var fileChunkDto = JsonSerializer.Deserialize<FileChunkDto>(args.Body.ToArray());

        if (fileChunkDto is null) return;

        var file = _files.FirstOrDefault(x => x.Name == fileChunkDto.Name);

        if (file is null)
        {
            file = new FileDto
            {
                Name = fileChunkDto.Name,
                Chunks = new List<FileChunkDto>(),
                ChunksAmount = fileChunkDto.EndIndicator + 1
            };
            _files.Add(file);
        }

        file.Chunks.Add(fileChunkDto);

        

        if (file.Chunks.Count == file.ChunksAmount) 
        {
            file.Bytes = file.Chunks.OrderBy(x => x.Number).SelectMany(x => x.Data);

            try
            {
                File.WriteAllBytes($"{_folderPath}/{file.Name}", file.Bytes.ToArray());

                Console.WriteLine($"File saved successfully:{file.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    
}

