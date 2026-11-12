# Intent: TEC-05 · Calidad, pruebas y CI/CD

> Autor: Claudio Ortiz Castillo (PO) · Fecha: 2026-11-11 · Estado: **borrador** (pendiente de aprobación)

> Formato: `specs/plantillas/intent-plantilla.md`. Trabajo técnico (prefijo `TEC`): el vendedor no ve ningún cambio.
> Es el taller de la Unidad 6: «Que ningún cambio llegue a `main` sin pruebas» (80 minutos, individual).

## Qué queremos

Que ningún cambio llegue a `develop` sin pasar por pruebas automáticas, **todo en local**. Tres cosas: (1) tests de borde que de verdad pueden fallar,
(2) un CI local en Docker (build y test en un contenedor) que, si pasa, despliega la API en Docker, y
(3) una falla real, provocada a mano, diagnosticada por Claude (solo lectura), corregida por una persona y con el CI local en verde.
Actualización 2026-11-12: Claudio recibió autorización para usar GitHub y creó el repo; vuelven `ci.yml`, `claude.yml` y el PR con `@claude`, y el Docker local se conserva. Nada se instala.

## Por qué lo queremos

La U5 dejó una red (60 casos del golden master), pero corre solo si alguien se acuerda de ejecutar `dotnet test`.
Un cambio que rompe el cálculo de totales un viernes no debe llegar al lunes sin que nadie lo note. El CI convierte la red en un requisito.

## Para quién

El equipo de desarrollo (y quien revisa PR). El vendedor no ve nada nuevo.

## Reglas que ya conocemos

- **Claude diagnostica; tú corriges y haces el merge con el CI local en verde.**
- **Las fallas se rompen a mano en Rider**, nunca pidiéndoselas a Claude (el hook de la U5 le devolvería el error y Claude lo «arreglaría»).
- **El token nunca va en un archivo ni en el repo.** Va como secreto del repo `CLAUDE_CODE_OAUTH_TOKEN`; los pasos los hace Claudio. Tampoco hay secretos en la imagen Docker.
- **Un test que no puede fallar no prueba nada:** cada test nuevo se rompe a mano una vez; si no falla, se borra.
- **Lo que no está en la spec se pregunta** (regla 11): los tests de borde salen de reglas escritas (RG-01…RG-08), no de reglas inventadas.
- **H1 se corrige aquí, por decisión de Claudio:** los umbrales de descuento (500.000 y 1.000.000) pasan a la regla del grabado (`>=`). No hay contabilidad en un taller de capacitación ni documento oficial en el repo; la decisión queda registrada como del PO. Va en un commit aparte, como R05.

## Restricciones

- **Plazo:** 80 minutos (preparación 5, tests 20, pipeline 25, falla 25, cierre 5).
- **Sin paquetes NuGet nuevos** (regla 8) y **sin instalar software**: solo Docker, que ya está instalado (hay que abrir Docker Desktop).
- **Commits locales; push, PR y merge los hace Claudio** (salvo que diga lo contrario). La integración a `develop` también.
- **Plan B:** `unidad-6/taller/u6-respaldo/` (`claude.yml` listo) y, sin App o token, diagnosticar el log local del CI en Docker con Claude Code.

## Cómo sabremos que funcionó

`ci.yml` en verde en Actions, `claude.yml` en `main`, `scripts/ci-local.ps1` en verde con la API respondiendo en `http://localhost:5080/salud`, la compuerta cerrada cuando un test falla,
tests de borde revisados (cada uno falló una vez al romper el código), una falla diagnosticada y corregida, y `docs/calidad/registro-u6.md` completo.

## Lo que NO es

- No es migrar otro módulo ni tocar `legado/` (solo lectura): H1 se corrige solo en el C#.
- No es que Claude corrija la falla ni haga merge.
- No es un despliegue en la nube: el «despliegue» es la API corriendo en un contenedor local. GitHub se usa para el CI y para `@claude`.
