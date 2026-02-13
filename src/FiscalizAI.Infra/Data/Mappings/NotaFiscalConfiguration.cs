using FiscalizAI.Core.Entities;
using FiscalizAI.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiscalizAI.Infra.Data.Mappings;

public class NotaFiscalConfiguration : IEntityTypeConfiguration<NotaFiscal>
{
    public void Configure(EntityTypeBuilder<NotaFiscal> builder)
    {
        builder.ToTable("NotasFiscais");

        //PK BaseEntity
        builder.HasKey(nf => nf.Id);

        //Relacionamento OneToMany com Empresa
        builder.Property(nf => nf.EmpresaId)
            .IsRequired();

        builder.HasOne(nf => nf.Empresa)
            .WithMany()
            .HasForeignKey(nf => nf.EmpresaId)
            .OnDelete(DeleteBehavior.Cascade);

        //Chave de acesso com indice unico
        builder.Property(nf => nf.ChaveAcesso)
            .IsRequired()
            .HasMaxLength(44)
            .IsFixedLength();

        builder.HasIndex(nf => nf.ChaveAcesso)
            .IsUnique();

        builder.Property(nf => nf.Numero)
            .IsRequired()
            .HasColumnType("bigint");

        builder.Property(nf => nf.Serie)
            .IsRequired()
            .HasColumnType("bigint");
            
        //Value Object é  necessário transformar em string para armazenar
        //no banco e depois converter de volta para o tipo Cnpj na entidade
        builder.Property(nf => nf.CnpjEmitente)
            .HasConversion(
                cnpj => cnpj.Value, // Convertendo Cnpj para string ao salvar
                value => new Cnpj(value) // Convertendo string de volta para Cnpj ao ler
            )
            .IsRequired()
            .HasMaxLength(14)
            .IsFixedLength();

        builder.Property(nf => nf.ValorTotal)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(nf => nf.NomeEmitente)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(nf => nf.DataEmissao)
            .HasColumnType("timestamp without time zone");

        builder.Property(nf => nf.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(nf => nf.Tipo)
            .HasConversion<string>()
            .HasMaxLength(20);

        //Armazenamento das NF
        builder.Property(nf => nf.CaminhoStorage)
            .HasMaxLength(255);
    }
}
