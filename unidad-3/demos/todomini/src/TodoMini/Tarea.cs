namespace TodoMini;

/// <summary>Una tarea de la lista.</summary>
public sealed class Tarea
{
    public Tarea(int id, string titulo, DateTimeOffset creadaEn)
    {
        Id = id;
        Titulo = titulo;
        CreadaEn = creadaEn;
    }

    public int Id { get; }

    public string Titulo { get; }

    public bool Completada { get; private set; }

    /// <summary>Momento de creación, en UTC.</summary>
    public DateTimeOffset CreadaEn { get; }

    public void Completar() => Completada = true;
}
