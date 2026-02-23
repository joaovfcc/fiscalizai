Atue como um Desenvolvedor C# Sênior especialista em Clean Architecture.

Estamos iniciando o Épico de Gestão de Empresas. Preciso que você crie as interfaces, a implementação do serviço de negócios (Application) e a implementação do repositório de dados (Infraestrutura).

Passo 1: Leitura de Contexto Obrigatória
Antes de escrever qualquer código, leia o conteúdo dos seguintes arquivos:

A entidade Empresa.cs e a entidade de vínculo UsuarioEmpresa.cs (ou como estiver mapeado o N:N no Core/Domain).

O seu DbContext (na camada Infra/Data) para entender os DbSet existentes.

Os DTOs CreateEmpresaDto.cs e EmpresaDto.cs na camada Application.

A interface ICryptographyService.cs (ou similar) na camada Application/Interfaces.

Passo 2: Criação dos Contratos (Interfaces)

Crie IEmpresaRepository em FiscalizAI.Core/Interfaces. Métodos: AddAsync(Empresa empresa), GetByIdAsync(Guid id, string usuarioId), GetAllByUsuarioIdAsync(string usuarioId), e SaveChangesAsync().

Crie IEmpresaService em FiscalizAI.Application/Interfaces. Métodos: CriarAsync(CreateEmpresaDto dto, string usuarioId), ObterPorIdAsync(Guid id, string usuarioId), ListarPorUsuarioIdAsync(string usuarioId).

Passo 3: Implementação do Serviço (EmpresaService.cs)
Crie a classe em FiscalizAI.Application/Services, implementando IEmpresaService. Use injeção de dependência via construtor primário (C# 12+) para injetar o IEmpresaRepository e o ICryptographyService. NÃO injete o DbContext aqui.

Criptografia: Utilize o ICryptographyService para criptografar a SenhaCertificado vinda do DTO antes de atribuí-la à entidade.

Extração de Validade do PFX: Utilize a classe nativa X509Certificate2 do System.Security.Cryptography.X509Certificates para ler o dto.CertificadoDigital usando a senha em texto puro. Extraia a propriedade .NotAfter e atribua ao campo DataVencimentoCertificado da entidade.

Vínculo Multi-Tenant: Ao instanciar a entidade Empresa, certifique-se de adicionar a relação com o usuarioId na lista de vínculos (UsuarioEmpresas).

Passe a entidade pronta para o IEmpresaRepository.AddAsync e chame SaveChangesAsync.

Passo 4: Implementação do Repositório (EmpresaRepository.cs)
Crie a classe em FiscalizAI.Infra/Repositories, implementando IEmpresaRepository. Use construtor primário para injetar o seu DbContext.

Segurança no GetByIdAsync e GetAllByUsuarioIdAsync: Utilize o Entity Framework Core (LINQ) para buscar a empresa. É obrigatório incluir um .Where() que verifique se o usuarioId passado existe na tabela de vínculo (UsuarioEmpresa) daquela Empresa. Se o usuário tentar buscar o ID de uma empresa de outro contador, a consulta deve retornar null.

Operações de Escrita: Implemente o AddAsync usando o _context.Empresas.AddAsync(empresa) e o SaveChangesAsync chamando _context.SaveChangesAsync().

Gere um código limpo, assíncrono, mapeando a entidade para EmpresaDto nos retornos do serviço para não vazar dados sensíveis.