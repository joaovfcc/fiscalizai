using FiscalizAI.Core.Entities;
using FiscalizAI.Core.Interfaces;
using FiscalizAI.Infra.Data;
using FiscalizAI.Infra.Identity;
using Microsoft.EntityFrameworkCore;

namespace FiscalizAI.Infra.Repositories;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly FiscalizAIContext _context;

    public EmpresaRepository(FiscalizAIContext context)
    {
        _context = context;
    }

    public async Task<Guid> AddAsync(Empresa empresa)
    {
        await _context.Empresas.AddAsync(empresa);
        return empresa.Id;
    }

    public async Task AddAcessoAsync(Guid empresaId, string usuarioId)
    {
        var acesso = new AcessoEmpresa
        {
            EmpresaId = empresaId,
            UserId = usuarioId
        };
        await _context.AcessoEmpresas.AddAsync(acesso);
    }

    public async Task<Empresa?> GetByIdAsync(Guid id, string usuarioId)
    {
        return await (from e in _context.Empresas
            join ae in _context.AcessoEmpresas on e.Id equals ae.EmpresaId
            where e.Id == id && ae.UserId == usuarioId
            select e).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Empresa>> GetAllByUsuarioIdAsync(string usuarioId)
    {
        return await (from e in _context.Empresas
            join ae in _context.AcessoEmpresas on e.Id equals ae.EmpresaId
            where ae.UserId == usuarioId
            select e).ToListAsync();
    }
}