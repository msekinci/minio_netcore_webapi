using Microsoft.AspNetCore.Mvc;
using minio_netcore_webapi.Models;
using minio_netcore_webapi.Services;

namespace minio_netcore_webapi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ObjectsController : BaseController
{
    private readonly ObjectService _objectService;

    public ObjectsController(ObjectService objectService)
    {
        _objectService = objectService;
    }

    [HttpPost("{bucketName}")]
    public async Task<IActionResult> PutObject(string bucketName, [FromForm] PutObjectDto model)
    {
        return CreateActionResult(await _objectService.PutObject(bucketName, model.File));
    }
    
    [HttpGet("{bucketName}/{objectName}")]
    public async Task<IActionResult> GetObject(string bucketName, string objectName, [FromQuery]string fileName)
    {
        
        var result = await _objectService.GetObject(bucketName, objectName, fileName);
        if (result.Error == null)
        {
            return File(result.Data!, _objectService.GetContentType(fileName), fileName);
        }
        
        return CreateActionResult(await _objectService.GetObject(bucketName, objectName, fileName));
    }
    
    [HttpDelete("{bucketName}/{objectName}")]
    public async Task<IActionResult> ListObjects(string bucketName, string objectName)
    {
        return CreateActionResult(await _objectService.RemoveObject(bucketName, objectName));
    }
}