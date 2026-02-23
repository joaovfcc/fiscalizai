using System.ComponentModel.DataAnnotations;

namespace FiscalizAI.Application.DTOs.Empresa;

public record UpdateCertificadoDto(
    [Required(ErrorMessage = "O certificado digital é obrigatório")]
    byte[] CertificadoDigital,

    [Required(ErrorMessage = "A senha do certificado é obrigatória")]
    string SenhaCertificado,

    DateTime? DataVencimentoCertificado
);
