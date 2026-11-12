namespace PedidosWeb.Core.Clientes;

// Servicio heredado: lo escribió un ex integrante del equipo, con apuro.
// Se ordenó en TEC-01 (nombres claros, búsqueda única, condiciones simples) sin cambiar su comportamiento,
// que quedó fijado por 14 tests en ClientesServiceTests y ClientesServiceCaracterizacionTests.
public class ClientesService
{
    private readonly List<Cliente> _clientes;

    public ClientesService() : this(ClientesDeEjemplo.Crear()) { }

    public ClientesService(List<Cliente> clientes)
    {
        _clientes = clientes;
    }

    // ficha del cliente para la pantalla
    public string ObtenerFicha(string id)
    {
        var cliente = BuscarPorId(id);
        if (cliente == null) return "";
        string rut = cliente.Rut.Replace(".", "").Replace(" ", "").ToUpper();
        string zona = cliente.Region switch
        {
            "RM" or "V" => "Zona centro",
            "VIII" => "Zona sur",
            "II" => "Zona norte",
            _ => "Sin zona",
        };
        string estado = cliente.Activo ? "Activo" : "Inactivo";
        return cliente.Nombre + " | " + rut + " | " + zona + " | " + estado;
    }

    // si existe y está activo
    public bool ExisteYEstaActivo(string id)
    {
        return BuscarPorId(id) is { Activo: true };
    }

    // el primer cliente con ese id (se compara exacto), o null si no hay
    private Cliente? BuscarPorId(string id) => _clientes.FirstOrDefault(cliente => cliente.Id == id);
}
