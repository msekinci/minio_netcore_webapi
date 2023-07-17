using CommunityToolkit.HighPerformance;
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

    #region Bucket Methods
    
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
    
    #endregion

    #region Object Methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bucketName">Bucket name</param>
    /// <param name="objectName">The file in bucket</param>
    /// <param name="fileName">The file name we want to download</param>
    /// <returns></returns>
    public async Task GetObject(string bucketName, string objectName, string fileName)
    {
        var args = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithFile(fileName);
        await _minioClient.GetObjectAsync(args).ConfigureAwait(false);
    }
    
    public async Task RemoveObject(string bucketName, string objectName, string? versionId = null)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName);
        
        if (!string.IsNullOrEmpty(versionId))
        {
            args = args.WithVersionId(versionId);
        }
        
        await _minioClient.RemoveObjectAsync(args).ConfigureAwait(false);
    }
    
    public async Task PutObject(string bucketName, MemoryStream stream, string objectName, string fileName, string contentType, string? versionId = null)
    {
        var metaData = new Dictionary<string, string>
            (StringComparer.Ordinal)
            {
                { "FileName", fileName }
            };
        
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType);
        await _minioClient.PutObjectAsync(putObjectArgs);
    }

    #endregion
}