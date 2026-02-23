using System.ComponentModel.DataAnnotations;

namespace FiscalizAI.Application.DTOs.Empresa;

public record UpdateEmpresaDto(
    [Required(ErrorMessage = "A razão social é obrigatória")]
    [StringLength(200, ErrorMessage = "A razão social deve ter no máximo 200 caracteres")]
    string RazaoSocial,

    [Required(ErrorMessage = "A UF é obrigatória")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "A UF deve ter exatamente 2 caracteres")]
    string Uf,

    bool Ativo
);
