using System.Net;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PedidosWeb.Api.Tests.Pedidos;

/// <summary>Tests HTTP de GET /api/pedidos/{id}/detalle (HU-02, spec v1.1: CA-02, CA-03 y CA-04).</summary>
public class DetallePedidoEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _cliente;

    public DetallePedidoEndpointTests(WebApplicationFactory<Program> fabrica)
    {
        _cliente = fabrica.CreateClient();
    }

    [Fact]
    public async Task GetDetalle_Pedido8_Devuelve200ConElJsonDeLaSpec()
    {
        // CA-02: el cuerpo debe ser exactamente el ejemplo del pedido 8 de la spec.
        const string ejemplo = """
            {
              "id": 8,
              "folio": "PED-STGO-000008",
              "fecha": "2026-10-20",
              "estado": "Entregado",
              "sucursal": "STGO",
              "clienteId": "C-001",
              "clienteNombre": "Ferretería El Clavo",
              "lineas": [
                { "numero": 1, "producto": "Harina para pan 25 kg", "cantidad": 3,  "precioUnitario": 21500, "subtotal": 64500 },
                { "numero": 2, "producto": "Mantequilla 250 g",     "cantidad": 12, "precioUnitario": 2950,  "subtotal": 35400 },
                { "numero": 3, "producto": "Bebida cola 1,5 L",     "cantidad": 10, "precioUnitario": 2020,  "subtotal": 20200 }
              ],
              "total": 120100
            }
            """;

        var respuesta = await _cliente.GetAsync("/api/pedidos/8/detalle");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        await AssertJsonIgualAsync(ejemplo, respuesta);
    }

    [Fact]
    public async Task GetDetalle_PedidoInexistente_Devuelve404ConCuerpoDeError()
    {
        // CA-03: el 404 lleva el error {codigo, mensaje} para que un frontend pueda informar al usuario.
        const string esperado = """
            { "codigo": "PEDIDO_NO_ENCONTRADO", "mensaje": "No existe el pedido 999." }
            """;

        var respuesta = await _cliente.GetAsync("/api/pedidos/999/detalle");

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
        await AssertJsonIgualAsync(esperado, respuesta);
    }

    [Fact]
    public async Task GetPedidoPorId_Pedido8_SigueDevolviendoElPedidoSinLineas()
    {
        // CA-04: la ruta GET /api/pedidos/{id} no cambia con la llegada de /detalle.
        const string esperado = """
            {
              "id": 8,
              "folio": "PED-STGO-000008",
              "clienteId": "C-001",
              "clienteNombre": "Ferretería El Clavo",
              "fecha": "2026-10-20",
              "estado": "Entregado",
              "total": 120100,
              "sucursal": "STGO"
            }
            """;

        var respuesta = await _cliente.GetAsync("/api/pedidos/8");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        await AssertJsonIgualAsync(esperado, respuesta);
    }

    /// <summary>Compara el JSON completo (nombres, valores, tipos y orden de los arreglos), sin importar espacios.</summary>
    private static async Task AssertJsonIgualAsync(string esperado, HttpResponseMessage respuesta)
    {
        Assert.Equal("application/json", respuesta.Content.Headers.ContentType?.MediaType);

        var cuerpo = await respuesta.Content.ReadAsStringAsync();
        var esperadoJson = JsonNode.Parse(esperado);
        var realJson = JsonNode.Parse(cuerpo);

        Assert.True(
            JsonNode.DeepEquals(esperadoJson, realJson),
            $"El JSON no coincide.\nEsperado: {esperadoJson?.ToJsonString()}\nReal:     {realJson?.ToJsonString()}");
    }
}
