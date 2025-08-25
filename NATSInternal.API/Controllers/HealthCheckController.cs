namespace NATSInternal.Controllers;

[Route("Api/HealthCheck")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    #region StaticFields
    private static readonly DateTime _startedDateTime = DateTime.UtcNow.ToApplicationTime();
    #endregion

    #region Methods
    [HttpGet("Ping")]
    public IActionResult Ping()
    {
        return Ok();
    }

    [HttpGet("StartedDateTime")]
    public IActionResult StartedDateTime()
    {
        return Ok(_startedDateTime);
    }

    [HttpGet("RunningTime")]
    public IActionResult RunningTime()
    {
        return Ok(DateTime.UtcNow.ToApplicationTime() - _startedDateTime);
    }
    #endregion
}