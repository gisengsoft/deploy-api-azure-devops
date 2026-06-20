using Microsoft.AspNetCore.Mvc;
using ApiTarefas.Models;

namespace ApiTarefas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TarefasController : ControllerBase
{
    // Armazenamento EM MEMÓRIA (sem banco de dados, para manter o projeto simples).
    // Os dados reiniciam toda vez que a API é reiniciada — é proposital para fins didáticos.
    private static readonly List<Tarefa> _tarefas = new()
    {
        new Tarefa { Id = 1, Titulo = "Estudar Azure DevOps", Concluida = false },
        new Tarefa { Id = 2, Titulo = "Criar o pipeline CI/CD", Concluida = false }
    };
    private static int _proximoId = 3;

    // GET /api/tarefas  -> lista todas as tarefas
    [HttpGet]
    public ActionResult<IEnumerable<Tarefa>> Listar()
    {
        return Ok(_tarefas);
    }

    // GET /api/tarefas/5  -> busca uma tarefa pelo id
    [HttpGet("{id:int}")]
    public ActionResult<Tarefa> ObterPorId(int id)
    {
        var tarefa = _tarefas.FirstOrDefault(t => t.Id == id);
        if (tarefa is null)
            return NotFound(new { mensagem = $"Tarefa {id} nao encontrada." });

        return Ok(tarefa);
    }

    // POST /api/tarefas  -> cria uma nova tarefa
    [HttpPost]
    public ActionResult<Tarefa> Criar([FromBody] Tarefa novaTarefa)
    {
        if (string.IsNullOrWhiteSpace(novaTarefa.Titulo))
            return BadRequest(new { mensagem = "O titulo da tarefa e obrigatorio." });

        novaTarefa.Id = _proximoId++;
        _tarefas.Add(novaTarefa);

        // Retorna 201 Created com o cabecalho Location apontando para a nova tarefa
        return CreatedAtAction(nameof(ObterPorId), new { id = novaTarefa.Id }, novaTarefa);
    }

    // PUT /api/tarefas/5  -> atualiza uma tarefa existente
    [HttpPut("{id:int}")]
    public IActionResult Atualizar(int id, [FromBody] Tarefa dados)
    {
        var tarefa = _tarefas.FirstOrDefault(t => t.Id == id);
        if (tarefa is null)
            return NotFound(new { mensagem = $"Tarefa {id} nao encontrada." });

        if (string.IsNullOrWhiteSpace(dados.Titulo))
            return BadRequest(new { mensagem = "O titulo da tarefa e obrigatorio." });

        tarefa.Titulo = dados.Titulo;
        tarefa.Concluida = dados.Concluida;

        return NoContent();
    }

    // DELETE /api/tarefas/5  -> remove uma tarefa
    [HttpDelete("{id:int}")]
    public IActionResult Remover(int id)
    {
        var tarefa = _tarefas.FirstOrDefault(t => t.Id == id);
        if (tarefa is null)
            return NotFound(new { mensagem = $"Tarefa {id} nao encontrada." });

        _tarefas.Remove(tarefa);

        return NoContent();
    }
}
