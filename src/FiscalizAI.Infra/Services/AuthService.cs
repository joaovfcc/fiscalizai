using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FiscalizAI.Application.DTOs.Auth;
using FiscalizAI.Application.Interfaces;
using FiscalizAI.Infra.Identity;
using Microsoft.AspNetCore.Identity;

namespace FiscalizAI.Infra.Services;

/// <summary>
/// Serviço responsável por gerenciar autenticação e autorização de usuários.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    
    public AuthService(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Realiza o login do usuário e retorna tokens de acesso e refresh.
    /// </summary>
    public async Task<TokenResult> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            throw new UnauthorizedAccessException("Credenciais inválidas");
        }

        var accessToken = GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new TokenResult(accessToken, refreshToken);
    }

    /// <summary>
    /// Registra um novo usuário no sistema.
    /// </summary>
    public async Task<bool> RegistrarAsync(RegistroDto dto)
    {
        var userExists = await _userManager.FindByEmailAsync(dto.Email);
        if (userExists != null)
        {
            throw new InvalidOperationException("Email já cadastrado");
        }

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            NomeCompleto = dto.NomeCompleto
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Erro ao criar usuário: {errors}");
        }

        return true;
    }

    /// <summary>
    /// Renova os tokens de acesso e refresh utilizando um token expirado.
    /// </summary>
    public async Task<TokenResult> RefreshTokenAsync(RefreshTokenRequestDto dto)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(dto.AccessToken);
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("Token inválido");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.RefreshToken != dto.RefreshToken)
        {
            throw new UnauthorizedAccessException("Refresh token inválido");
        }

        if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Refresh token expirado");
        }

        var newAccessToken = GenerateJwtToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return new TokenResult(newAccessToken, newRefreshToken);
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.NomeCompleto)
        };

        var token = _tokenService.GenerateAccessToken(claims);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
