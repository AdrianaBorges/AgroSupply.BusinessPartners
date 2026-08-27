# AgroSupply.BusinessPartners

API REST desenvolvida em **.NET 8** para gerenciamento de parceiros de negócios em um contexto B2B, aplicando princípios de **DDD**, **Clean Architecture**, persistência com **Entity Framework Core** e **testes automatizados**.

O projeto foi desenvolvido de forma incremental, priorizando separação de responsabilidades, testabilidade, documentação e simplicidade arquitetural.

---

## Funcionalidades

A API disponibiliza atualmente:

- criação de Business Partners;
- consulta por identificador;
- listagem de Business Partners;
- atualização cadastral;
- inativação lógica;
- associação de múltiplos números de telefone;
- recuperação dos telefones associados ao Business Partner.

A inativação preserva o registro para fins de rastreabilidade através de:

```text
IsActive = false
DeactivatedAt = data/hora da inativação
```

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
| API | Contratos e endpoints REST |

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

`BusinessPartner` representa a entidade principal do domínio.

Um Business Partner pode possuir vários números de telefone:

```text
BusinessPartner 1 ───────── N PhoneNumber
```

A associação é realizada através do comportamento do próprio Business Partner, mantendo o controle da operação no domínio.

Os telefones são classificados através de:

`PhoneNumberType`

com os tipos:

- `Mobile`;
- `Landline`;
- `Other`.

---

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server / LocalDB
- Swagger / OpenAPI
- xUnit
- Docker
- Git

---

## Endpoints

| Método | Endpoint | Finalidade |
| --- | --- | --- |
| POST | `/api/BusinessPartners` | Criar Business Partner |
| GET | `/api/BusinessPartners` | Listar Business Partners |
| GET | `/api/BusinessPartners/{id}` | Consultar por identificador |
| PUT | `/api/BusinessPartners/{id}` | Atualizar dados cadastrais |
| DELETE | `/api/BusinessPartners/{id}` | Inativar Business Partner |
| POST | `/api/BusinessPartners/{id}/phone-numbers` | Associar telefone |

A documentação completa dos contratos HTTP é disponibilizada através do Swagger/OpenAPI.

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

---

## Banco de Dados

A persistência utiliza **SQL Server** através do **Entity Framework Core 8**.

O modelo atualmente possui as estruturas:

```text
BusinessPartners
      │
      │ 1:N
      ▼
PhoneNumbers
```

A evolução do schema é controlada através de migrations:

```text
InitialCreate
      ↓
AddBusinessPartnerDeactivatedAt
      ↓
AddBusinessPartnerPhoneNumbers
```

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

---

## Testes Automatizados

A estratégia de testes acompanha as responsabilidades arquiteturais da solução.

| Projeto | Testes | Resultado |
| --- | ---: | --- |
| Domain.Tests | 17 | Aprovados |
| Application.Tests | 10 | Aprovados |
| Api.Tests | 9 | Aprovados |
| **Total** | **36** | **36 aprovados** |

A suíte pode ser executada através de:

```bash
dotnet test
```

Resultado atual:

```text
36 testes executados
36 testes aprovados
0 falhas
```

Os testes cobrem regras de domínio, casos de uso e fluxos HTTP da API.

---

## Documentação

Além do README, o projeto possui uma **Wiki técnica** com documentação detalhada da solução.

A Wiki contém:

- visão geral do projeto;
- evidências de testes funcionais;
- testes automatizados;
- documentação Swagger/OpenAPI;
- arquitetura da solução;
- modelo de domínio;
- persistência e banco de dados;
- contratos e endpoints da API;
- decisões técnicas.

A documentação foi construída juntamente com a evolução da aplicação para manter alinhamento entre implementação, testes e decisões arquiteturais.

---

## Principais Decisões Técnicas

Entre as principais decisões adotadas estão:

- .NET 8 como plataforma;
- Clean Architecture de forma pragmática;
- conceitos de DDD sem complexidade desnecessária;
- `BusinessPartner` como entidade principal do agregado;
- relacionamento 1:N com `PhoneNumber`;
- inativação lógica em vez de exclusão física;
- Repository para abstração da persistência;
- Fluent API para manter o domínio independente do EF Core;
- migrations para versionamento do banco;
- contratos HTTP separados das entidades de domínio;
- Swagger/OpenAPI integrado ao código;
- testes automatizados desenvolvidos incrementalmente.

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

O objetivo foi entregar não apenas uma API funcional, mas uma solução compreensível, testável, documentada e preparada para evolução.
