using System.ComponentModel.DataAnnotations;

namespace FiscalizAI.Application.DTOs.Empresa;

public record CreateEmpresaDto(
    [Required(ErrorMessage = "A razão social é obrigatória")]
    [StringLength(200, ErrorMessage = "A razão social deve ter no máximo 200 caracteres")]
    string RazaoSocial,

    [Required(ErrorMessage = "O CNPJ é obrigatório")]
    string Cnpj,

    [Required(ErrorMessage = "A UF é obrigatória")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "A UF deve ter exatamente 2 caracteres")]
    string Uf,

    [Required(ErrorMessage = "O certificado digital é obrigatório")]
    byte[] CertificadoDigital,

    [Required(ErrorMessage = "A senha do certificado é obrigatória")]
    string SenhaCertificado
);
