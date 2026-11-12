using System.Net;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PedidosWeb.Api.Tests.Pedidos;

/// <summary>Tests HTTP de GET /api/pedidos/buscar (HU-01, spec v1.1: CA-21 y CA-22).</summary>
public class BuscarPedidosEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _cliente;

    public BuscarPedidosEndpointTests(WebApplicationFactory<Program> fabrica)
    {
        _cliente = fabrica.CreateClient();
    }

    [Fact]
    public async Task GetBuscar_ClienteExistente_Devuelve200ConPaginaJson()
    {
        // CA-21: el cuerpo debe ser exactamente el Ejemplo 1 de la spec.
        const string ejemplo1 = """
            {
              "pedidos": [
                { "id": 15, "folio": "PED-VAL-000015", "clienteId": "C-004", "clienteNombre": "Almacén La Esquina", "fecha": "2026-09-09", "estado": "Entregado",  "total": 23750,  "sucursal": "VAL" },
                { "id": 7,  "folio": "PED-VAL-000007", "clienteId": "C-004", "clienteNombre": "Almacén La Esquina", "fecha": "2026-09-21", "estado": "Confirmado", "total": 33150,  "sucursal": "VAL" },
                { "id": 39, "folio": "PED-VAL-000039", "clienteId": "C-004", "clienteNombre": "Almacén La Esquina", "fecha": "2026-10-04", "estado": "Despachado", "total": 113050, "sucursal": "VAL" },
                { "id": 31, "folio": "PED-VAL-000031", "clienteId": "C-004", "clienteNombre": "Almacén La Esquina", "fecha": "2026-10-16", "estado": "Entregado",  "total": 122450, "sucursal": "VAL" },
                { "id": 23, "folio": "PED-VAL-000023", "clienteId": "C-004", "clienteNombre": "Almacén La Esquina", "fecha": "2026-10-28", "estado": "Confirmado", "total": 14350,  "sucursal": "VAL" }
              ],
              "total": 5,
              "pagina": 1,
              "tamanoPagina": 10
            }
            """;

        var respuesta = await _cliente.GetAsync("/api/pedidos/buscar?clienteId=C-004");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        await AssertJsonIgualAsync(ejemplo1, respuesta);
    }

    [Fact]
    public async Task GetBuscar_SinFiltros_Devuelve400ConCuerpoDeError()
    {
        // CA-22: el cuerpo debe ser exactamente el Ejemplo 3 de la spec.
        const string ejemplo3 = """
            { "codigo": "FILTRO_REQUERIDO", "mensaje": "Debes indicar al menos un filtro: clienteId, estado, sucursal, desde o hasta." }
            """;

        var respuesta = await _cliente.GetAsync("/api/pedidos/buscar");

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        await AssertJsonIgualAsync(ejemplo3, respuesta);
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
