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

        modelBuilder.Entity<ApplicationUser>(builder =>
        {
            builder.ToTable("AspNetUsers");
            builder.Property(user => user.NomeCompleto).HasMaxLength(100);
            builder.Property(user => user.RefreshToken).HasMaxLength(256);
            builder.Property(user => user.RefreshTokenExpiryTime).HasColumnType("timestamp with time zone");
           
        });

        modelBuilder.Entity<AcessoEmpresa>(builder =>
        {
            builder.ToTable("AcessoEmpresas"); // Nome da tabela no banco

            // A CHAVE MÁGICA: Chave Composta (Isso impede duplicidade de vínculo)
            builder.HasKey(ae => new { ae.UserId, ae.EmpresaId });

            // Relacionamento com o Usuário (Lado String do Identity)
            builder.HasOne(ae => ae.User)
                .WithMany(u => u.AcessoEmpresas) // Certifique-se que ApplicationUser tem essa lista
                .HasForeignKey(ae => ae.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Se deletar o user, apaga o vínculo

            // Relacionamento com a Empresa (Lado Guid)
            builder.HasOne(ae => ae.Empresa)
                .WithMany() 
                .HasForeignKey(ae => ae.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}

