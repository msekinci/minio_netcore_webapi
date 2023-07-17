namespace minio_netcore_webapi.Models;

public class PutObjectDto
{
    public IFormFile File { get; set; } = null!;
}