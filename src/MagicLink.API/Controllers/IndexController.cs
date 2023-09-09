using MagicLink.API.Interfaces;
using MagicLink.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicLink.API.Controllers;

[ApiController]
[Route("v1/")]
public class IndexController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<IndexController> _logger;

    public IndexController(ITokenService tokenService, ILogger<IndexController> logger)
    {
        _logger = logger;
        _tokenService = tokenService;
    }

    [HttpPost("[action]")]
    public ActionResult Login([FromBody] User model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var token = _tokenService.GenerateToken(model.Email);
        // Queue e-mail
        return Ok(token);
    }

    [HttpGet("[action]/{token}")]
    public ActionResult Auth([FromRoute] string token)
    {
        if (!_tokenService.Validate(token))
        {
            return BadRequest();
        }
        return Ok();
    }
}
