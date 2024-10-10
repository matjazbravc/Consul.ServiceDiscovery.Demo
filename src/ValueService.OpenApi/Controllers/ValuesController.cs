using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace ValueService.OpenApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
  private readonly ILogger<ValuesController> _logger;
  private readonly string _baseUrl;
  private readonly HttpRequest _httpRequest;

  public ValuesController(ILogger<ValuesController> logger, IHttpContextAccessor context)
  {
    if (context.HttpContext != null)
    {
      _httpRequest = context.HttpContext.Request;
      _baseUrl = $"{_httpRequest.Scheme}://{_httpRequest.Host}";
    }
    _logger = logger;
  }

  [HttpGet("badcode")]
  public string BadCode() => throw new ArgumentException("Some bad code was executed!");

  [HttpGet]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult Get()
  {
    IHeaderDictionary headers = _httpRequest.Headers;
    StringValues ocelotReqId = headers["OcelotRequestId"];
    if (string.IsNullOrEmpty(ocelotReqId))
    {
      ocelotReqId = "N/A";
    }
    string msg = $"Url: {_baseUrl}, Method: {_httpRequest.Method}, Path: {_httpRequest.Path}, OcelotRequestId: {ocelotReqId}";
    _logger.LogInformation("{Message}", msg);
    return Ok(msg);
  }

  [HttpGet("healthcheck")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult Healthcheck()
  {
    string msg = $"{_httpRequest.Host} is healthy";
    _logger.LogInformation("{Message}", msg);
    return Ok(msg);
  }

  [HttpGet("status")]
  [ProducesResponseType<string>(StatusCodes.Status200OK)]
  public IActionResult Status()
  {
    string msg = $"Running on {_httpRequest.Host}";
    _logger.LogInformation("{Message}", msg);
    return Ok(msg);
  }
}
