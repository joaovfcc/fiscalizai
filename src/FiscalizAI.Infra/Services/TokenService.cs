using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FiscalizAI.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FiscalizAI.Infra.Services;

/// <summary>
/// Serviço responsável pelo ciclo de vida de tokens JWT (geração, refresh e extração de claims).
/// </summary>
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Gera um token de acesso JWT com as claims especificadas.
    /// </summary>
    public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = _configuration["Jwt:Key"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"]!);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        if (securityKey.KeySize < 256)
            throw new ArgumentException("A chave JWT deve ter pelo menos 256 bits (32 caracteres)");

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials);
    }

    /// <summary>
    /// Gera um refresh token criptograficamente seguro (64 bytes em Base64).
    /// </summary>
    /// <remarks>
    /// Utiliza <see cref="RandomNumberGenerator"/> em vez de Guid para garantir entropia adequada contra ataques de força bruta.
    /// </remarks>
    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    /// <summary>
    /// Extrai o principal de um token JWT ignorando sua data de expiração, utilizado exclusivamente para o fluxo de Refresh Token.
    /// </summary>
    /// <remarks>
    /// A validação de lifetime está desabilitada (<c>ValidateLifetime = false</c>), mas a assinatura, issuer e audience ainda são rigorosamente validadas.
    /// </remarks>
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false, // Ignora a expiração para permitir o refresh
            ValidateIssuerSigningKey = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
            TokenReplayCache = null
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Token inválido ou algoritmo alterado.");
        }

        return principal;
    }
}