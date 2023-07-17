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
            if (!bucketExists)
            {
                return ApiResponse<bool>.Fail("Bucket was not found!", HttpStatusCode.BadRequest);
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
    
    public async Task<ApiResponse<bool>> RemoveObject(string bucketName, string objectName)
    {
        try
        {
            var bucketExists = await _minioService.BucketExist(bucketName);
            if (!bucketExists)
            {
                return ApiResponse<bool>.Fail("Bucket was not found!", HttpStatusCode.BadRequest);
            }
            
            await _minioService.RemoveObject(bucketName, objectName);
            return ApiResponse<bool>.Success(true, HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return ApiResponse<bool>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
    
    public async Task<ApiResponse<MemoryStream>> GetObject(string bucketName, string objectName, string fileName)
    {
        try
        {
            var bucketExists = await _minioService.BucketExist(bucketName);
            if (!bucketExists)
            {
                return ApiResponse<MemoryStream>.Fail("Bucket was not found!", HttpStatusCode.BadRequest);
            }
            
            var stream = await _minioService.GetObject(bucketName, objectName, fileName);
            return ApiResponse<MemoryStream>.Success(stream, HttpStatusCode.OK);
        }
        catch (Exception e)
        {
            return ApiResponse<MemoryStream>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
    
    public string GetContentType(string fileName)
    {
        if (fileName.Contains(".jpg"))
        {
            return "image/jpg";
        }
        else if (fileName.Contains(".jpeg"))
        {
            return "image/jpeg";
        }
        else if (fileName.Contains(".png"))
        {
            return "image/png";
        }
        else if (fileName.Contains(".gif"))
        {
            return "image/gif";
        }
        else if (fileName.Contains(".pdf"))
        {
            return "application/pdf";
        }
        else
        {
            return "application/octet-stream";
        }
    }
}