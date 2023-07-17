using Microsoft.AspNetCore.Mvc;
using minio_netcore_webapi.Models;
using minio_netcore_webapi.Services;

namespace minio_netcore_webapi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BucketsController : BaseController
{
    private readonly BucketService _bucketService;

    public BucketsController(BucketService bucketService)
    {
        _bucketService = bucketService;
    }
    
    [HttpPost]
    public async Task<IActionResult> MakeBucket([FromBody] MakeBucketDto model)
    {
        return CreateActionResult(await _bucketService.MakeBucket(model.BucketName));
    }
    
    [HttpPost]
    public async Task<IActionResult> MakeBucketWithLock([FromBody] MakeBucketDto model)
    {
        return CreateActionResult(await _bucketService.MakeBucketWithLock(model.BucketName));
    }
    
    [HttpGet]
    public async Task<IActionResult> ListBuckets()
    {
        return CreateActionResult(await _bucketService.ListBuckets());
    }
    
    [HttpGet("{bucketName}")]
    public async Task<IActionResult> BucketExist(string bucketName)
    {
        return CreateActionResult(await _bucketService.BucketExist(bucketName));
    }
    
    [HttpDelete("{bucketName}")]
    public async Task<IActionResult> RemoveBucket(string bucketName)
    {
        return CreateActionResult(await _bucketService.RemoveBucket(bucketName));
    }
    
    [HttpGet("{bucketName}")]
    public async Task<IActionResult> ListObjects(string bucketName)
    {
        return CreateActionResult(await _bucketService.ListObjects(bucketName));
    }
}