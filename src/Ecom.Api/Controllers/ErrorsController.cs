using Ecom.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers;
[Route("errors/{statusCode}")]
[ApiController]
public class ErrorsController : ControllerBase
{
    [HttpGet]
    public IActionResult Error(int statusCode)
    {
        return new ObjectResult(new 
            ResponseApi(statusCode, $"An error occurred with status code {statusCode}"));
      
    }
}
