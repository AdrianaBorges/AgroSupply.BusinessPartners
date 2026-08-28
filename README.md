# AgroSupply.BusinessPartners

API REST desenvolvida em **.NET 8** para gerenciamento de parceiros de negócios em um contexto B2B, aplicando princípios de **DDD**, **Clean Architecture**, persistência com **Entity Framework Core**, **autenticação JWT**, **logging estruturado** e **testes automatizados**.

O projeto foi desenvolvido de forma incremental, priorizando separação de responsabilidades, testabilidade, segurança, documentação e simplicidade arquitetural.

---

## Funcionalidades

A API disponibiliza atualmente:

- criação de Business Partners;
- consulta por identificador;
- listagem de Business Partners;
- atualização cadastral;
- inativação lógica;
- associação de múltiplos números de telefone;
- consulta individual de telefone;
- atualização de telefone;
- remoção de telefone;
- criação de relacionamentos comerciais B2B;
- consulta de relacionamentos comerciais;
- inativação de relacionamentos comerciais;
- autenticação baseada em JWT;
- proteção de endpoints através de Bearer Token;
- logging estruturado dos principais fluxos da API.

A inativação do `BusinessPartner` preserva o registro para fins de rastreabilidade através de:

```text
IsActive = false
DeactivatedAt = data/hora da inativação
```

Já a remoção de `PhoneNumber` representa uma exclusão física da entidade dependente pertencente ao agregado.

Os relacionamentos comerciais entre parceiros são representados por `BusinessRelationship`, permitindo estabelecer uma relação entre fornecedor (`Supplier`) e comprador (`Buyer`).

---

## Arquitetura

A solução está organizada em quatro projetos principais:

```text
src
├── AgroSupply.BusinessPartners.Api
├── AgroSupply.BusinessPartners.Application
├── AgroSupply.BusinessPartners.Domain
└── AgroSupply.BusinessPartners.Infrastructure
```

e três projetos de testes:

```text
tests
├── AgroSupply.BusinessPartners.Api.Tests
├── AgroSupply.BusinessPartners.Application.Tests
└── AgroSupply.BusinessPartners.Domain.Tests
```

As responsabilidades estão distribuídas da seguinte forma:

| Projeto | Responsabilidade |
| --- | --- |
| Domain | Entidades, estados e comportamentos de negócio |
| Application | Casos de uso e abstrações |
| Infrastructure | Persistência e implementações técnicas |
| API | Contratos, autenticação e endpoints REST |

O fluxo principal segue:

```text
HTTP Request
     ↓
    API
     ↓
Application
     ↓
  Domain
     ↓
Repository
     ↓
Infrastructure
     ↓
SQL Server
```

A arquitetura utiliza princípios de Clean Architecture e DDD de forma pragmática, evitando abstrações sem necessidade concreta para o escopo da solução.

---

## Modelo de Domínio

`BusinessPartner` representa a entidade principal do domínio e funciona como principal ponto de controle do agregado.

Um Business Partner pode possuir vários números de telefone:

```text
BusinessPartner 1 ───────── N PhoneNumber
```

As operações relacionadas aos telefones são realizadas através do próprio `BusinessPartner`, preservando o encapsulamento da coleção.

Entre os comportamentos disponíveis estão:

```text
AddPhoneNumber(...)
GetPhoneNumber(...)
UpdatePhoneNumber(...)
RemovePhoneNumber(...)
```

Os telefones são classificados através de:

`PhoneNumberType`

com os tipos:

- `Mobile`;
- `Residential`;
- `Commercial`.

### Relacionamentos B2B

A entidade `BusinessRelationship` representa uma relação comercial entre dois parceiros:

```text
Supplier ───── BusinessRelationship ───── Buyer
```

Entre as regras implementadas estão:

- fornecedor e comprador são obrigatórios;
- um parceiro não pode estabelecer relacionamento comercial consigo mesmo;
- fornecedor e comprador devem existir;
- ambos os parceiros devem estar ativos;
- o relacionamento é criado com status ativo;
- relacionamentos podem ser inativados;
- a inativação é idempotente;
- não é permitido criar mais de um relacionamento ativo entre o mesmo fornecedor e comprador;
- o relacionamento inverso representa uma relação comercial distinta;
- um relacionamento anteriormente inativado não impede a criação de uma nova relação ativa.

---

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server / LocalDB
- JWT Bearer Authentication
- Swagger / OpenAPI
- `ILogger<T>`
- xUnit
- Microsoft.AspNetCore.Mvc.Testing
- Entity Framework Core InMemory
- Docker
- Git

---

## Endpoints

Atualmente a API disponibiliza **13 operações REST**.

### Authentication

| Método | Endpoint | Finalidade |
| --- | --- | --- |
| POST | `/api/authentication/login` | Autenticar e obter JWT |

### Business Partners

| Método | Endpoint | Finalidade |
| --- | --- | --- |
| POST | `/api/BusinessPartners` | Criar Business Partner |
| GET | `/api/BusinessPartners` | Listar Business Partners |
| GET | `/api/BusinessPartners/{id}` | Consultar por identificador |
| PUT | `/api/BusinessPartners/{id}` | Atualizar dados cadastrais |
| DELETE | `/api/BusinessPartners/{id}` | Inativar Business Partner |

### Phone Numbers

| Método | Endpoint | Finalidade |
| --- | --- | --- |
| POST | `/api/BusinessPartners/{id}/phone-numbers` | Associar telefone |
| GET | `/api/BusinessPartners/{id}/phone-numbers/{phoneNumberId}` | Consultar telefone |
| PUT | `/api/BusinessPartners/{id}/phone-numbers/{phoneNumberId}` | Atualizar telefone |
| DELETE | `/api/BusinessPartners/{id}/phone-numbers/{phoneNumberId}` | Remover telefone |

### Business Relationships

| Método | Endpoint | Finalidade |
| --- | --- | --- |
| POST | `/api/business-relationships` | Criar relacionamento comercial |
| GET | `/api/business-relationships/{id}` | Consultar relacionamento |
| DELETE | `/api/business-relationships/{id}` | Inativar relacionamento |

As rotas de telefone permanecem subordinadas ao `BusinessPartner`, refletindo o relacionamento **1:N** definido no domínio.

Os endpoints de Business Partners e Business Relationships são protegidos por autenticação JWT.

A documentação completa dos contratos HTTP é disponibilizada através do Swagger/OpenAPI.

---

## Autenticação JWT

A API utiliza autenticação baseada em **JWT (JSON Web Token)**.

O endpoint:

```text
POST /api/authentication/login
```

recebe as credenciais de autenticação e, quando válidas, retorna um token de acesso do tipo Bearer.

O token deve ser enviado nas requisições protegidas através do header:

```text
Authorization: Bearer {token}
```

Os controllers responsáveis por Business Partners e Business Relationships utilizam `[Authorize]`, impedindo o acesso anônimo aos recursos protegidos.

As credenciais destinadas à avaliação técnica da solução estão documentadas na Wiki do projeto.

A configuração foi estruturada de forma que credenciais de demonstração utilizadas no ambiente de desenvolvimento não representem a estratégia recomendada para ambientes produtivos.

---

## Logging Estruturado

A API utiliza o mecanismo nativo de logging do ASP.NET Core através de:

```csharp
ILogger<T>
```

São registrados eventos relevantes dos principais fluxos, incluindo:

- início de operações;
- conclusão de operações;
- recursos não encontrados;
- tentativas de operações inválidas;
- conflitos de regras de negócio;
- autenticação válida e inválida.

Os logs utilizam parâmetros estruturados para facilitar rastreabilidade e diagnóstico.

Dados pessoais sensíveis do Business Partner, como CPF, nome e telefone, não são registrados nos eventos de negócio implementados.

---

## Swagger / OpenAPI

Durante a execução da aplicação em ambiente de desenvolvimento, a documentação interativa está disponível em:

```text
/swagger
```

A especificação OpenAPI pode ser consultada através de:

```text
/swagger/v1/swagger.json
```

A documentação é gerada a partir da própria aplicação e enriquecida através de XML Documentation Comments e informações dos códigos HTTP esperados.

O Swagger também está configurado para utilização de autenticação Bearer, permitindo informar o JWT através da opção **Authorize** e executar os endpoints protegidos diretamente pela interface.

Atualmente o Swagger documenta todas as **13 operações REST** disponibilizadas pela API.

---

## Banco de Dados

A persistência utiliza **SQL Server** através do **Entity Framework Core 8**.

O modelo contempla:

```text
BusinessPartner
      │
      │ 1:N
      ▼
PhoneNumber

Supplier
      │
      ▼
BusinessRelationship
      │
      ▼
Buyer
```

A evolução do schema é controlada através de migrations:

```text
InitialCreate
      ↓
AddBusinessPartnerDeactivatedAt
      ↓
AddBusinessPartnerPhoneNumbers
      ↓
AddBusinessRelationships
```

O relacionamento entre `BusinessPartner` e `PhoneNumber` utiliza:

```csharp
DeleteBehavior.ClientCascade
```

Essa configuração permite que o Entity Framework Core trate corretamente a remoção explícita de um telefone pertencente ao agregado enquanto as entidades estão sendo rastreadas pelo contexto.

A decisão mantém a estratégia de persistência na camada Infrastructure e evita transferir detalhes do Entity Framework Core para o domínio ou para a Application.

---

## Estratégias de Remoção

A solução possui comportamentos distintos conforme a responsabilidade de cada entidade:

| Entidade | Estratégia |
| --- | --- |
| `BusinessPartner` | Inativação lógica |
| `PhoneNumber` | Remoção física explícita |
| `BusinessRelationship` | Inativação lógica |

O `BusinessPartner` permanece disponível após sua inativação, com:

```text
IsActive = false
DeactivatedAt = data/hora da inativação
```

Já um `PhoneNumber` removido deixa de existir na persistência.

O `BusinessRelationship` preserva o histórico da relação comercial após sua inativação.

---

## Configuração do Banco

A connection string utilizada pela aplicação é definida através da configuração:

```text
DefaultConnection
```

Antes de executar a aplicação, ajuste a connection string conforme o ambiente utilizado.

---

## Aplicando as Migrations

As migrations podem ser aplicadas através do Package Manager Console:

```powershell
Update-Database
```

ou através da CLI:

```bash
dotnet ef database update
```

---

## Executando o Projeto

### Pré-requisitos

Para executar a solução é necessário possuir:

- .NET 8 SDK;
- SQL Server ou SQL Server LocalDB;
- ferramenta compatível com Entity Framework Core migrations.

### Clonar o repositório

```bash
git clone https://github.com/AdrianaBorges/AgroSupply.BusinessPartners.git
```

Acesse o diretório:

```bash
cd AgroSupply.BusinessPartners
```

### Restaurar dependências

```bash
dotnet restore
```

### Compilar

```bash
dotnet build
```

### Atualizar o banco

```bash
dotnet ef database update
```

### Executar a API

```bash
dotnet run --project src/AgroSupply.BusinessPartners.Api
```

Após iniciar a aplicação, acesse o Swagger através da URL exibida no terminal seguida de:

```text
/swagger
```

Para acessar os endpoints protegidos, realize a autenticação através de:

```text
POST /api/authentication/login
```

e informe o token obtido na opção **Authorize** do Swagger.

---

## Testes Automatizados

A estratégia de testes acompanha as responsabilidades arquiteturais da solução e evoluiu juntamente com as funcionalidades implementadas.

A suíte pode ser executada através de:

```bash
dotnet test
```

Resultado atual:

```text
81 testes executados
81 testes aprovados
0 falhas
```

Os testes cobrem, entre outros cenários:

- regras de domínio;
- casos de uso;
- fluxos HTTP;
- inativação lógica;
- relacionamento 1:N;
- inclusão de telefone;
- consulta de telefone;
- atualização de telefone;
- remoção de telefone;
- cenários `404 Not Found`;
- persistência efetiva da remoção;
- criação de relacionamentos B2B;
- validação de fornecedor e comprador;
- bloqueio de autorrelacionamento;
- bloqueio de relacionamento ativo duplicado;
- inativação de relacionamentos comerciais;
- autenticação com credenciais válidas;
- rejeição de credenciais inválidas;
- retorno `401 Unauthorized` para acesso não autenticado a endpoint protegido.

Os testes de integração da API utilizam `WebApplicationFactory` e banco em memória para manter isolamento em relação ao banco de desenvolvimento.

A evolução da suíte acompanhou o crescimento funcional da solução:

```text
26 testes
    ↓
36 testes
    ↓
53 testes
    ↓
78 testes
    ↓
81 testes
```

---

## Documentação

Além do README, o projeto possui uma **Wiki técnica** com documentação detalhada da solução.

A Wiki contém:

- visão geral do projeto;
- evidências de testes funcionais;
- evidências funcionais dos relacionamentos B2B;
- autenticação JWT;
- testes automatizados;
- documentação Swagger/OpenAPI;
- arquitetura da solução;
- modelo de domínio;
- persistência e banco de dados;
- contratos e endpoints da API;
- decisões técnicas;
- documentação específica sobre persistência e remoção de telefones;
- evidências de logging estruturado.

A documentação foi construída juntamente com a evolução da aplicação para manter alinhamento entre implementação, testes e decisões arquiteturais.

---

## Principais Decisões Técnicas

Entre as principais decisões adotadas estão:

- .NET 8 como plataforma;
- Clean Architecture de forma pragmática;
- conceitos de DDD sem complexidade desnecessária;
- `BusinessPartner` como entidade principal do agregado;
- relacionamento 1:N com `PhoneNumber`;
- CRUD de `PhoneNumber` mantido dentro do agregado;
- `BusinessRelationship` para representação das relações comerciais B2B;
- distinção explícita entre fornecedor (`Supplier`) e comprador (`Buyer`);
- inativação lógica de `BusinessPartner`;
- remoção física explícita de `PhoneNumber`;
- inativação lógica de `BusinessRelationship`;
- `DeleteBehavior.ClientCascade` para tratamento da entidade dependente no EF Core;
- Repository Pattern para abstração da persistência;
- ausência de um `PhoneNumberRepository` sem necessidade concreta;
- Fluent API para manter o domínio independente do EF Core;
- migrations para versionamento do banco;
- contratos HTTP separados das entidades de domínio;
- autenticação JWT através de Bearer Token;
- serviço de autenticação abstraído através de `IAuthenticationService`;
- geração de tokens centralizada em `JwtTokenService`;
- proteção de recursos através de `[Authorize]`;
- logging estruturado utilizando `ILogger<T>`;
- Swagger/OpenAPI integrado ao código e configurado para autenticação Bearer;
- testes automatizados desenvolvidos incrementalmente;
- testes de integração isolados através de `WebApplicationFactory` e EF Core InMemory.

Também foi deliberadamente evitada a inclusão de componentes como CQRS completo, MediatR, mensageria e outras abstrações que não apresentavam necessidade concreta para o escopo atual.

---

## Padrão de Idioma

A solução adota:

**Inglês**

- código-fonte;
- classes;
- métodos;
- propriedades;
- contratos;
- endpoints;
- documentação XML;
- Swagger/OpenAPI.

**Português (pt-BR)**

- README;
- Wiki;
- documentação explicativa;
- mensagens destinadas ao usuário.

Essa separação mantém a nomenclatura técnica consistente sem prejudicar a clareza da documentação destinada à apresentação do projeto.

---

## Estrutura do Repositório

```text
AgroSupply.BusinessPartners
│
├── src
│   ├── AgroSupply.BusinessPartners.Api
│   ├── AgroSupply.BusinessPartners.Application
│   ├── AgroSupply.BusinessPartners.Domain
│   └── AgroSupply.BusinessPartners.Infrastructure
│
├── tests
│   ├── AgroSupply.BusinessPartners.Api.Tests
│   ├── AgroSupply.BusinessPartners.Application.Tests
│   └── AgroSupply.BusinessPartners.Domain.Tests
│
└── AgroSupply.BusinessPartners.sln
```

---

## Qualidade

A solução foi construída de forma incremental, mantendo como parte do processo:

```text
Implementação
     ↓
Compilação
     ↓
Testes automatizados
     ↓
Validação funcional
     ↓
Documentação
     ↓
Commit
```

Os testes também foram utilizados como mecanismo de feedback para decisões técnicas.

Um exemplo foi a remoção de `PhoneNumber`, em que o teste de integração identificou o comportamento inadequado da configuração inicial com `DeleteBehavior.Restrict`.

Após a análise, a estratégia foi ajustada para:

```csharp
DeleteBehavior.ClientCascade
```

A evolução das funcionalidades B2B e de autenticação também foi acompanhada pela ampliação da cobertura automatizada.

Ao final da implementação atual, toda a suíte foi executada com sucesso:

```text
81 / 81 testes aprovados
0 falhas
```

O objetivo foi entregar não apenas uma API funcional, mas uma solução compreensível, segura, testável, documentada e preparada para evolução.
