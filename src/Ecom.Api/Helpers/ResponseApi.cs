namespace Ecom.Api.Helpers;

public class ResponseApi
{
    public ResponseApi(int statusCode, string? message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetMessageFromStatusCode(statusCode);
    }

    public int StatusCode { get; set; }
    public string? Message { get; set; }

    private string GetMessageFromStatusCode(int statusCode)
    {
        return statusCode switch
        {
            200 => "OK",
            201 => "Created",
            400 => "Bad Request",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Unknown Status"
        };
    }
}
