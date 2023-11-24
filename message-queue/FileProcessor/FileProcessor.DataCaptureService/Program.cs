using FileProcessor.DataCaptureService;

var folderPath = "C:\\Users\\Maksym_Dembitskyi\\Desktop\\FileSource";

Watch(folderPath);

static void Watch(string path)
{
    using var watcher = new FileSystemWatcher(path);
    var fileService = new FileService(path);

    watcher.Created += fileService.OnFileCreated;
    watcher.Created += fileService.ReadFile;

    watcher.EnableRaisingEvents = true;

    Console.WriteLine($"Watching for changes in: {path}");
    Console.WriteLine("Press 'Enter' to exit.");
    Console.ReadLine();
}