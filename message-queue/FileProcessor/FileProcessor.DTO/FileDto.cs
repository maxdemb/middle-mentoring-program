namespace FileProcessor.DTO;

public class FileDto
{
    public string Name { get; set; }
    public List<FileChunkDto> Chunks { get; set; }
    public int ChunksAmount { get; set; }
    public IEnumerable<byte> Bytes { get; set; }
}
