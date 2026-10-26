# Tasks Frontend

Aplicação web para acompanhar tarefas pessoais, desenvolvida com Angular 22. O frontend autentica usuários pela API Tasks e oferece um dashboard e operações de gerenciamento de tarefas.

## Tecnologias

- Angular 22 e TypeScript
- Angular Material
- RxJS
- npm

## Funcionalidades

- Login com e-mail e senha
- Dashboard com totais por status e tarefas recentes
- Listagem, criação, edição, detalhes e exclusão de tarefas
- Pesquisa local por título e descrição
- Filtros locais por status e prioridade
- Paginação local da lista
- Rotas protegidas para dashboard e tarefas

## Pré-requisitos

- Node.js `^22.22.3`, `^24.15.0` ou `>=26.0.0`, conforme os requisitos do Angular instalado
- npm 11 ou compatível com a versão registrada no `package.json`
- API do backend em execução
- PostgreSQL configurado para o backend, com o schema já criado

A API e os requisitos do banco estão descritos em [backend/README.md](../backend/README.md).

## Instalação e execução local

Em um terminal, no diretório `frontend/`:

```bash
npm ci
npm start
```

O servidor Angular fica disponível, por padrão, em `http://localhost:4200`.

O proxy de desenvolvimento em [proxy.conf.json](proxy.conf.json) encaminha solicitações iniciadas por `/api` para `http://localhost:5000` e remove o prefixo `/api`. Portanto, inicie o backend com o perfil HTTP na porta 5000 antes de usar o login ou as telas de tarefas. Consulte [backend/README.md](../backend/README.md) para configuração e inicialização da API.

## Integração com a API

A URL base usada pelo frontend é `/api`. No ambiente de desenvolvimento, o proxy encaminha chamadas como `/api/auth` e `/api/tasks` para `/auth` e `/tasks` no backend.

O login envia `e_mail` e `password` para `POST /auth`. O token recebido é salvo em `sessionStorage`; um interceptor o envia no cabeçalho `Authorization: Bearer <token>` nas demais chamadas HTTP. As rotas de dashboard e tarefas são protegidas por um guard no frontend, e a API também valida a autenticação.

A listagem envia o parâmetro de query `user-id`, mas o backend atual não usa esse filtro. A API determina o usuário pelas informações do token e retorna apenas as tarefas desse usuário. Pesquisa, filtros e paginação são aplicados localmente pelo frontend.

### Usuário existente necessário

O login exige uma conta já existente. Embora a API tenha `POST /users`, essa rota também exige JWT; como o frontend não possui cadastro, o primeiro usuário precisa ser provisionado por outro meio. Consulte o README do backend para os detalhes atuais da API.

## Produção

O endereço `/api` é relativo. Em produção, configure o servidor web ou reverse proxy para encaminhar `/api` à API backend, removendo esse prefixo conforme necessário. O proxy definido em [proxy.conf.json](proxy.conf.json) é usado pelo servidor de desenvolvimento e não configura o servidor de produção.

Se a API for hospedada em outra origem sem reverse proxy, será necessário ajustar a URL usada pelo frontend e configurar CORS no backend para a origem do frontend.

## Build e testes

Para gerar o build de produção:

```bash
npm run build
```
