# Especificação do Controlador: EmpresasController

## Contexto
Atue como um Desenvolvedor C# Sênior especialista em Clean Architecture e ASP.NET Core.
Sua tarefa é implementar a camada de Apresentação (API) para a entidade "Empresa", focando exclusivamente na criação do controlador. A fronteira anti-corrupção e as validações (Data Annotations) já estão definidas na classe `CreateEmpresaRequest`.

## Arquivo a ser criado/atualizado
`src/FiscalizAI.Api/Controllers/EmpresasController.cs`

## Definição da Classe
- **Namespace:** `FiscalizAI.Api.Controllers`
- A classe deve herdar de `ControllerBase`.
- **Atributos obrigatórios da classe:** - `[ApiController]`
  - `[Route("api/[controller]")]`
  - `[Authorize]`
- **Injeção de Dependência:** Injete a interface `IEmpresaService` via construtor.

## Endpoints

### 1. POST - Criar Empresa
- **Assinatura:** `[HttpPost]` `public async Task<IActionResult> Criar([FromForm] CreateEmpresaRequest request)`
- **Regras de Negócio do Endpoint:**
  1. **Autenticação:** Extraia o ID do usuário logado usando `User.FindFirstValue(ClaimTypes.NameIdentifier)`. Se o ID for nulo ou vazio, retorne `Unauthorized(new { Erro = "Usuário não identificado no token." })`.
  2. **Tratamento de Arquivo:** Leia a propriedade `request.ArquivoCertificado` (tipo `IFormFile`) usando um `MemoryStream` e extraia o `byte[]`.
  3. **Mapeamento:** Instancie o `CreateEmpresaDto` (da camada Application) passando as propriedades de texto do `request` e o `byte[]` recém-gerado.
  4. **Persistência:** Chame `var resultado = await _empresaService.CriarAsync(dto, usuarioId)`.
  5. **Retorno:** Retorne status HTTP 201 Created chamando `CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado)`.

### 2. GET - Obter Empresa por ID
- **Assinatura:** `[HttpGet("{id:guid}")]` `public async Task<IActionResult> ObterPorId(Guid id)`
- **Regras de Negócio do Endpoint:**
  1. **Autenticação:** Extraia o ID do usuário logado via Claims (`ClaimTypes.NameIdentifier`). Se não houver, retorne `Unauthorized()`.
  2. **Busca:** Chame `var empresa = await _empresaService.ObterPorIdAsync(id, usuarioId)`.
  3. **Retorno:** Se retornar nulo, devolva `NotFound(new { Erro = "Empresa não encontrada ou acesso negado." })`. Caso contrário, devolva `Ok(empresa)`.

## Regras Estritas de Arquitetura (Guardrails)
- **Isolamento da Web:** NÃO vaze o tipo `IFormFile` para a chamada do Serviço. A comunicação com o `IEmpresaService` deve ser estritamente via `byte[]`.
- **Mapeamento (Extension Methods):** Não utilize bibliotecas de mapeamento (ex: AutoMapper) e evite a instanciação manual poluidora direto no método do Controller. Em vez disso, utilize (ou crie na camada da API) um Extension Method estático (ex: `request.ToDto(bytesCertificado)`) para converter o `CreateEmpresaRequest` no `CreateEmpresaDto`, mantendo a classe do Controller extremamente enxuta e focada no fluxo HTTP.