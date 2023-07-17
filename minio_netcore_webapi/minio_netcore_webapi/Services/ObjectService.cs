using System.Net;
using minio_netcore_webapi.Models;

namespace minio_netcore_webapi.Services;

public class ObjectService
{
    private readonly MinioService _minioService;

    public ObjectService(MinioService minioService)
    {
        _minioService = minioService;
    }
    
    public async Task<ApiResponse<bool>> PutObject(string bucketName, IFormFile file)
    {
        try
        {
            var bucketExists = await _minioService.BucketExist(bucketName);
            if (bucketExists)
            {
                return ApiResponse<bool>.Fail("Bucket name already exists!", HttpStatusCode.BadRequest);
            }
            
            string objectName = Guid.NewGuid().ToString().Substring(0, 7) + Path.GetExtension(file.FileName);
            string contentType = file.ContentType;
            
            MemoryStream stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;
            
            await _minioService.PutObject(bucketName, stream, objectName, contentType, null);
            return ApiResponse<bool>.Success(true, HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return ApiResponse<bool>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
    
    public async Task<ApiResponse<List<string>>> ListObjects(string bucketName)
    {
        try
        {
            var bucketExists = await _minioService.BucketExist(bucketName);
            if (bucketExists)
            {
                return ApiResponse<List<string>>.Fail("Bucket name already exists!", HttpStatusCode.BadRequest);
            }
            
            var listBuckets = await _minioService.ListObjects(bucketName);
            return ApiResponse<List<string>>.Success(listBuckets, HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return ApiResponse<List<string>>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
    
    public async Task<ApiResponse<bool>> GetObject(string bucketName, string objectName, string fileName)
    {
        try
        {
            var bucketExists = await _minioService.BucketExist(bucketName);
            if (bucketExists)
            {
                return ApiResponse<bool>.Fail("Bucket name already exists!", HttpStatusCode.BadRequest);
            }
            
            await _minioService.GetObject(bucketName, objectName, fileName);
            return ApiResponse<bool>.Success(true, HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return ApiResponse<bool>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
}