using System.Net;

namespace minio_netcore_webapi.Models;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public string? Error { get; set; }
    public HttpStatusCode Status { get; set; }

    public static ApiResponse<T> Success(T data, HttpStatusCode status)
    {
        return new ApiResponse<T>
        {
            Data = data,
            Status = status
        };
    }

    public static ApiResponse<T> Fail(string error, HttpStatusCode status)
    {
        return new ApiResponse<T>
        {
            Error = error,
            Status = status
        };
    }
}