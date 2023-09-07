using MagicLink.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicLink.Controllers;

[ApiController]
[Route("v1/")]
public class IndexController : ControllerBase
{
    private readonly ILogger<IndexController> _logger;

    public IndexController(ILogger<IndexController> logger)
    {
        _logger = logger;
    }

    [HttpPost("[action]")]
    public ActionResult Login([FromBody] User model)
    {
        return Ok("It's working!");
    }

    [HttpGet("[action]")]
    public ActionResult Auth(string token)
    {
        return Ok("It's working!");
    }
}
