
using Microsoft.AspNetCore.Mvc;

namespace minio_netcore_webapi.Controllers;

[ApiController]
public class BucketsController : ControllerBase
{
    // GET
    public IActionResult GetBucketList()
    {
        
        return Json();
    }
}