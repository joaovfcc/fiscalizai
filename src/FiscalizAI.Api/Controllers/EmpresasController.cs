using System.Security.Claims;
using FiscalizAI.Api.Models.Mappings;
using FiscalizAI.Api.Models.Requests;
using FiscalizAI.Application.DTOs.Empresa;
using FiscalizAI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiscalizAI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmpresasController : ControllerBase
{
    private readonly IEmpresaService _empresaService;

    public EmpresasController(IEmpresaService empresaService)
    {
        _empresaService = empresaService;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromForm] CreateEmpresaRequest request)
    {
        var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(usuarioId))
        {
            return Unauthorized(new { Erro = "Usuário não identificado no token." });
        }

        using var memoryStream = new MemoryStream();
        await request.ArquivoCertificado.CopyToAsync(memoryStream);
        var certificadoBytes = memoryStream.ToArray();

        var dto = request.ToDto(certificadoBytes);
        var resultado = await _empresaService.CriarAsync(dto, usuarioId);

        return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(usuarioId))
        {
            return Unauthorized();
        }

        var empresa = await _empresaService.ObterPorIdAsync(id, usuarioId);

        if (empresa == null)
        {
            return NotFound(new { Erro = "Empresa não encontrada ou acesso negado." });
        }

        return Ok(empresa);
    }
}
