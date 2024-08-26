using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class ErrorController : ControllerBase
{
    [HttpGet]
    [Route("error")]
    public IActionResult HandleError()
    {
        // Your error handling logic here
        return Problem();
    }
}
