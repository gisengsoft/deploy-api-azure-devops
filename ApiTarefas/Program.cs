var builder = WebApplication.CreateBuilder(args);

// Registra o suporte a Controllers (onde ficam os endpoints da API)
builder.Services.AddControllers();

var app = builder.Build();

// Endpoint raiz: serve como "health check" para confirmar que a API está no ar.
// Útil também depois do deploy para abrir a URL do Azure e ver se respondeu.
app.MapGet("/", () => Results.Ok(new
{
    api = "API de Tarefas",
    status = "online",
    versao = "1.0.0",
    documentacao = "Use os endpoints abaixo (prefixo /api/tarefas)",
    endpoints = new[]
    {
        "GET    /api/tarefas",
        "GET    /api/tarefas/{id}",
        "POST   /api/tarefas",
        "PUT    /api/tarefas/{id}",
        "DELETE /api/tarefas/{id}"
    }
}));

// Mapeia as rotas dos Controllers
app.MapControllers();

app.Run();
