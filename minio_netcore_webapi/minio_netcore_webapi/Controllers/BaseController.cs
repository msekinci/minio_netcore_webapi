using System.Net;
using Microsoft.AspNetCore.Mvc;
using minio_netcore_webapi.Models;

namespace minio_netcore_webapi.Controllers;

[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    [NonAction]
    public IActionResult CreateActionResult<T>(ApiResponse<T> response)
    {
        return new ObjectResult(response.Status == HttpStatusCode.NoContent ? null : response)
        {
            StatusCode = response.Status.GetHashCode()
        };
    }
}