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
- consulta individual de telefone;
- atualização de telefone;
- remoção de telefone.

A inativação do `BusinessPartner` preserva o registro para fins de rastreabilidade através de:

```text
IsActive = false
DeactivatedAt = data/hora da inativação
```

Já a remoção de `PhoneNumber` representa uma exclusão física da entidade dependente pertencente ao agregado.

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

Atualmente a API disponibiliza **9 operações REST**.

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

As rotas de telefone permanecem subordinadas ao `BusinessPartner`, refletindo o relacionamento **1:N** definido no domínio.

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

Atualmente o Swagger documenta todas as **9 operações REST** disponibilizadas pela API.

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

O relacionamento entre `BusinessPartner` e `PhoneNumber` utiliza:

```csharp
DeleteBehavior.ClientCascade
```

Essa configuração permite que o Entity Framework Core trate corretamente a remoção explícita de um telefone pertencente ao agregado enquanto as entidades estão sendo rastreadas pelo contexto.

A decisão mantém a estratégia de persistência na camada Infrastructure e evita transferir detalhes do Entity Framework Core para o domínio ou para a Application.

---

## Estratégias de Remoção

A solução possui comportamentos distintos para as duas entidades:

| Entidade | Estratégia |
| --- | --- |
| `BusinessPartner` | Inativação lógica |
| `PhoneNumber` | Remoção física explícita |

O `BusinessPartner` permanece disponível após sua inativação, com:

```text
IsActive = false
DeactivatedAt = data/hora da inativação
```

Já um `PhoneNumber` removido deixa de existir na persistência.

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
| Domain.Tests | 22 | Aprovados |
| Application.Tests | 16 | Aprovados |
| Api.Tests | 15 | Aprovados |
| **Total** | **53** | **53 aprovados** |

A suíte pode ser executada através de:

```bash
dotnet test
```

Resultado atual:

```text
53 testes executados
53 testes aprovados
0 falhas
0 ignorados
```

Os testes cobrem:

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
- persistência efetiva da remoção.

A evolução da suíte acompanhou o crescimento funcional da solução:

```text
26 testes
    ↓
36 testes
    ↓
53 testes
```

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
- decisões técnicas;
- documentação específica sobre persistência e remoção de telefones.

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
- inativação lógica de `BusinessPartner`;
- remoção física explícita de `PhoneNumber`;
- `DeleteBehavior.ClientCascade` para tratamento da entidade dependente no EF Core;
- Repository para abstração da persistência;
- ausência de um `PhoneNumberRepository` sem necessidade concreta;
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

e toda a suíte foi novamente executada com sucesso:

```text
53 / 53 testes aprovados
```

O objetivo foi entregar não apenas uma API funcional, mas uma solução compreensível, testável, documentada e preparada para evolução.
