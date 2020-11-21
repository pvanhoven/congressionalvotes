using System;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("")]
public class HealthCheckController : ControllerBase
{
    [HttpGet("heartbeat")]
    public DateTime GetCurrentUtcDate() => DateTime.UtcNow;
}