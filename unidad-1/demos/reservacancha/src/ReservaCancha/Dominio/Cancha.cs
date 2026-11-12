namespace ReservaCancha.Dominio;

public enum TipoCancha
{
    Futbolito5,
    Futbolito7,
    Futbol11
}

public sealed record Cancha(int Id, string Nombre, TipoCancha Tipo)
{
    /// <summary>Las canchas del recinto «La Pelota» (ver README).</summary>
    public static IReadOnlyList<Cancha> DelRecinto { get; } = new[]
    {
        new Cancha(1, "Cancha Norte", TipoCancha.Futbolito5),
        new Cancha(2, "Cancha Sur", TipoCancha.Futbolito7),
        new Cancha(3, "Cancha Estadio", TipoCancha.Futbol11)
    };
}
