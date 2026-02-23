using FiscalizAI.Core.Enums;

namespace FiscalizAI.Application.DTOs.NotaFiscal;

public record NotaFiscalDto(
    Guid Id,
    Guid EmpresaId,
    string ChaveAcesso,
    long Numero,
    int Serie,
    string CnpjEmitente,
    string NomeEmitente,
    DateTime DataEmissao,
    decimal ValorTotal,
    StatusNota Status,
    TipoOperacao Tipo,
    string CaminhoStorage,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
