using System.ComponentModel.DataAnnotations;

namespace MagicLink.API.Models;

public record User([EmailAddress] string Email);