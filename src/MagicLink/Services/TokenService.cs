using System;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using MagicLink.Interfaces;

namespace MagicLink.Services;

public class TokenService : ITokenService
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly string _securityKey;

    public TokenService(string securityKey)
    {
        _securityKey = securityKey;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public string GenerateToken(string nameIdentifier)
    {
        var securityKey = Encoding.ASCII.GetBytes(_securityKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, nameIdentifier)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(securityKey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    public bool Validate(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentNullException(nameof(token));
        }

        var securityKey = Encoding.ASCII.GetBytes(_securityKey);
        
        try
        {
            _tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

           // var jwtToken = (JwtSecurityToken)validatedToken;
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }
}