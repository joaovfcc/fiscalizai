using FiscalizAI.Core.Entities;

namespace FiscalizAI.Core.Interfaces;

public interface IEmpresaRepository
{
    Task<Guid> AddAsync(Empresa empresa);
    Task AddAcessoAsync(Guid empresaId, string usuarioId);
    Task<Empresa?> GetByIdAsync(Guid id, string usuarioId);
    Task<IEnumerable<Empresa>> GetAllByUsuarioIdAsync(string usuarioId);
}
