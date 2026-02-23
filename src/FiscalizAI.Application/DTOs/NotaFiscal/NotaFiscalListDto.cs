namespace FiscalizAI.Application.DTOs.NotaFiscal;

public record NotaFiscalListDto(
    Guid Id,
    Guid EmpresaId,
    string ChaveAcesso,
    long Numero,
    int Serie,
    string CnpjEmitente,
    string NomeEmitente,
    DateTime DataEmissao,
    decimal ValorTotal,
    string Status,
    string Tipo
);
