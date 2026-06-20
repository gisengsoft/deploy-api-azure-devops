namespace ApiTarefas.Models;

// Modelo que representa uma tarefa (item de lista de afazeres).
public class Tarefa
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool Concluida { get; set; }
}
