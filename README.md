# BarberBoss

API para gerenciamento de cobranças de uma barbearia, construída com ASP.NET Core e persistência em MySQL.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- MySQL 8
- Docker e Docker Compose
- xUnit para testes automatizados

## Estrutura da solução

```text
src/
├── BarberBoss.API             # API, controllers, filtros e configuração da aplicação
├── BarberBoss.Application     # Casos de uso, validações, mapeamentos e relatórios
├── BarberBoss.Domain          # Entidades, enums, contratos e regras do domínio
├── BarberBoss.Infrastructure  # EF Core, DbContext, repositórios e Unit of Work
├── BarberBoss.Communcations   # Requests, responses, filtros e constantes compartilhadas
└── BarberBoss.Exceptions      # Exceções e mensagens de erro

tests/
└── Validators.Tests           # Testes dos validadores
```

A aplicação segue uma separação em camadas. A API recebe as requisições HTTP, a camada Application coordena os casos de uso, o Domain concentra os contratos e entidades e a Infrastructure acessa o banco de dados.

## Funcionalidades

O recurso `Billings` disponibiliza:

- Listagem paginada de cobranças
- Consulta de cobrança por identificador
- Cadastro de cobrança
- Atualização de cobrança
- Exclusão de cobrança
- Geração de relatórios em PDF e Excel na camada Application

## Pacotes utilizados

### `BarberBoss.API`

- `Microsoft.AspNetCore.OpenApi` `10.0.14` — suporte à documentação OpenAPI.
- `Scalar.AspNetCore` `2.19.0` — interface para explorar a API documentada.
- `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` `1.24.2-preview.1` — integração com contêineres no Visual Studio.

### `BarberBoss.Application`

- `AutoMapper` `16.3.0` — mapeamento entre requests, entidades e responses.
- `ClosedXML` `0.105.1` — criação e manipulação de relatórios Excel.
- `FluentValidation` `12.2.0` — validação dos dados de entrada.
- `Microsoft.Extensions.DependencyInjection` `9.0.22` — registro das dependências da aplicação.
- `PDFsharp-MigraDoc` `6.1.0` — geração de relatórios PDF.

### `BarberBoss.Infrastructure`

- `Microsoft.EntityFrameworkCore` `9.0.22` — ORM e controle da persistência.
- `Pomelo.EntityFrameworkCore.MySql` `9.0.0` — provedor do Entity Framework Core para MySQL.

### `tests/Validators.Tests`

- `xunit` `2.9.3` — framework de testes.
- `xunit.runner.visualstudio` `3.1.4` — execução dos testes no Visual Studio e no `dotnet test`.
- `Microsoft.NET.Test.Sdk` `17.14.1` — infraestrutura de execução dos testes.
- `FluentAssertions` `8.11.0` — asserções legíveis.
- `coverlet.collector` `6.0.4` — coleta de cobertura de testes.

## Pré-requisitos

Instale os seguintes recursos:

- .NET SDK 10
- Docker Desktop ou Docker Engine com Docker Compose
- Git

Verifique as instalações:

```bash
dotnet --version
docker --version
docker compose version
```

## Configuração do banco de dados

O projeto contém um `docker-compose.yml` que inicia um MySQL 8 com os seguintes valores de desenvolvimento:

| Configuração | Valor |
|---|---|
| Host | `localhost` |
| Porta | `3306` |
| Banco | `barberbossdb` |
| Usuário | `admin_user` |
| Senha | definida no `docker-compose.yml` |

A connection string usada pela API está em `src/BarberBoss.API/appsettings.Development.json` e deve corresponder às credenciais do contêiner.

> Não utilize essas credenciais em produção. Para ambientes reais, use variáveis de ambiente, User Secrets ou um gerenciador de segredos.

## Como executar localmente

### 1. Clonar o repositório

```bash
git clone <url-do-repositorio>
cd BarberBoss
```

### 2. Iniciar o MySQL

Na raiz do projeto, execute:

```bash
docker compose up -d mysql-dev
```

Confira se o contêiner está em execução:

```bash
docker compose ps
```

A API precisa conseguir acessar `localhost:3306`. Se a porta já estiver ocupada, ajuste o mapeamento no `docker-compose.yml` e a porta na connection string.

### 3. Restaurar dependências e compilar

```bash
dotnet restore src/BarberBoss.API/BarberBoss.API.csproj
dotnet build src/BarberBoss.API/BarberBoss.API.csproj
```

### 4. Executar a API

```bash
dotnet run --project src/BarberBoss.API/BarberBoss.API.csproj
```

Com o perfil padrão de desenvolvimento, os endereços são:

- HTTP: `http://localhost:5253`
- HTTPS: `https://localhost:7192`

Durante o desenvolvimento, a documentação OpenAPI e a interface Scalar ficam disponíveis quando configuradas pela aplicação. A URL pode ser exibida no console após iniciar a API.

### 5. Executar os testes

```bash
dotnet test tests/Validators.Tests/Validators.Tests.csproj
```

Para coletar cobertura:

```bash
dotnet test tests/Validators.Tests/Validators.Tests.csproj \
  --collect:"XPlat Code Coverage"
```

## Endpoints de cobranças

A rota base é `/api/Billings`.

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/Billings` | Lista cobranças com paginação |
| `GET` | `/api/Billings/{id}` | Consulta uma cobrança por GUID |
| `POST` | `/api/Billings` | Cadastra uma cobrança |
| `PUT` | `/api/Billings/{id}` | Atualiza uma cobrança |
| `DELETE` | `/api/Billings/{id}` | Exclui uma cobrança |

Exemplo de requisição para cadastro:

```http
POST http://localhost:5253/api/Billings
Content-Type: application/json

{
  "description": "Corte masculino",
  "price": 45.00,
  "paymentMethod": 0
}
```

Os nomes e valores exatos dos campos devem seguir o contrato `RequestRegisterBillingJson` e os enums definidos no projeto.

## Executar com Docker

O `docker-compose.yml` atualmente configura o banco de dados. A API possui um `Dockerfile` próprio e pode ser criada com:

```bash
docker build -f src/BarberBoss.API/Dockerfile -t barberboss-api .
docker run --rm -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  barberboss-api
```

Ao executar a API em um contêiner separado, `localhost` aponta para o próprio contêiner da API. Nesse cenário, configure o host do MySQL como o nome do serviço Docker (`mysql-dev`) e coloque os contêineres na mesma rede. Para desenvolvimento local com `dotnet run`, mantenha `localhost` na connection string.

## Parando o ambiente

Para parar o banco sem remover os dados:

```bash
docker compose stop mysql-dev
```

Para remover o contêiner e também o volume persistido:

```bash
docker compose down -v
```

> O comando `down -v` apaga os dados locais do MySQL.

## Observações

- O banco deve estar disponível antes de realizar operações que gravem ou consultem dados.
- A mensagem `Unable to connect to any of the specified MySQL hosts` normalmente indica que o MySQL não está em execução, está em outra porta ou que o host não corresponde ao ambiente onde a API está rodando.
- A configuração de retry do EF Core pode ajudar em falhas transitórias, mas não substitui a correção de host, porta, credenciais ou disponibilidade do MySQL.
