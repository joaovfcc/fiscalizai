using System.ComponentModel.DataAnnotations;

namespace FiscalizAI.Application.DTOs.Auth;

public record LoginDto(
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    string Email,

    [Required(ErrorMessage = "A senha é obrigatória")]
    string Password
);
