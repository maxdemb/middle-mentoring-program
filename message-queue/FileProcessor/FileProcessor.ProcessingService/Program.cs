using System.Runtime.CompilerServices;
using System.Text.Json;
using FileProcessor.DTO;
using FileProcessor.ProcessingService;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


var extensions = new [] { "txt", "pdf"};
var destinationFolderRoot = "C:\\Users\\Maksym_Dembitskyi\\Desktop\\FileDestination\\";

using var messageService = new MessageService();

foreach (var extension in extensions)
{
    var destinationFolder = $"{destinationFolderRoot}\\{extension}";
    var fileService = new FileService(destinationFolder);
    Console.WriteLine($"New destination is set: {destinationFolder}");

    var consumer = messageService.AddConsumer(extension);

    consumer.Received += fileService.HandleFileChunkReceived;

}

Console.WriteLine("Enter anything to exit");
Console.ReadLine();



