using FiscalizAI.Application.DTOs.Empresa;

namespace FiscalizAI.Application.Interfaces;

public interface IEmpresaService
{
    Task<EmpresaDto> CriarAsync(CreateEmpresaDto dto, string usuarioId);
    Task<EmpresaDto?> ObterPorIdAsync(Guid id, string usuarioId);
    Task<IEnumerable<EmpresaDto>> ListarPorUsuarioIdAsync(string usuarioId);
}
