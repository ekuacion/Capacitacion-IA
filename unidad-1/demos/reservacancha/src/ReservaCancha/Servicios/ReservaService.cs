using ReservaCancha.Dominio;

namespace ReservaCancha.Servicios;

/// <summary>Valida y registra reservas. Las reglas están numeradas en el README (R1 a R7).</summary>
public sealed class ReservaService
{
    private readonly IReadOnlyList<Cancha> _canchas;
    private readonly IReloj _reloj;
    private readonly List<Reserva> _reservas = new();

    public ReservaService(IEnumerable<Cancha> canchas, IReloj reloj)
    {
        _canchas = canchas.ToList();
        _reloj = reloj;
    }

    public IReadOnlyList<Reserva> Reservas => _reservas;

    public ResultadoReserva Reservar(Reserva reserva)
    {
        // R2
        if (string.IsNullOrWhiteSpace(reserva.Cliente))
            return ResultadoReserva.Falla("El nombre del cliente es obligatorio.");

        // R3
        if (!_canchas.Any(c => c.Id == reserva.CanchaId))
            return ResultadoReserva.Falla("La cancha no existe.");

        // R4
        if (reserva.Inicio < _reloj.Ahora)
            return ResultadoReserva.Falla("No se puede reservar en el pasado.");

        // R6 y R7: pendientes (issue #12).

        // R5: dos reservas se cruzan si una empieza antes de que la otra termine.
        var seCruza = _reservas.Any(r =>
            r.CanchaId == reserva.CanchaId &&
            r.Inicio < reserva.Fin &&
            reserva.Inicio < r.Fin);
        if (seCruza)
            return ResultadoReserva.Falla("La cancha ya está reservada en ese horario.");

        _reservas.Add(reserva);
        return ResultadoReserva.Ok();
    }
}
