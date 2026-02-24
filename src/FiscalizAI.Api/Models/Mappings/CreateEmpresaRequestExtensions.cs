using FiscalizAI.Api.Models.Requests;
using FiscalizAI.Application.DTOs.Empresa;

namespace FiscalizAI.Api.Models.Mappings;

public static class CreateEmpresaRequestExtensions
{
    public static CreateEmpresaDto ToDto(this CreateEmpresaRequest request, byte[] certificadoBytes)
    {
        return new CreateEmpresaDto(
            RazaoSocial: request.RazaoSocial,
            Cnpj: request.Cnpj,
            Uf: request.Uf,
            CertificadoDigital: certificadoBytes,
            SenhaCertificado: request.SenhaCertificado
        );
    }
}
