using Ecom.Api.Helpers;

namespace Ecom.Api.Mapping;

public class ApiExceptions : ResponseApi
{
    public ApiExceptions(int statusCode, string? message = null, string details = null) : base(statusCode, message)
    {
        Details = details;
    }

    public string Details { get; set; }
}
