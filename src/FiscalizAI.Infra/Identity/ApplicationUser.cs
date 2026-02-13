using Microsoft.AspNetCore.Identity;

namespace FiscalizAI.Infra.Identity;

    public class ApplicationUser : IdentityUser
    {
    public string NomeCompleto { get;  set; }
    public string? RefreshToken { get;  set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public ICollection<AcessoEmpresa> AcessoEmpresas { get; set; }
}

