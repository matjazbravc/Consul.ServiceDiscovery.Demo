using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ValueService.OpenApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
  private readonly HttpContext _context;
  private readonly ILogger<ValuesController> _logger;
  private readonly string _baseUrl;

  public ValuesController(ILogger<ValuesController> logger, IHttpContextAccessor context)
  {
    if (context.HttpContext != null)
    {
      _context = context.HttpContext;
      _baseUrl = $"{_context.Request.Scheme}://{_context.Request.Host}";
    }
    _logger = logger;
  }

  [HttpGet("badcode")]
  public string BadCode()
  {
    throw new Exception("Some bad code was executed!");
  }

  [HttpGet]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult Get()
  {
    var ocelotReqId = _context.Request.Headers["OcelotRequestId"];
    if (string.IsNullOrEmpty(ocelotReqId))
    {
      ocelotReqId = "N/A";
    }
    var msg = $"Url: {_baseUrl}, Method: {_context.Request.Method}, Path: {_context.Request.Path}, OcelotRequestId: {ocelotReqId}";
    _logger.LogInformation(msg);
    return Ok(msg);
  }

  [HttpGet("healthcheck")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult Healthcheck()
  {
    var msg = $"{_context.Request.Host} is healthy";
    _logger.LogInformation(msg);
    return Ok(msg);
  }

  [HttpGet("status")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult Status()
  {
    var msg = $"Running on {_context.Request.Host}";
    _logger.LogInformation(msg);
    return Ok(msg);
  }
}
