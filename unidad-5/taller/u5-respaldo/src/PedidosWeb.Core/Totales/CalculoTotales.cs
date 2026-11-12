namespace PedidosWeb.Core.Totales;

/// <summary>Una línea del pedido, como TLineaCalculo en legado/src/UCalculoTotales.pas.</summary>
public record LineaCalculo(int Cantidad, decimal PrecioUnitario, string Categoria);

/// <summary>Resultado del cálculo, como TResultadoTotales en el legado.</summary>
public record ResultadoTotales(decimal Subtotal, decimal BaseDescuento, decimal PorcDescuento,
    decimal Descuento, decimal Flete, decimal Neto, decimal Iva, decimal Total);

public static class CalculoTotales
{
    /// <summary>Calcula los totales del pedido igual que UCalculoTotales.pas. Se implementa en la etapa 2.</summary>
    public static ResultadoTotales Calcular(IReadOnlyList<LineaCalculo> lineas, bool esVip,
        int region, decimal fleteBase, decimal tasaIva)
        => throw new NotImplementedException("Se migra en la etapa 2.");
}
