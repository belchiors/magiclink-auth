using System.ComponentModel.DataAnnotations;

namespace MagicLink.Models;

public record User([EmailAddress] string Email);