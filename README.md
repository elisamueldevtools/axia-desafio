# Axia Veículos API

API REST para cadastro e consulta de veículos com autenticação JWT e controle de acesso por roles.

## Tecnologias

- **.NET 8.0** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core InMemory** - Persistência
- **MediatR** - CQRS (Commands/Queries)
- **FluentValidation** - Validação de requests
- **BCrypt.Net** - Hash de senhas
- **JWT RS256** - Autenticação com chaves RSA
- **xUnit + Moq + FluentAssertions** - Testes unitários

## Arquitetura

Projeto estruturado em **Clean Architecture** com **CQRS**.

### Por que Clean Architecture + CQRS?

- **Independência de frameworks**: o domínio não conhece detalhes de infraestrutura
- **Testabilidade**: cada camada pode ser testada isoladamente com mocks
- **Separação de responsabilidades**: Commands alteram estado, Queries apenas consultam
- **Manutenibilidade**: mudanças em uma camada não afetam as demais
- **Escalabilidade**: fácil adicionar novos casos de uso sem impactar existentes

### Estrutura de Pastas

```
src/
├── Axia.Veiculos.Domain/
│   ├── Entities/
│   │   ├── Usuario.cs
│   │   └── Veiculo.cs
│   ├── Enums/
│   │   ├── Marca.cs
│   │   └── Role.cs
│   └── Interfaces/
│       ├── IBaseRepository.cs
│       ├── IUsuarioRepository.cs
│       └── IVeiculoRepository.cs
│
├── Axia.Veiculos.Application/
│   ├── Common/
│   │   ├── Behaviors/
│   │   │   └── ValidationBehavior.cs
│   │   ├── Interfaces/
│   │   │   ├── IJwtTokenGenerator.cs
│   │   │   └── IPasswordHasher.cs
│   │   └── Result.cs
│   └── UseCase/
│       ├── Auth/
│       │   ├── LoginCommand.cs
│       │   ├── LoginCommandHandler.cs
│       │   └── LoginCommandValidator.cs
│       ├── Usuarios/
│       │   ├── Commands/
│       │   └── Queries/
│       └── Veiculos/
│           ├── Commands/
│           └── Queries/
│
├── Axia.Veiculos.Infra/
│   ├── Context/
│   │   └── AppDbContext.cs
│   ├── Repositories/
│   │   ├── BaseRepository.cs
│   │   ├── UsuarioRepository.cs
│   │   └── VeiculoRepository.cs
│   └── Security/
│       ├── BCryptPasswordHasher.cs
│       ├── JwtTokenGenerator.cs
│       └── RsaKeyService.cs
│
├── Axia.Veiculos.WebApi/
│   ├── Attributes/
│   │   └── AuthorizationAxiaAttribute.cs
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── UsuariosController.cs
│   │   └── VeiculosController.cs
│   ├── Configurations/
│   ├── Middlewares/
│   └── Program.cs
│
└── Tests/
    ├── Axia.Veiculos.Domain.Tests/
    ├── Axia.Veiculos.Application.Tests/
    ├── Axia.Veiculos.Infra.Tests/
    └── Axia.Veiculos.WebApi.Tests/
```

### Camadas

**Domain** - Núcleo da aplicação, não depende de nada externo.
- Entidades com regras de negócio (Usuario, Veiculo)
- Enums do domínio (Marca, Role)
- Interfaces dos repositórios (contratos)

**Application** - Casos de uso da aplicação, depende apenas do Domain.
- Commands: operações que alteram estado (Create, Update, Delete)
- Queries: operações de leitura (GetById, List)
- Handlers: executam a lógica dos Commands/Queries
- Validators: validação de entrada via FluentValidation
- Result Pattern: padroniza respostas de sucesso/erro

**Infra** - Implementações concretas, depende do Domain e Application.
- Repositórios: acesso a dados via EF Core
- Security: JWT (RSA), BCrypt para senhas
- DbContext: configuração do Entity Framework

**WebApi** - Ponto de entrada HTTP, depende de todas as camadas.
- Controllers: recebem requests e delegam ao MediatR
- Middlewares: tratamento global de exceções
- Configurations: JWT, Swagger, DI

**Tests** - Testes unitários isolados por camada.
- Domain: testes de entidades
- Application: testes de handlers com mocks
- Infra: testes de repositórios com InMemory
- WebApi: testes de controllers

### Repository Pattern

Abstração do acesso a dados através de interfaces no Domain e implementações no Infra.

- `IBaseRepository<T>`: contrato genérico (GetById, GetAll, Add, Update, Delete)
- `IUsuarioRepository`: adiciona GetByLogin e ExistsByLogin
- `IVeiculoRepository`: herda apenas do base

Os handlers dependem apenas das interfaces, permitindo trocar a implementação (InMemory → SQL Server) sem alterar a Application.

### Result Pattern

Padroniza todas as respostas da API, evitando exceções para controle de fluxo.

```json
{
  "isSuccess": true,
  "message": "Veículo cadastrado",
  "statusCode": 201,
  "data": "guid-do-veiculo",
  "errors": null
}
```

Factory methods disponíveis:
- `Result.Success()` → 200
- `Result.Created()` → 201
- `Result.BadRequest()` → 400
- `Result.Unauthorized()` → 401
- `Result.Forbidden()` → 403
- `Result.NotFound()` → 404

### JWT com RSA (RS256)

Tokens assinados com chave privada RSA e validados com chave pública.

- **Algoritmo**: RS256 (RSA + SHA-256)
- **Chave privada**: usada apenas para gerar tokens (servidor)
- **Chave pública**: usada para validar tokens (pode ser distribuída)
- **Expiração**: 60 minutos

Configuração no `appsettings.json`:
```json
{
  "Jwt": {
    "Issuer": "AxiaVeiculos",
    "Audience": "AxiaVeiculosApi",
    "PrivateCert": "MIICWgIBAAKBgF9e...",
    "PublicCert": "MIGeMA0GCSqGSIb3..."
  }
}
```

### Roles no Token JWT

A role do usuário é incluída como claim no token, permitindo autorização sem consultar o banco.

Claims geradas:
```json
{
  "sub": "guid-do-usuario",
  "name": "Admin",
  "role": "Admin",
  "exp": 1234567890
}
```

O `AuthorizationAxiaAttribute` valida a role diretamente do token:
- Token ausente/inválido → 401 Unauthorized
- Role sem permissão → 403 Forbidden

## Como Executar

```bash
cd src/Axia.Veiculos.WebApi
dotnet restore
dotnet run
```
## Autenticação

### Usuário Padrão (Seed)

| Login | Senha | Role |
|-------|-------|------|
| admin | admin1234 | Admin |

### Realizar Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "login": "admin",
  "senha": "admin1234"
}
```

### Usar Token no Swagger

1. Execute o login e copie o `accessToken`
2. Clique em **Authorize**
3. Digite: `Bearer {seu_token}`

## Sistema de Roles

| Role | Permissões |
|------|------------|
| Reader | Consultar veículos |
| User | Reader + Cadastrar/Editar/Remover veículos |
| Admin | User + Gerenciar usuários |

## Validações

### Usuário
- Nome: obrigatório, mín 3 caracteres
- Login: obrigatório, mín 3 caracteres, único
- Senha: obrigatória, mín 6 caracteres
- Role: Reader, User ou Admin

### Veículo
- Descrição: obrigatória, máx 100 caracteres
- Marca: enum válido (documentado no Swagger)
- Modelo: obrigatório, máx 30 caracteres
- Valor: maior que zero (quando informado)

## Testes

```bash
cd src
dotnet test
```

| Camada | Testes |
|--------|--------|
| Domain | 6 |
| Application | 26 |
| Infra | 17 |
| WebApi | 12 |
| **Total** | **61** |

## Observações

- **Banco InMemory**: dados são perdidos ao reiniciar
- **Token JWT**: expira em 60 minutos
- **Senhas**: hash BCrypt com salt automático
