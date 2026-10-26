# Tasks API

API REST para cadastro de usuários, autenticação e gerenciamento de tarefas, desenvolvida em C# com ASP.NET Core 10.

## Visão geral

A API oferece cadastro e consulta de usuários, login com JWT e operações CRUD de tarefas. As rotas de tarefas exigem autenticação e limitam cada usuário aos próprios registros.

## Tecnologias

- C# e .NET 10
- ASP.NET Core Controllers
- Entity Framework Core
- PostgreSQL com Npgsql
- JWT Bearer
- JSON

## Estrutura do projeto

- [controllers](controllers) — endpoints HTTP
- [services](services) — regras de negócio e autenticação
- [repositories](repositories) — consultas e persistência
- [models/DTOs](models/DTOs) — entidades de transporte e contratos JSON
- [data/configurations](data/configurations) — contexto e mapeamento do banco
- [enums](enums) — valores de prioridade e status
- [extensions](extensions) — registro de serviços, repositórios e autenticação
- [middlewares](middlewares) — tratamento de exceções
- [shared](shared) — respostas comuns e utilitários

## Pré-requisitos

- .NET SDK 10
- PostgreSQL acessível pela aplicação
- Um banco e schema compatíveis com os mapeamentos do Entity Framework Core

O repositório não contém migrations nem scripts SQL para criar o schema. A API espera encontrar as tabelas `usuarios` e `tarefas`; configure o banco antes de executar as chamadas de negócio. Os mapeamentos também usam a função PostgreSQL `uuidv7()` para gerar identificadores.

## Configuração

A aplicação requer estas configurações:

| Chave | Descrição |
| --- | --- |
| `ConnectionStrings:AppDbContext` | String de conexão PostgreSQL |
| `Jwt:SecretKey` | Chave de assinatura do token |
| `Jwt:Issuer` | Emissor esperado e incluído no token |
| `Jwt:Audience` | Público esperado e incluído no token |
| `Jwt:ExpiresInMinutes` | Validade do token em minutos |

No ambiente local, forneça os valores por variáveis de ambiente. O .NET converte `__` em `:` para nomes de configuração:

```powershell
$env:ConnectionStrings__AppDbContext = "<connection-string>"
$env:Jwt__SecretKey = "<strong-random-secret>"
$env:Jwt__Issuer = "<issuer>"
$env:Jwt__Audience = "<audience>"
$env:Jwt__ExpiresInMinutes = "120"
```

Use uma chave JWT longa e aleatória. Não versione credenciais ou chaves reais; em produção, use um gerenciador de segredos apropriado.

## Executar localmente

A partir do diretório `backend/`, depois de configurar as variáveis e preparar o banco:

```bash
dotnet restore
dotnet build
dotnet run --launch-profile http
```

O perfil `http` disponibiliza a API em `http://localhost:5000`. O perfil `https` usa `https://localhost:7079` e também `http://localhost:5000`.

Não há projeto de testes automatizados no repositório. `dotnet test` não executará uma suíte de testes deste projeto.

## Autenticação

`POST /auth` é público. As demais rotas descritas neste README exigem um token JWT válido no cabeçalho:

```http
Authorization: Bearer <access_token>
```

Isso também se aplica a `POST /users`: o cadastro não é anônimo na configuração atual. Para criar um usuário, é necessário já possuir um token válido; portanto, o endpoint não permite iniciar sozinho o primeiro cadastro de uma instalação.

O token valida emissor, público, assinatura e validade. A API atribui o usuário autenticado às tarefas no servidor: não envie `id_usuario` ao criar ou atualizar uma tarefa. As consultas e alterações de tarefas são restritas ao usuário do token.

## Respostas

As respostas de sucesso usam o envelope:

```json
{
  "success": true,
  "message": "...",
  "data": {}
}
```

As respostas de erro usam `success`, `message` e `error`. Uma requisição sem autenticação retorna `401` e uma resposta nesse formato.

## Endpoints

| Método | Rota | Acesso | Resultado |
| --- | --- | --- | --- |
| `POST` | `/users` | Autenticado | Cria usuário (`201`) |
| `GET` | `/users/{email}` | Autenticado | Busca usuário pelo e-mail (`200`) |
| `POST` | `/auth` | Público | Autentica e retorna token (`201`) |
| `POST` | `/tasks` | Autenticado | Cria tarefa (`201`) |
| `GET` | `/tasks` | Autenticado | Lista tarefas do usuário (`200`) |
| `GET` | `/tasks/{id}` | Autenticado | Busca tarefa do usuário (`200`) |
| `PUT` | `/tasks/{id}` | Autenticado | Atualiza tarefa do usuário (`200`) |
| `DELETE` | `/tasks/{id}` | Autenticado | Exclui tarefa do usuário (`200`) |

### Usuários

`POST /users` recebe:

```json
{
  "e_mail": "usuario@example.com",
  "password": "<password>"
}
```

A resposta de sucesso inclui o identificador em `data.id`. `GET /users/{email}` retorna `data` com `id` e `e_mail`; codifique o e-mail como componente de URL quando necessário.

### Autenticação

`POST /auth` recebe o mesmo formato de e-mail e senha do cadastro. A resposta inclui os campos abaixo em `data`:

```json
{
  "user_id": "<uuid>",
  "access_token": "<jwt>",
  "expires_in_minutes": 120
}
```

### Tarefas

`POST /tasks` e `PUT /tasks/{id}` recebem:

```json
{
  "titulo": "Revisar documentação",
  "descricao": "Conferir os endpoints da API",
  "prioridade": 1,
  "status": 0
}
```

`GET /tasks` lista somente as tarefas do usuário autenticado, ordenadas pela data de inclusão decrescente e, em seguida, pela prioridade decrescente. A rota não oferece filtros por query string.

O DTO retornado pela API também inclui `id`, `id_usuario`, `data_hora_incluido`, `data_hora_alterado`, `data_hora_completado`, `data_hora_cancelado` e `e_mail_usuario`, além dos campos enviados na entrada.

Prioridades (`prioridade`):

| Valor | Nome |
| --- | --- |
| `0` | `Low` |
| `1` | `Medium` |
| `2` | `High` |

Status (`status`):

| Valor | Nome |
| --- | --- |
| `0` | `Pending` |
| `1` | `InProgress` |
| `2` | `Completed` |
| `3` | `Cancelled` |

Exemplos de chamadas com cURL, usando `http://localhost:5000` como endereço local:

```bash
curl -X POST http://localhost:5000/users \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <existing_access_token>" \\
  -d '{"e_mail":"usuario@example.com","password":"<password>"}'

curl -X POST http://localhost:5000/auth \
  -H "Content-Type: application/json" \
  -d '{"e_mail":"usuario@example.com","password":"<password>"}'

curl http://localhost:5000/tasks \
  -H "Authorization: Bearer <access_token>"
```

## Banco de dados

O Entity Framework Core mapeia usuários para `usuarios` e tarefas para `tarefas`. Cada tarefa possui uma chave estrangeira `id_usuario` para o usuário proprietário. O banco limita `prioridade` ao intervalo de `0` a `2` e `status` de `0` a `3`; títulos e descrições têm limites de 50 e 150 caracteres, respectivamente.

Como o projeto não inclui migrations nem scripts de criação, o schema precisa ser provisionado separadamente antes da execução. Não há um comando `dotnet ef database update` pronto para inicializar o banco neste repositório.

## Observações de segurança

As senhas são atualmente transformadas em SHA-256 simples, sem salt ou função específica para armazenamento de senhas. Isso não é uma prática adequada para proteger credenciais em produção; use uma solução própria para hashing de senhas antes de disponibilizar o serviço com dados reais.

O arquivo [backend.http](backend.http) contém somente uma chamada de exemplo para `/weatherforecast` em uma porta diferente do perfil local e não inclui token. Ele não representa um roteiro de teste completo para as rotas de negócio.
