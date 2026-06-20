# Guia de Deploy — API na Nuvem com Azure DevOps

Passo a passo completo para colocar a API de Tarefas no ar usando **GitHub + Azure DevOps + Azure App Service**.

> 💡 Os nomes de alguns botões/menus na Azure podem mudar com o tempo. Se não achar um item com o nome exato, procure a opção equivalente — o fluxo continua o mesmo.

---

## Visão geral do fluxo

```
1. Codigo no GitHub
        |
2. Azure DevOps detecta o push  ->  roda o pipeline (azure-pipelines.yml)
        |
3. Pipeline compila e gera o artefato (.zip)
        |
4. Pipeline faz o deploy no Azure App Service
        |
5. API no ar em https://SEU-APP.azurewebsites.net
```

---

## Pré-requisitos

- Conta no **GitHub** (você já tem: `gisengsoft`).
- Conta no **Azure DevOps** — grátis em https://dev.azure.com
- Conta no **Azure** (portal) — grátis em https://portal.azure.com
  (o nível gratuito *F1* do App Service é suficiente para testar)
- **Git** instalado (você já tem o Git Bash).

---

## PARTE 1 — Subir o código para o GitHub

> O passo a passo com os comandos do Git Bash está no final desta página (seção **"Comandos Git"**).

Depois de subir, confirme que o repositório no GitHub contém o arquivo `azure-pipelines.yml` na raiz.

---

## PARTE 2 — Criar o projeto no Azure DevOps

1. Acesse https://dev.azure.com e faça login.
2. Clique em **New project** (Novo projeto).
3. Dê um nome (ex.: `DIO-API`), deixe como **Private** e clique em **Create**.

---

## PARTE 3 — Criar o Pipeline (CI)

1. Dentro do projeto, no menu lateral, clique em **Pipelines**.
2. Clique em **Create Pipeline** (ou **New pipeline**).
3. Em *"Where is your code?"*, escolha **GitHub**.
4. Autorize o Azure Pipelines a acessar sua conta do GitHub (login/OAuth).
5. Selecione o repositório deste projeto.
6. Em *"Configure your pipeline"*, escolha **Existing Azure Pipelines YAML file**.
7. No campo *Path*, selecione **/azure-pipelines.yml** e clique em **Continue**.
8. Revise o conteúdo (tela *"Review your pipeline YAML"*) e clique em **Run**.

✅ O pipeline vai rodar a etapa **Build**: instala o .NET 8, compila, publica e gera o
artefato chamado **drop**. Quando aparecer o ✔️ verde, a integração contínua está funcionando.

> Você pode ver o artefato gerado clicando na execução do pipeline > **Related** / **Artifacts**.

---

## PARTE 4 — Criar o App Service no Azure (onde a API vai rodar)

1. Acesse https://portal.azure.com
2. Clique em **Create a resource** > **Web App**.
3. Preencha:
   - **Subscription**: sua assinatura.
   - **Resource Group**: clique em *Create new* (ex.: `rg-dio-api`).
   - **Name**: um nome único no mundo (ex.: `dio-api-tarefas-gisengsoft`).
     Esta será a URL: `https://SEU-NOME.azurewebsites.net`
   - **Publish**: **Code**
   - **Runtime stack**: **.NET 8 (LTS)**
   - **Operating System**: **Linux**
   - **Region**: a mais próxima (ex.: *Brazil South*).
   - **Pricing plan**: escolha o plano **Free F1** (para testes).
4. Clique em **Review + create** e depois **Create**.
5. Aguarde a criação e **anote o nome exato** do App Service.

---

## PARTE 5 — Criar a Service Connection (ligação Azure DevOps ↔ Azure)

Isso autoriza o pipeline a publicar no seu App Service.

1. No **Azure DevOps**, dentro do projeto, clique em **Project Settings**
   (engrenagem, no canto inferior esquerdo).
2. No menu, clique em **Service connections**.
3. Clique em **New service connection** > **Azure Resource Manager** > **Next**.
4. Escolha a opção automática (ex.: **Workload Identity federation (automatic)**
   ou **Service principal (automatic)**).
5. Selecione sua **Subscription**.
6. Em **Resource group**, pode selecionar o `rg-dio-api` (ou deixar no escopo da assinatura).
7. Dê um nome para a conexão em **Service connection name** (ex.: `conexao-azure`).
8. Marque **Grant access permission to all pipelines** e clique em **Save**.

> 📌 Guarde o nome da conexão (`conexao-azure`) — você vai usá-lo no próximo passo.

---

## PARTE 6 — Ativar o Deploy (CD) no pipeline

1. Abra o arquivo **`azure-pipelines.yml`** (no seu PC).
2. Localize o bloco comentado **"ETAPA 2: DEPLOY no Azure App Service"** (no final do arquivo).
3. **Remova o `#`** do início de todas as linhas daquele bloco (descomente o estágio `Deploy`).
4. Troque os dois valores:
   - `azureSubscription: 'COLOQUE-O-NOME-DA-SUA-SERVICE-CONNECTION'`
     → coloque o nome da conexão criada na Parte 5 (ex.: `conexao-azure`).
   - `appName: 'COLOQUE-O-NOME-DO-SEU-APP-SERVICE'`
     → coloque o nome do App Service criado na Parte 4.
5. Salve e faça **commit + push** (veja os comandos no final).

O push dispara o pipeline novamente. Agora ele roda **Build** e depois **Deploy**.

---

## PARTE 7 — Testar a API no ar

Quando o pipeline terminar com ✔️, abra no navegador:

```
https://SEU-APP.azurewebsites.net/
```

Você verá o JSON de status (health check). Teste os endpoints:

```
https://SEU-APP.azurewebsites.net/api/tarefas
```

Ou via curl / Postman:

```bash
curl https://SEU-APP.azurewebsites.net/api/tarefas

curl -X POST https://SEU-APP.azurewebsites.net/api/tarefas \
  -H "Content-Type: application/json" \
  -d '{"titulo":"Minha primeira tarefa na nuvem","concluida":false}'
```

🎉 **Pronto! Sua API está rodando na nuvem com deploy automático (CI/CD).**

A cada novo `git push` na branch `main`, o Azure DevOps recompila e republica sozinho.

---

## Comandos Git (Git Bash)

Veja o **passo a passo de comandos** na resposta do chat (ou no README).
Resumo para o primeiro envio:

```bash
cd caminho/para/deploy-api-azure-devops
git init
git add .
git commit -m "Projeto inicial: API de Tarefas + pipeline Azure DevOps"
git branch -M main
git remote add origin https://github.com/gisengsoft/NOME-DO-REPOSITORIO.git
git push -u origin main
```

Para enviar alterações depois (ex.: após ativar o deploy):

```bash
git add .
git commit -m "Ativa estagio de deploy no Azure App Service"
git push
```

---

## Problemas comuns

| Problema | Solução |
|----------|---------|
| Pipeline não dispara | Confirme que o push foi na branch `main` e que o `azure-pipelines.yml` está na raiz. |
| Erro de permissão no deploy | Verifique se a Service Connection tem acesso e se o nome no YAML está idêntico. |
| `appName` não encontrado | O nome no YAML deve ser **exatamente** igual ao do App Service no portal. |
| Página mostra erro 500 ao abrir | Aguarde 1–2 min após o deploy (o App Service inicializa) e recarregue. |
| Quer ver os logs | No portal do Azure: App Service > **Log stream**. |
