using MagicLink.API.Models;

namespace MagicLink.API.Interfaces;

public interface ITokenService
{
    string GenerateToken(string nameIdentifier);
    bool Validate(string token);
}