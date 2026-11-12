using Microsoft.AspNetCore.Mvc;
using PedidosWeb.Core.Lineas;
using PedidosWeb.Core.Pedidos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPedidoRepositorio, PedidoRepositorioEnMemoria>();
builder.Services.AddSingleton<BuscadorPedidos>();
builder.Services.AddSingleton<ILineaPedidoRepositorio, LineaPedidoRepositorioEnMemoria>();
builder.Services.AddSingleton<ServicioDetallePedido>();

var app = builder.Build();

// Para comprobar que la API está arriba.
app.MapGet("/salud", () => Results.Ok(new { estado = "ok" }));

// Detalle de un pedido por id.
app.MapGet("/api/pedidos/{id:int}", (int id, IPedidoRepositorio repositorio) =>
    repositorio.ObtenerPorId(id) is { } pedido
        ? Results.Ok(pedido)
        : Results.NotFound());

// HU-01 · Listar pedidos con filtros (specs/HU-01-listar-pedidos/spec.md).
// Los parámetros llegan como texto: la validación y los errores {codigo, mensaje} los decide Core.
app.MapGet("/api/pedidos/buscar", (
    [FromQuery] string? clienteId,
    [FromQuery] string? estado,
    [FromQuery] string? sucursal,
    [FromQuery] string? desde,
    [FromQuery] string? hasta,
    [FromQuery] string? pagina,
    [FromQuery] string? tamanoPagina,
    BuscadorPedidos buscador) =>
{
    var resultado = buscador.Buscar(
        new ConsultaPedidos(clienteId, estado, sucursal, desde, hasta, pagina, tamanoPagina));

    return resultado.EsExito
        ? Results.Ok(resultado.Pagina)
        : Results.BadRequest(resultado.Error);
});

// HU-02 · Detalle de un pedido (specs/HU-02-detalle-pedido/spec.md).
// Core arma el detalle o decide el error {codigo, mensaje}; aquí solo se traduce a HTTP.
app.MapGet("/api/pedidos/{id:int}/detalle", (int id, ServicioDetallePedido servicio) =>
{
    var resultado = servicio.Obtener(id);

    return resultado.EsExito
        ? Results.Ok(resultado.Detalle)
        : Results.NotFound(resultado.Error);
});

app.Run();

// Permite crear tests de integración con WebApplicationFactory<Program> más adelante.
public partial class Program { }
