using System.ComponentModel.DataAnnotations;

namespace FiscalizAI.Application.DTOs.Auth;

public record RefreshTokenRequestDto(
    [Required(ErrorMessage = "O access token é obrigatório")]
    string AccessToken,

    [Required(ErrorMessage = "O refresh token é obrigatório")]
    string RefreshToken
);
