﻿namespace NATSInternal.Controllers;

[Route("Api/HealthCheck")]
[ApiController]
public class HealthCheckController : ControllerBase
{
    private static readonly DateTime _startedDateTime = DateTime.UtcNow.ToApplicationTime();

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
}