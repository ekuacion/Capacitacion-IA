namespace ArriendoWeb.Core;

/// <summary>
/// TarifaBase migrada desde legado/src/UTarifas.pas (demos de la Unidad 5).
/// Calcula igual que el legado: 20 de 20 casos de migracion/tarifas-golden.csv.
/// </summary>
public static class Tarifas
{
    // Temporada alta: diciembre, enero y febrero (UTarifas.pas: FactorTemporada).
    public static decimal FactorTemporada(DateTime fecha) =>
        fecha.Month is 12 or 1 or 2 ? 1.15m : 1m;

    // Cliente frecuente: 5 o más arriendos (UTarifas.pas: FactorFrecuente).
    public static decimal FactorFrecuente(int cantArriendos) =>
        cantArriendos >= 5 ? 0.90m : 1m;

    // Round de Delphi redondea al par: A08 da 341446,5 y queda en 341446.
    public static decimal TarifaBase(decimal tarifaDia, int dias, DateTime fechaSalida, int cantArriendos)
    {
        var bruto = tarifaDia * dias * FactorTemporada(fechaSalida) * FactorFrecuente(cantArriendos);
        return Math.Round(bruto, 0, MidpointRounding.ToEven);
    }
}
