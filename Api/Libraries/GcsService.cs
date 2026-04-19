using Google.Cloud.Storage.V1;

namespace Api.Libraries;

public class GcsService
{
    private readonly StorageClient _storage;

    private readonly string _bucketName = "your-bucket-name";

    public GcsService()
    {
        _storage = StorageClient.Create();
    }
    public async Task UploadFileAsync(IFormFile file)
    {
        using var stream = file.OpenReadStream();

        await _storage.UploadObjectAsync(
            bucket: _bucketName,
            objectName: file.FileName,
            contentType: file.ContentType,
            source: stream
        );
    }
    public async Task<byte[]> DownloadFileAsync(string fileName)
    {
        using var ms = new MemoryStream();

        await _storage.DownloadObjectAsync(
            bucket: _bucketName,
            objectName: fileName,
            destination: ms
        );

        return ms.ToArray();
    }
    public IEnumerable<string> ListFiles(string folder)
    {
        var objects = _storage.ListObjects(_bucketName, folder);

        foreach (var obj in objects)
        {
            yield return obj.Name;
        }
    }
    public async Task DeleteFileAsync(string fileName)
    {
        await _storage.DeleteObjectAsync(_bucketName, fileName);
    }
}