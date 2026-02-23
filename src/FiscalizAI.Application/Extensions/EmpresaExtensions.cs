using System.Security.Cryptography.X509Certificates;
using FiscalizAI.Application.DTOs.Empresa;
using FiscalizAI.Core.Domain.ValueObjects;
using FiscalizAI.Core.Entities;

namespace FiscalizAI.Application.Extensions;

public static class EmpresaExtensions
{
    public static EmpresaDto ToDto(this Empresa empresa)
    {
        return new EmpresaDto(
            empresa.Id,
            empresa.RazaoSocial,
            empresa.Cnpj.Value,
            empresa.Uf,
            empresa.DataVencimentoCertificado,
            empresa.UltimoNsu,
            empresa.Ativo,
            empresa.UltimaSincronizacao,
            empresa.DataCadastro,
            null);
    }

    public static DateTime? ExtrairDataVencimento(this byte[] certificadoDigital, string senha)
    {
        try
        {
            using var cert = new X509Certificate2(certificadoDigital, senha);
            return cert.NotAfter;
        }
        catch
        {
            return null;
        }
    }
    
    public static Empresa ToEntity(this CreateEmpresaDto dto, string senhaCriptografada, DateTime? dataVencimento)
    {
        return new Empresa(
            razaoSocial: dto.RazaoSocial,
            cnpj: new Cnpj(dto.Cnpj),
            uf: dto.Uf,
            certificadoDigital: dto.CertificadoDigital,
            senhaCertificado: senhaCriptografada,
            dataVencimentoCertificado: dataVencimento
        );
    }
}