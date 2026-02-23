using FiscalizAI.Application.DTOs.Auth;

namespace FiscalizAI.Application.Interfaces;

public interface IAuthService
{
    Task<TokenResult> LoginAsync(LoginDto dto);
    Task<bool> RegistrarAsync(RegistroDto dto);
    Task<TokenResult> RefreshTokenAsync(RefreshTokenRequestDto dto);
}
