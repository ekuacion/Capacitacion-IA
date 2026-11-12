namespace PedidosWeb.Core.Totales;

/// <summary>Una línea del pedido (equivale a TLineaCalculo en legado/src/UCalculoTotales.pas).</summary>
public record LineaCalculo(int Cantidad, decimal PrecioUnitario, string Categoria);

/// <summary>Resultado del cálculo en CLP (equivale a TResultadoTotales en el legado).</summary>
public record ResultadoTotales(
    decimal Subtotal,
    decimal BaseDescuento,
    decimal PorcDescuento,
    decimal Descuento,
    decimal Neto,
    decimal Flete,
    decimal Iva,
    decimal Total);

/// <summary>
/// Cálculo de totales del pedido, migrado desde legado/src/UCalculoTotales.pas.
/// Calcula igual que el legado, incluidos sus defectos (el golden master es el contrato).
/// </summary>
public static class CalculoTotales
{
    private const string CategoriaOferta = "OFE";
    private const decimal UmbralDescuentoAlto = 1_000_000m;
    private const decimal UmbralDescuentoMedio = 500_000m;

    public static ResultadoTotales Calcular(
        IReadOnlyList<LineaCalculo> lineas, bool esVip, int region, decimal fleteBase, decimal tasaIva)
    {
        var subtotal = CalcularSubtotal(lineas);
        var baseDescuento = CalcularBaseDescuento(lineas);
        var porcDescuento = PorcentajeDescuento(baseDescuento, esVip);
        var descuento = RedondearAlPeso(baseDescuento * porcDescuento);
        var neto = subtotal - descuento;
        var flete = lineas.Count == 0 ? 0m : CalcularFlete(fleteBase, region);
        var iva = RedondearAlPeso((neto + flete) * tasaIva);
        var total = neto + flete + iva;

        return new ResultadoTotales(subtotal, baseDescuento, porcDescuento, descuento, neto, flete, iva, total);
    }

    /// <summary>
    /// Redondeo al peso con 0,50 hacia arriba (corrección R05). El legado (Calc3, UCalculoTotales.pas:50)
    /// hacía Trunc(valor + 0,49), que bajaba la fracción de 0,50.
    /// </summary>
    private static decimal RedondearAlPeso(decimal valor) => Math.Truncate(valor + 0.5m);

    private static decimal CalcularSubtotal(IReadOnlyList<LineaCalculo> lineas)
        => lineas.Sum(l => l.Cantidad * l.PrecioUnitario);

    /// <summary>Las líneas de categoría OFE (sin espacios ni mayúsculas) no entran al descuento.</summary>
    private static decimal CalcularBaseDescuento(IReadOnlyList<LineaCalculo> lineas)
        => lineas
            .Where(l => !string.Equals(l.Categoria.Trim(), CategoriaOferta, StringComparison.OrdinalIgnoreCase))
            .Sum(l => l.Cantidad * l.PrecioUnitario);

    /// <summary>Regla del grabado del legado: umbrales con &gt;= (H1 corregido en TEC-05, decisión del PO).</summary>
    private static decimal PorcentajeVolumen(decimal baseDescuento) => baseDescuento switch
    {
        >= UmbralDescuentoAlto => 0.08m,
        >= UmbralDescuentoMedio => 0.05m,
        _ => 0m,
    };

    /// <summary>Volumen + 3 % si el cliente es VIP, con tope de 10 %.</summary>
    private static decimal PorcentajeDescuento(decimal baseDescuento, bool esVip)
    {
        var porcentaje = PorcentajeVolumen(baseDescuento);
        if (esVip) porcentaje += 0.03m;
        return Math.Min(porcentaje, 0.10m);
    }

    /// <summary>Regiones 11 y 12 pagan 15 % más de flete base.</summary>
    private static decimal CalcularFlete(decimal fleteBase, int region)
        => region is 11 or 12 ? RedondearAlPeso(fleteBase * 1.15m) : RedondearAlPeso(fleteBase);
}
