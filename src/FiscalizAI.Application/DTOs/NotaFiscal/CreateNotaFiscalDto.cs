using System.ComponentModel.DataAnnotations;
using FiscalizAI.Core.Enums;

namespace FiscalizAI.Application.DTOs.NotaFiscal;

public record CreateNotaFiscalDto(
    [Required(ErrorMessage = "O ID da empresa é obrigatório")]
    Guid EmpresaId,

    [Required(ErrorMessage = "A chave de acesso é obrigatória")]
    [StringLength(44, MinimumLength = 44, ErrorMessage = "A chave de acesso deve ter 44 caracteres")]
    string ChaveAcesso,

    [Required(ErrorMessage = "O número é obrigatório")]
    long Numero,

    [Required(ErrorMessage = "A série é obrigatória")]
    int Serie,

    [Required(ErrorMessage = "O CNPJ do emitente é obrigatório")]
    string CnpjEmitente,

    [Required(ErrorMessage = "O nome do emitente é obrigatório")]
    string NomeEmitente,

    [Required(ErrorMessage = "A data de emissão é obrigatória")]
    DateTime DataEmissao,

    [Required(ErrorMessage = "O valor total é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor total deve ser maior que zero")]
    decimal ValorTotal,

    [Required(ErrorMessage = "O status é obrigatório")]
    StatusNota Status,

    [Required(ErrorMessage = "O tipo de operação é obrigatório")]
    TipoOperacao Tipo,

    [Required(ErrorMessage = "O caminho no storage é obrigatório")]
    string CaminhoStorage
);
