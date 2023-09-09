using System;
using System.Text;

using MagicLink.API.Interfaces;
using MagicLink.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicLink.API.Controllers;

[ApiController]
[Route("v1/")]
public class IndexController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<IndexController> _logger;

    private readonly string _htmlTemplate = @"<!DOCTYPE html><html lang='en'><head><meta charset='UTF-8'><meta name='viewport' content='width=device-width, initial-scale=1.0'><title>Login to Our Application</title></head><body><div style='max-width:600px;margin:0 auto;padding:20px;'><h1>Login to Our Application</h1><p>Click the button below to log in to our application using a magic link:</p><a href='{magicLink}' style='display:inline-block;padding:10px 20px;background-color:#007BFF;color:#ffffff;text-decoration:none;font-weight:bold;border-radius:5px;'>Login with Magic Link</a><p>If you did not request this login or have any questions, please contact our support team.</p></div></body></html>";

    public IndexController(
        ITaskService taskService,
        ITokenService tokenService,
        ILogger<IndexController> logger)
    {
        _logger = logger;

        _taskService = taskService;
        _tokenService = tokenService;
    }

    [HttpPost("[action]")]
    public ActionResult Login([FromBody] User model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        _logger.LogInformation("Generating web token");
        var token = _tokenService.GenerateToken(model.Email);
        
        _logger.LogInformation("Creating message");
        var message = new Message
        {
            From = model.Email,
            To = model.Email,
            Body = _htmlTemplate.Replace("magicLink", token)
        };

        _taskService.SendMessage(Encoding.UTF8.GetBytes(message.ToJson()));
        
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
