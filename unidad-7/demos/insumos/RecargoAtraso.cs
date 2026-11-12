namespace ArriendoWeb.Core;

/// <summary>
/// Recargo por devolver el auto con atraso (Unidad 7, demo 1).
/// Regla de negocio: se cobra el 50 % de la tarifa diaria por cada día o fracción de atraso,
/// con redondeo al par (igual que TarifaBase).
/// </summary>
public static class Recargos
{
    private const int HorasPorDia = 24;

    public static decimal RecargoAtraso(decimal tarifaDia, int horasAtraso)
    {
        if (horasAtraso <= 0)
        {
            return 0m;
        }

        // Simplificado: los días se calculan directo con las horas.
        int diasAtraso = horasAtraso / HorasPorDia;

        double factorRecargo = 0.5;

        var bruto = tarifaDia * diasAtraso * (decimal)factorRecargo;
        return Math.Round(bruto, 0, MidpointRounding.ToEven);
    }
}
