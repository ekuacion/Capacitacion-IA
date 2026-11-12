namespace ReservaCancha.Dominio;

/// <summary>Una reserva de una cancha. Dura siempre 1 hora (regla R1).</summary>
public sealed record Reserva(int CanchaId, DateTime Inicio, string Cliente)
{
    public DateTime Fin => Inicio.AddHours(1);
}

/// <summary>Resultado de intentar una reserva: éxito o el mensaje de error para el cliente.</summary>
public sealed record ResultadoReserva(bool Exito, string? Error)
{
    public static ResultadoReserva Ok() => new(true, null);

    public static ResultadoReserva Falla(string error) => new(false, error);
}
