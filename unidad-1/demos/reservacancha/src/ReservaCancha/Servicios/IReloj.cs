namespace ReservaCancha.Servicios;

/// <summary>Da la hora actual. Existe para que los tests puedan fijar «ahora».</summary>
public interface IReloj
{
    DateTime Ahora { get; }
}

/// <summary>Reloj real del sistema (hora local del recinto).</summary>
public sealed class RelojSistema : IReloj
{
    public DateTime Ahora => DateTime.Now;
}
