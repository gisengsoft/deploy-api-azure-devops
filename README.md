# API de Tarefas — Deploy na Nuvem com Azure DevOps

Projeto do desafio da **DIO** — *"Como fazer o Deploy de uma API na Nuvem na Prática"*.

Uma API REST simples (CRUD de tarefas) feita em **.NET 8**, com um **pipeline de CI/CD no Azure DevOps** que compila, empacota e (opcionalmente) faz o deploy automático no **Azure App Service** a cada `push` na branch `main`.

```
Desenvolvedor  ->  GitHub  ->  Azure Pipelines (Build + Publish)  ->  Azure App Service
```

---

## 🧱 Tecnologias

- **.NET 8** (ASP.NET Core Web API, com Controllers)
- **Azure DevOps Pipelines** (CI/CD via `azure-pipelines.yml`)
- **Azure App Service** (hospedagem na nuvem)
- **GitHub** (repositório do código)

> Sem banco de dados e sem pacotes externos: os dados ficam **em memória**, para manter o projeto simples e didático.

---

## 📁 Estrutura do projeto

```
deploy-api-azure-devops/
├── azure-pipelines.yml         # Pipeline de CI/CD (build + publish + deploy)
├── ApiTarefas.sln              # Solução (abre no Visual Studio)
├── README.md                   # Este arquivo
├── GUIA-DEPLOY.md              # Passo a passo completo do deploy
├── .gitignore
└── ApiTarefas/
    ├── ApiTarefas.csproj
    ├── Program.cs              # Configuração da aplicação
    ├── Controllers/
    │   └── TarefasController.cs  # Endpoints do CRUD
    ├── Models/
    │   └── Tarefa.cs           # Modelo de dados
    ├── Properties/
    │   └── launchSettings.json
    ├── appsettings.json
    └── appsettings.Development.json
```

---

## 🔌 Endpoints da API

Prefixo base: `/api/tarefas`

| Método | Rota                  | Descrição                       | Resposta de sucesso |
|--------|-----------------------|---------------------------------|---------------------|
| GET    | `/`                   | Health check (status da API)    | `200 OK`            |
| GET    | `/api/tarefas`        | Lista todas as tarefas          | `200 OK`            |
| GET    | `/api/tarefas/{id}`   | Busca uma tarefa pelo id        | `200 OK` / `404`    |
| POST   | `/api/tarefas`        | Cria uma nova tarefa            | `201 Created`       |
| PUT    | `/api/tarefas/{id}`   | Atualiza uma tarefa existente   | `204 No Content`    |
| DELETE | `/api/tarefas/{id}`   | Remove uma tarefa               | `204 No Content`    |

### Exemplo de corpo (JSON) para POST e PUT

```json
{
  "titulo": "Estudar Azure DevOps",
  "concluida": false
}
```

---

## ▶️ Rodando localmente

Pré-requisito: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
# Dentro da pasta do projeto
dotnet restore
dotnet run --project ApiTarefas
```

A API sobe em `http://localhost:5062`. Teste no navegador ou com `curl`:

```bash
# Listar tarefas
curl http://localhost:5062/api/tarefas

# Criar uma tarefa
curl -X POST http://localhost:5062/api/tarefas \
  -H "Content-Type: application/json" \
  -d '{"titulo":"Fazer deploy no Azure","concluida":false}'
```

---

## ☁️ Deploy na nuvem (Azure DevOps)

O passo a passo completo — criar o projeto no Azure DevOps, configurar o pipeline,
criar o App Service e ativar o deploy automático — está no arquivo **[GUIA-DEPLOY.md](GUIA-DEPLOY.md)**.

Resumo do fluxo:

1. Suba este repositório para o **GitHub**.
2. No **Azure DevOps**, crie um projeto e um **Pipeline** apontando para este repositório (ele detecta o `azure-pipelines.yml` automaticamente).
3. O pipeline **compila** e gera o **artefato** a cada push.
4. Crie um **App Service** no Azure e uma **Service Connection**.
5. Ative o estágio de **Deploy** no `azure-pipelines.yml` (instruções dentro do arquivo).

---

## ✅ O que foi testado

Todos os endpoints foram validados localmente antes da entrega:

- `GET /` → 200 (status online)
- `GET /api/tarefas` → 200 (lista)
- `GET /api/tarefas/{id}` → 200 / 404
- `POST /api/tarefas` → 201 (com validação 400 para título vazio)
- `PUT /api/tarefas/{id}` → 204
- `DELETE /api/tarefas/{id}` → 204
- `dotnet publish` gera o pacote de deploy corretamente
