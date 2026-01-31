# Axia Veículos API

API REST para cadastro e consulta de veículos com autenticação JWT e controle de acesso por roles.

## Tecnologias

| Tecnologia | Versão | Descrição |
|------------|--------|-----------|
| .NET | 8.0 | Framework principal |
| ASP.NET Core | 8.0 | Web API com Controllers |
| Entity Framework Core | 8.0 | ORM com InMemory Provider |
| MediatR | 12.x | CQRS (Commands/Queries) |
| FluentValidation | 11.x | Validação de requests |
| BCrypt.Net | 4.x | Hash de senhas |
| Serilog | 8.x | Logging estruturado |
| Swashbuckle | 6.x | OpenAPI/Swagger |

## Arquitetura

O projeto segue **Clean Architecture** com separação clara de responsabilidades:

```
src/
├── Axia.Veiculos.Domain/           # Camada de Domínio (núcleo)
│   ├── Entities/                   # Entidades de domínio
│   │   ├── Usuario.cs
│   │   └── Veiculo.cs
│   ├── Enums/                      # Enumeradores
│   │   ├── Marca.cs
│   │   └── Role.cs
│   └── Interfaces/                 # Contratos de repositórios
│       ├── IUsuarioRepository.cs
│       └── IVeiculoRepository.cs
│
├── Axia.Veiculos.Application/      # Camada de Aplicação (casos de uso)
│   ├── Common/
│   │   ├── Behaviors/              # Pipeline behaviors (validação)
│   │   ├── Interfaces/             # Contratos de serviços
│   │   └── Result.cs               # Result Pattern
│   ├── UseCase/
│   │   ├── Auth/
│   │   │   ├── Requests/           # DTOs de entrada
│   │   │   ├── Responses/          # DTOs de saída
│   │   │   ├── LoginCommand.cs
│   │   │   ├── LoginCommandHandler.cs
│   │   │   └── LoginCommandValidator.cs
│   │   ├── Usuarios/
│   │   │   ├── Requests/
│   │   │   ├── Responses/
│   │   │   ├── Commands/
│   │   │   └── Queries/
│   │   └── Veiculos/
│   │       ├── Requests/
│   │       ├── Responses/
│   │       ├── Commands/
│   │       └── Queries/
│   └── DependencyInjection.cs
│
├── Axia.Veiculos.Infra/            # Camada de Infraestrutura
│   ├── Context/
│   │   └── AppDbContext.cs         # EF Core DbContext
│   ├── Repositories/               # Implementação dos repositórios
│   │   ├── UsuarioRepository.cs
│   │   └── VeiculoRepository.cs
│   ├── Security/
│   │   ├── BCryptPasswordHasher.cs
│   │   ├── JwtTokenGenerator.cs
│   │   └── RsaKeyService.cs        # Chaves RSA para JWT
│   └── DependencyInjection.cs
│
└── Axia.Veiculos.WebApi/           # Camada de Apresentação
    ├── Attributes/
    │   └── AuthorizationAxiaAttribute.cs
    ├── Controllers/
    │   ├── AuthController.cs
    │   ├── UsuariosController.cs
    │   └── VeiculosController.cs
    ├── Extensions/
    │   └── CustomExtensions.cs
    ├── Middlewares/
    │   └── ExceptionHandlingMiddleware.cs
    └── Program.cs
```

### Fluxo de Dados

```
Request → Controller → Request DTO → Command/Query → Handler → Response DTO → Response
```

## Como Executar

```bash
cd src/Axia.Veiculos.WebApi
dotnet restore
dotnet run
```

**URLs:**
- API: `http://localhost:5036`
- Swagger: `http://localhost:5036/swagger`

## Autenticação

A API utiliza **JWT (JSON Web Token)** com algoritmo **RS256** (RSA + SHA-256) para autenticação.

### Configuração JWT

As chaves RSA são configuradas no `appsettings.json`:

```json
{
  "Jwt": {
    "Issuer": "AxiaVeiculos",
    "Audience": "AxiaVeiculosApi",
    "ExpirationMinutes": 60,
    "PrivateCert": "MIICWgIBAAKBgF9efJAU...",
    "PublicCert": "MIGeMA0GCSqGSIb3DQEB..."
  }
}
```

### Realizar Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "login": "admin",
  "senha": "admin1234"
}
```

**Resposta:**
```json
{
  "isSuccess": true,
  "message": "Login realizado com sucesso",
  "statusCode": 200,
  "data": {
    "accessToken": "eyJhbGciOiJSUzI1NiIs...",
    "tokenType": "Bearer"
  }
}
```

### Usar Token no Swagger

1. Execute o login e copie o `accessToken`
2. Clique em **Authorize** no Swagger
3. Digite: `Bearer {seu_token}`
4. Clique em **Authorize**

### Usuário Padrão (Seed)

A aplicação cria automaticamente um usuário administrador:

| Campo | Valor |
|-------|-------|
| Login | admin |
| Senha | admin1234 |
| Role | Admin |

## Sistema de Roles

A API implementa controle de acesso baseado em roles (RBAC).

### Roles Disponíveis

| Role | Valor | Permissões |
|------|-------|------------|
| Reader | 1 | Consultar veículos |
| User | 2 | Reader + Cadastrar/Editar/Remover veículos |
| Admin | 3 | User + Gerenciar usuários |

### Claims no Token JWT

O token inclui a role do usuário em duas claims para compatibilidade:

```json
{
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": "Admin",
  "role": "Admin"
}
```

## AuthorizationAxiaAttribute

Atributo customizado para validação de roles nos endpoints.

### Implementação

```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationAxiaAttribute : Attribute, IAuthorizationFilter
{
    private readonly Role[] _roles;

    public AuthorizationAxiaAttribute(params Role[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Verifica se usuário está autenticado
        // Verifica se possui a role necessária
        // Retorna 401 ou 403 conforme o caso
    }
}
```

### Uso nos Controllers

```csharp
// Apenas Admin pode acessar
[HttpGet]
[AuthorizationAxia(Role.Admin)]
public async Task<IActionResult> GetAll() { ... }

// Admin ou User podem acessar
[HttpPost]
[AuthorizationAxia(Role.Admin, Role.User)]
public async Task<IActionResult> Create() { ... }

// Qualquer role autenticada pode acessar
[HttpGet]
[AuthorizationAxia(Role.Admin, Role.User, Role.Reader)]
public async Task<IActionResult> List() { ... }
```

### Respostas de Erro

**401 Unauthorized** - Token ausente ou inválido:
```json
{
  "isSuccess": false,
  "message": "Credenciais inválidas",
  "statusCode": 401
}
```

**403 Forbidden** - Sem permissão para o recurso:
```json
{
  "isSuccess": false,
  "message": "Sem permissão para este recurso",
  "statusCode": 403
}
```

## Endpoints

### Auth

| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| POST | /api/auth/login | Realizar login | Não |

### Usuários

| Método | Rota | Descrição | Role |
|--------|------|-----------|------|
| GET | /api/usuarios | Listar todos | Admin |
| GET | /api/usuarios/{id} | Obter por ID | Admin |
| POST | /api/usuarios | Criar usuário | Admin |
| PUT | /api/usuarios/{id} | Atualizar | Admin |
| DELETE | /api/usuarios/{id} | Remover | Admin |

### Veículos

| Método | Rota | Descrição | Role |
|--------|------|-----------|------|
| GET | /api/veiculos | Listar todos | Reader+ |
| GET | /api/veiculos/{id} | Obter por ID | Reader+ |
| POST | /api/veiculos | Cadastrar | User+ |
| PUT | /api/veiculos/{id} | Atualizar | User+ |
| DELETE | /api/veiculos/{id} | Remover | User+ |

## Exemplos de Requests

### Criar Usuário

```json
POST /api/usuarios
{
  "nome": "João Silva",
  "login": "joao.silva",
  "senha": "minhasenha123",
  "role": "User"
}
```

### Criar Veículo

```json
POST /api/veiculos
{
  "descricao": "Sedan Completo 2024",
  "marca": "Toyota",
  "modelo": "Corolla",
  "opcionais": "Ar condicionado, Direção elétrica",
  "valor": 125000.00
}
```

### Marcas Disponíveis

- Volkswagen
- Chevrolet
- Fiat
- Ford
- Honda
- Toyota
- Hyundai
- Renault
- Nissan
- Jeep

## Validações

As validações são realizadas via **FluentValidation** no pipeline do MediatR.

### Usuário

| Campo | Regra |
|-------|-------|
| Nome | Obrigatório, mínimo 3 caracteres |
| Login | Obrigatório, mínimo 3 caracteres, único |
| Senha | Obrigatória, mínimo 6 caracteres |
| Role | Reader, User ou Admin |

### Veículo

| Campo | Regra |
|-------|-------|
| Descrição | Obrigatória, máximo 100 caracteres |
| Marca | Obrigatória, enum válido |
| Modelo | Obrigatório, máximo 30 caracteres |
| Valor | Maior que zero (quando informado) |

## Result Pattern

A API utiliza o **Result Pattern** para padronizar todas as respostas, eliminando o uso de exceções para controle de fluxo e garantindo consistência na comunicação com o cliente.

### Por que usar Result Pattern?

- **Consistência**: Todas as respostas seguem o mesmo formato
- **Previsibilidade**: O cliente sempre sabe o que esperar
- **Performance**: Evita overhead de exceções para erros de negócio
- **Semântica clara**: Métodos factory indicam explicitamente o resultado

### Estrutura das Classes

```csharp
// Resultado sem dados (operações void)
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Message { get; }
    public int StatusCode { get; }
    public IEnumerable<string>? Errors { get; }
}

// Resultado com dados tipados
public class Result<T> : Result
{
    public T? Data { get; }
}
```

### Factory Methods Disponíveis

| Método | Status | Uso |
|--------|--------|-----|
| `Success()` | 200 | Operação bem sucedida |
| `Created()` | 201 | Recurso criado |
| `BadRequest()` | 400 | Dados inválidos |
| `Unauthorized()` | 401 | Credenciais inválidas |
| `Forbidden()` | 403 | Sem permissão |
| `NotFound()` | 404 | Recurso não encontrado |
| `InternalError()` | 500 | Erro interno |

### Uso nos Handlers

```csharp
public class CreateVeiculoCommandHandler : IRequestHandler<CreateVeiculoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateVeiculoCommand request, CancellationToken ct)
    {
        // Validação de negócio
        if (await repository.ExistsAsync(request.Placa, ct))
            return Result<Guid>.BadRequest("Veículo já cadastrado");

        var veiculo = new Veiculo(request.Descricao, request.Marca, ...);
        await repository.AddAsync(veiculo, ct);

        return Result<Guid>.Created(veiculo.Id, "Veículo cadastrado");
    }
}
```

### Tratamento no BaseController

```csharp
protected IActionResult HandleResult<T>(Result<T> result)
{
    return StatusCode(result.StatusCode, new
    {
        result.IsSuccess,
        result.Message,
        result.StatusCode,
        result.Data,
        result.Errors
    });
}
```

### Exemplos de Resposta

**Sucesso (200):**
```json
{
  "isSuccess": true,
  "message": "Sucesso",
  "statusCode": 200,
  "data": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "descricao": "Sedan Completo",
    "marca": "Toyota"
  },
  "errors": null
}
```

**Criação (201):**
```json
{
  "isSuccess": true,
  "message": "Veículo cadastrado",
  "statusCode": 201,
  "data": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "errors": null
}
```

**Erro de Validação (400):**
```json
{
  "isSuccess": false,
  "message": "Dados inválidos",
  "statusCode": 400,
  "data": null,
  "errors": [
    "Descrição é obrigatória",
    "Valor deve ser maior que zero"
  ]
}
```

**Não Encontrado (404):**
```json
{
  "isSuccess": false,
  "message": "Veículo não encontrado",
  "statusCode": 404,
  "data": null,
  "errors": null
}
```

### Códigos de Status

| Código | Descrição |
|--------|-----------|
| 200 | Sucesso |
| 201 | Recurso criado |
| 400 | Erro de validação |
| 401 | Não autenticado |
| 403 | Sem permissão |
| 404 | Não encontrado |
| 500 | Erro interno |

## Observações

- **Banco InMemory**: Os dados são perdidos ao reiniciar a aplicação
- **Token JWT**: Expira em 60 minutos
- **Senhas**: Armazenadas com hash BCrypt (salt automático)
- **Logs**: Estruturados via Serilog no console
