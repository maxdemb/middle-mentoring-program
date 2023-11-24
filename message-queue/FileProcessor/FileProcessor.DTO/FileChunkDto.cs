namespace FileProcessor.DTO;

public record FileChunkDto(string Name, int Number, int EndIndicator, IEnumerable<byte> Data);
