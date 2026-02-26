using System.Security.Cryptography.X509Certificates;
using FiscalizAI.Application.DTOs.Empresa;
using FiscalizAI.Application.Extensions;
using FiscalizAI.Application.Interfaces;
using FiscalizAI.Core.Domain.ValueObjects;
using FiscalizAI.Core.Entities;
using FiscalizAI.Core.Interfaces;

namespace FiscalizAI.Application.Services;

public class EmpresaService : IEmpresaService
{
    private readonly IEmpresaRepository _repository;
    private readonly ICryptographyService _cryptographyService;
    private readonly IUnitOfWork _unitOfWork;
    
    public EmpresaService(IEmpresaRepository repository, ICryptographyService cryptographyService, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _cryptographyService = cryptographyService;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmpresaDto> CriarAsync(CreateEmpresaDto dto, string usuarioId)
    {
        var senhaCriptografada = _cryptographyService.Encrypt(dto.SenhaCertificado);

        var dataVencimento = dto.CertificadoDigital.ExtrairDataVencimento(dto.SenhaCertificado);

        var empresa = dto.ToEntity(senhaCriptografada, dataVencimento);

        var empresaId = await _repository.AddAsync(empresa);
        
        await _repository.AddAcessoAsync(empresaId, usuarioId);
        await _unitOfWork.CommitAsync();

        return empresa.ToDto();
    }

    public async Task<EmpresaDto?> ObterPorIdAsync(Guid id, string usuarioId)
    {
        var empresa = await _repository.GetByIdAsync(id, usuarioId);

        if (empresa == null)
            return null;

        return empresa.ToDto();
    }

    public async Task<IEnumerable<EmpresaDto>> ListarPorUsuarioIdAsync(string usuarioId)
    {
        var empresas = await _repository.GetAllByUsuarioIdAsync(usuarioId);

        return empresas.Select(e => e.ToDto()).ToList();
    }
    
}
