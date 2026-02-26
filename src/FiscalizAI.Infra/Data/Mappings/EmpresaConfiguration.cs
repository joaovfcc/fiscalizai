using FiscalizAI.Core.Entities;
using FiscalizAI.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiscalizAI.Infra.Data.Mappings;

public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
{
    public void Configure(EntityTypeBuilder<Empresa> builder)
    {
        builder.ToTable("Empresas");

        //PK BaseEntity
        builder.HasKey(e => e.Id);

        //Propriedades simples
        builder.Property(e => e.RazaoSocial)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType("varchar(200)");

        builder.Property(e => e.Uf)
            .HasMaxLength(2)
            .IsFixedLength()
            .HasColumnType("char(2)");

        builder.Property(e => e.SenhaCertificado)
            .IsRequired()
            .HasMaxLength(255);

        //Value Object é  necessário transformar em string para armazenar
        //no banco e depois converter de volta para o tipo Cnpj na entidade
        builder.Property(e => e.Cnpj)
            .HasConversion(
                cnpj => cnpj.Value, // Converte Cnpj para string ao salvar no banco
                value => new Cnpj(value) // Converte string de volta para Cnpj ao ler do banco
            )
            .IsRequired()
            .HasMaxLength(14)
            .IsFixedLength();

        // Índice único para o CNPJ
        builder.HasIndex(e => e.Cnpj).IsUnique();

        builder.Property(e => e.CertificadoDigital)
            .IsRequired()
            .HasColumnType("bytea");

        builder.Property(e => e.DataVencimentoCertificado)
            .HasColumnType("timestamp without time zone");

            builder.Property(e => e.UltimaSincronizacao)
            .HasColumnType("timestamp without time zone");
        
        //NSU Definido por padrão como 
        builder.Property(e => e.UltimoNsu)
            .HasDefaultValue(0);

        builder.Property(e => e.Ativo)
            .HasDefaultValue(true);



    }
}
