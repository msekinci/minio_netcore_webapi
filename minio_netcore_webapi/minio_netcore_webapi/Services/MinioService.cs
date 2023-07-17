using Minio;

namespace minio_netcore_webapi.Services;

public class MinioService
{
    private readonly IConfiguration _configuration;
    private readonly MinioClient _minioClient;

    public MinioService(IConfiguration configuration)
    {
        _configuration = configuration;

        string endPoint = _configuration["Minio:Endpoint"]!;
        string accessKey = _configuration["Minio:Accesskey"]!;
        string secretKey = _configuration["Minio:SecretKey"]!;

        _minioClient = new MinioClient()
            .WithEndpoint(endpoint: endPoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(false)
            .Build();
    }

    public async Task MakeBucket(string bucketName)
    {
        await _minioClient.MakeBucketAsync(
            new MakeBucketArgs()
                .WithBucket(bucketName)
        ).ConfigureAwait(false);
    }

    public async Task MakeBucketWithLock(string bucketName)
    {
        await _minioClient.MakeBucketAsync(
            new MakeBucketArgs()
                .WithBucket(bucketName)
                .WithObjectLock()
        ).ConfigureAwait(false);
    }

    public async Task<List<string>> ListBuckets()
    {
        List<string> listBuckets = new();
        var list = await _minioClient.ListBucketsAsync().ConfigureAwait(false);
        foreach (var bucket in list.Buckets) listBuckets.Add(bucket.Name);
        return listBuckets;
    }

    public async Task<bool> BucketExist(string bucketName)
    {
        var args = new BucketExistsArgs()
            .WithBucket(bucketName);
        var found = await _minioClient.BucketExistsAsync(args).ConfigureAwait(false);
        return found;
    }

    public async Task RemoveBucket(string bucketName)
    {
        await _minioClient.RemoveBucketAsync(
            new RemoveBucketArgs()
                .WithBucket(bucketName)
        ).ConfigureAwait(false);
    }

    public async Task<List<string>> ListObjects(string bucketName, string? prefix = null, bool recursive = true)
    {
        List<string> listObjects = new();
        
        var listArgs = new ListObjectsArgs()
            .WithBucket(bucketName)
            .WithPrefix(prefix)
            .WithRecursive(recursive);
        var observable = _minioClient.ListObjectsAsync(listArgs);
        var subscription = observable.Subscribe(
            item => listObjects.Add(item.Key),
            ex => throw ex,
            () => {});

        return listObjects;
    }
}