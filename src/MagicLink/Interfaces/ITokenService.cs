using MagicLink.Models;

namespace MagicLink.Interfaces;

public interface ITokenService
{
    string GenerateToken(string nameIdentifier);
    bool Validate(string token);
}