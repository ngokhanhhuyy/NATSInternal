namespace NATSInternal.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthCheckController : Controller
{
    [HttpGet("Ping")]
    public IActionResult Ping()
    {
        return Ok();
    }
}