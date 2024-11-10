using NATSInternal.Services.Localization;

namespace NATSInternal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UtilityController : ControllerBase
{
    [HttpGet("DisplayNames")]
    public IActionResult GetDisplayNames()
    {
        return Ok(DisplayNames.GetAll());
    }
}