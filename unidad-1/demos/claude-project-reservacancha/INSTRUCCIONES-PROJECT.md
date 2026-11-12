# Project de Claude web · ReservaCancha

Este Project se usa en la parte 2 de la demo «Chat, Project o agente» (Unidad 1, lámina 17). Tiene **solo el README** del repo: así el grupo ve que el Project conoce las reglas, pero no el código ni los tests.

## Cómo crearlo (5 minutos)

1. Entra a claude.ai con tu cuenta (Pro o Team) y abre **Projects → Create project**.
2. **Nombre:** `ReservaCancha`
3. **Descripción:** `Reservas de canchas del recinto La Pelota (demo del curso).`
4. **Instrucciones del Project** (copia el bloque de abajo en el campo de instrucciones personalizadas).
5. **Conocimiento:** sube **solo** el archivo `reservacancha/README.md`. No subas el código ni los tests.
6. Abre un chat dentro del Project y prueba que responde: *«¿Cuál es el horario del recinto el sábado?»*. Debe contestar de 09:00 a 22:00.

## Instrucciones del Project (copiar y pegar)

```
Eres un desarrollador del equipo de ReservaCancha. El stack es .NET 10, C# y xUnit. Las reglas del negocio están en el README del proyecto: úsalas y no inventes reglas que no estén ahí. Responde en español de Chile.
```

## Qué esperar en la demo

| Superficie | Qué suele pasar |
|---|---|
| Chat sin contexto | Inventa el modelo de datos, el lenguaje o el horario (por ejemplo, un horario fijo de 9 a 23 h para todos los días). |
| Este Project | Usa el horario del README y los mensajes de error, pero escribe código que nunca compiló ni probó, sobre clases que supone. |
| Claude Code en el repo | Lee `ReservaService.cs`, encuentra el comentario «R6 y R7: pendientes», edita el archivo, agrega tests y ejecuta `dotnet test`. |

Es un comportamiento probable, no garantizado: ensaya la demo antes de la clase y guarda capturas.
