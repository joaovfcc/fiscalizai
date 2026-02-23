using FiscalizAI.Core.Entities;
using FiscalizAI.Infra.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FiscalizAI.Infra.Data;
public class FiscalizAIContext : IdentityDbContext<ApplicationUser>
{
    public FiscalizAIContext(DbContextOptions<FiscalizAIContext> options) : base(options) { }

    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<NotaFiscal> NotasFiscais { get; set; }
    public DbSet<AcessoEmpresa> AcessoEmpresas { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FiscalizAIContext).Assembly);

        modelBuilder.Entity<ApplicationUser>(b =>
        {
            b.ToTable("AspNetUsers");
            b.Property(b => b.NomeCompleto).HasMaxLength(100);
            b.Property(b => b.RefreshToken).HasMaxLength(256);
            b.Property(b => b.RefreshTokenExpiryTime).HasColumnType("timestamp with time zone");
           
        });

        modelBuilder.Entity<AcessoEmpresa>(b =>
        {
            b.ToTable("AcessoEmpresas"); // Nome da tabela no banco

            // A CHAVE MÁGICA: Chave Composta (Isso impede duplicidade de vínculo)
            b.HasKey(ue => new { ue.UserId, ue.EmpresaId });

            // Relacionamento com o Usuário (Lado String do Identity)
            b.HasOne(ue => ue.User)
                .WithMany(u => u.AcessoEmpresas) // Certifique-se que ApplicationUser tem essa lista
                .HasForeignKey(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Se deletar o user, apaga o vínculo

            // Relacionamento com a Empresa (Lado Guid)
            b.HasOne(ue => ue.Empresa)
                .WithMany() 
                .HasForeignKey(ue => ue.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}

