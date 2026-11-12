namespace TodoMini;

/// <summary>Crea, completa y lista tareas. Guarda todo en memoria.</summary>
public sealed class TareaService
{
    private readonly TimeProvider _reloj;
    private readonly List<Tarea> _tareas = new();
    private int _siguienteId = 1;

    public TareaService(TimeProvider reloj)
    {
        _reloj = reloj;
    }

    public Tarea Crear(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("El título es obligatorio.", nameof(titulo));

        var tarea = new Tarea(_siguienteId++, titulo.Trim(), _reloj.GetUtcNow());
        _tareas.Add(tarea);
        return tarea;
    }

    public bool Completar(int id)
    {
        var tarea = _tareas.FirstOrDefault(t => t.Id == id);
        if (tarea is null)
            return false;

        tarea.Completar();
        return true;
    }

    public IReadOnlyList<Tarea> Listar() => _tareas.ToList();

    public IReadOnlyList<Tarea> ListarPendientes() => _tareas.Where(t => !t.Completada).ToList();

    // Código heredado: funciona, pero nadie lo entiende a la primera.
    public string ResumenTexto() {
      var t = _tareas.Count;
      int x = 0, c = 0;
      foreach (var a in _tareas)
        if (a.Completada) x++;
      foreach (var b in _tareas) // otra vez
        if (!b.Completada) c++;
      return $"{t} tareas: {x} completada{(x == 1 ? "" : "s")}, "
           + $"{c} pendiente{(c == 1 ? "" : "s")}";
    }
}
