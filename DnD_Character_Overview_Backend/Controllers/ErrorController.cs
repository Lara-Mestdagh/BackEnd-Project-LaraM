using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    [HttpGet]
    [Route("error")]
    public IActionResult HandleError()
    {
        // Log the error
        Serilog.Log.Error("An error occurred in the application.");

        // Your error handling logic here
        return Problem();
    }
}
