namespace FiscalizAI.Application.DTOs.Empresa;

public record EmpresaDto(
    Guid Id,
    string RazaoSocial,
    string Cnpj,
    string Uf,
    DateTime? DataVencimentoCertificado,
    long UltimoNsu,
    bool Ativo,
    DateTime? UltimaSincronizacao,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
