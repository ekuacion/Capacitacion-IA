# AGENTS.md · TodoMini

## Stack y comandos

- .NET 10, C#, xUnit.
- Compilar: `dotnet build` · Tests: `dotnet test`. Ejecuta los tests después de cada cambio.

## Reglas

1. Responde y comenta el código en español de Chile.
2. La fecha y la hora se obtienen siempre del `TimeProvider` inyectado. Nunca `DateTime.Now`.
3. Cada cambio de comportamiento lleva sus tests xUnit. Nombres: `Metodo_Escenario_ResultadoEsperado`.
4. No agregues paquetes NuGet sin preguntar.
5. Nunca hagas commit ni push: eso lo hace una persona.
