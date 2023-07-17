using System.Net;
using minio_netcore_webapi.Models;

namespace minio_netcore_webapi.Services;

public class BucketService
{
    private readonly MinioService _minioService;

    public BucketService(MinioService minioService)
    {
        _minioService = minioService;
    }

    public async Task<ApiResponse<bool>> MakeBucket(string bucketName)
    {
        try
        {
            var bucketExists = await _minioService.BucketExist(bucketName);
            if (bucketExists)
            {
                return ApiResponse<bool>.Fail("Bucket name already exists!", HttpStatusCode.BadRequest);
            }
            
            await _minioService.MakeBucket(bucketName);
            return ApiResponse<bool>.Success(true, HttpStatusCode.Created);
        }
        catch (Exception e)
        {
            return ApiResponse<bool>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
    
    public async Task<ApiResponse<bool>> MakeBucketWithLock(string bucketName)
    {
        try
        {
            var bucketExists = await _minioService.BucketExist(bucketName);
            if (bucketExists)
            {
                return ApiResponse<bool>.Fail("Bucket name already exists!", HttpStatusCode.BadRequest);
            }
            
            await _minioService.MakeBucketWithLock(bucketName);
            return ApiResponse<bool>.Success(true, HttpStatusCode.Created);
        }
        catch (Exception e)
        {
            return ApiResponse<bool>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
    
    public async Task<ApiResponse<List<string>>> ListBuckets()
    {
        try
        {
            var listBuckets = await _minioService.ListBuckets();
            return ApiResponse<List<string>>.Success(listBuckets, HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return ApiResponse<List<string>>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
    
    public async Task<ApiResponse<bool>> BucketExist(string bucketName)
    {
        try
        {
            var result = await _minioService.BucketExist(bucketName);
            return ApiResponse<bool>.Success(result, HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return ApiResponse<bool>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
    
    public async Task<ApiResponse<bool>> RemoveBucket(string bucketName)
    {
        try
        {
            var bucketExists = await _minioService.BucketExist(bucketName);
            if (!bucketExists)
            {
                return ApiResponse<bool>.Fail("Bucket was not found!", HttpStatusCode.NotFound);
            }
            
            await _minioService.RemoveBucket(bucketName);
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
            if (!bucketExists)
            {
                return ApiResponse<List<string>>.Fail("Bucket was not found!", HttpStatusCode.NotFound);
            }
            
            var listObjects = await _minioService.ListObjects(bucketName);
            return ApiResponse<List<string>>.Success(listObjects, HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return ApiResponse<List<string>>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
}