using FiscalizAI.Core.Common.Bases;
using FiscalizAI.Core.Common.Interfaces;
using FiscalizAI.Core.Common.Validation;
using FiscalizAI.Core.Domain.Exceptions;
using FiscalizAI.Core.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace FiscalizAI.Core.Entities;

public class Empresa : BaseEntity, IAggregateRoot
{
    public string RazaoSocial { get; private set; }
    public Cnpj Cnpj { get; private set; }
    public string Uf { get; private set; }
    public byte[] CertificadoDigital { get; private set; }
    public string SenhaCertificado { get; private set; }
    public DateTime? DataVencimentoCertificado { get; private set; }
    public long UltimoNsu { get; private set; }
    public bool Ativo { get; private set; } = true;
    public DateTime? UltimaSincronizacao { get; private set; }

    private Empresa()
    {
    }

    public Empresa(string razaoSocial, Cnpj cnpj, string uf
        , byte[] certificadoDigital, string senhaCertificado
        , DateTime? dataVencimentoCertificado)
    {
        RazaoSocial = razaoSocial;
        Cnpj = cnpj;
        Uf = uf.ToUpper();
        CertificadoDigital = certificadoDigital;
        SenhaCertificado = senhaCertificado;
        DataVencimentoCertificado = dataVencimentoCertificado;

        UltimoNsu = 0;
        Ativo = true;
        UltimaSincronizacao = null;

        Validate();
    }

    private void Validate()
    {
        AssertionConcern.AssertArgumentNotEmpty(RazaoSocial, "A Razão Social é obrigatória.");
        AssertionConcern.AssertArgumentNotNull(Cnpj, "O CNPJ é obrigatório.");
        AssertionConcern.AssertArgumentNotEmpty(Uf, "A UF é obrigatória.");
        AssertionConcern.AssertArgumentLength(Uf, 2, 2, "A UF deve conter exatamente 2 caracteres.");

        // Validações do Certificado
        AssertionConcern.AssertArgumentNotEmpty(SenhaCertificado, "A senha do certificado é obrigatória.");

        if (CertificadoDigital == null || CertificadoDigital.Length == 0)
            throw new DomainException("O arquivo do certificado não pode ser vazio.");

        if (DataVencimentoCertificado.HasValue && DataVencimentoCertificado.Value < DateTime.UtcNow)
            throw new DomainException("O certificado digital já está vencido.");
    }

    public void AtualizarCheckpoint(long novoNsu)
    {
        // Sempre atualizamos a data, pois o robô rodou com sucesso agora.
        UltimaSincronizacao = DateTime.UtcNow;

        // O NSU só muda se for maior (Lógica da Catraca)
        if (novoNsu > UltimoNsu)
        {
            UltimoNsu = novoNsu;
        }
    }


    public void AtualizarCertificado(byte[] novoCertificado, string novaSenha)
    {
        CertificadoDigital = novoCertificado;
        SenhaCertificado = novaSenha;
        Validate();
    }

    public void Desativar() => Ativo = false;
    public void Reativar() => Ativo = true;
}