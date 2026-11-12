# Spec: TEC-03 · Diagnóstico de Pedidos Legado

> Versión: 1.0 · Estado: implementada (aprobada por Claudio Ortiz Castillo, 2026-11-11) · Intent de origen: `specs/TEC-03-diagnostico-legado/intent.md`
> Autor del borrador: Claude Code · Aprobada por: Claudio Ortiz Castillo (regla 1 de `AGENTS.md`)

> Formato: adaptado de `specs/plantillas/spec-plantilla-comentada.md` y de la spec de TEC-02. Aquí la spec dice **qué debe
> cumplir** cada documento del diagnóstico y cómo se comprueba. Las decisiones marcadas con **[Decidir]** son tuyas.

## Qué se entrega

Cinco documentos en `docs/diagnostico/` (las plantillas ya existen y se **reemplazan** llenándolas), más dos archivos de
preparación en `.claude/`:

| Archivo | Etapa | Quién lo llena |
|---|---|---|
| `.claude/settings.json` (regla `deny` sobre `legado/**`) | Preparación | Claude, con tu aprobación |
| `.claude/agents/analista-legado.md` (subagente de solo lectura) | Preparación | Claude, desde `docs/plantilla-subagente.md` |
| `docs/diagnostico/inventario.md` | 1 · Entender | Claude en plan mode; tú apruebas el plan |
| `docs/diagnostico/dependencias.md` | 1 · Entender | Claude en plan mode; tú apruebas el plan |
| `docs/diagnostico/reglas-negocio.md` | 2 · Encontrar | Subagente `analista-legado`; **tú verificas** |
| `docs/diagnostico/riesgos.md` | 2 · Encontrar | Subagente `analista-legado`; tú revisas |
| `docs/diagnostico/matriz-decision.md` | 3 · Decidir | Claude propone pesos y puntajes; **tú decides** |

## Reglas del diagnóstico

- **RG-01 · `legado/` no se toca.** Ni un archivo se edita, se mueve ni se borra. Lo garantiza la regla `deny` y la regla 10
  de `AGENTS.md`.
- **RG-02 · Evidencia obligatoria.** Toda regla de negocio y todo riesgo cita `archivo:línea`, y esa línea debe existir en el
  archivo. Sin evidencia no entra al catálogo; lo dudoso se marca «por verificar».
- **RG-03 · El código manda, no los comentarios.** Si un comentario o un README contradice al código, vale el código y la
  contradicción se anota (por ejemplo, Delphi 5 + Paradox/BDE en `legado/README.md` frente a Delphi 7 + DBISAM 4 en el README del proyecto).
- **RG-04 · «Verificada = Sí» solo si una persona la comprobó.** Significa que Claudio abrió el archivo en esa línea y leyó el
  código. Una regla que solo vio la IA queda en «por verificar».
- **RG-05 · Sin números a ojo.** Las líneas por archivo se cuentan con una herramienta (`wc -l` o equivalente) y las cuentas de la
  matriz las hace Claude y las revisa Claudio.
- **RG-06 · Español de Chile y sin jerga en el resumen.** El resumen ejecutivo es para la gerencia.

## Qué debe tener cada documento

- **RD-01 · `inventario.md`.** Una fila por cada archivo de `legado/src/` (los 10 `.pas` y los 6 `.dfm`), por
  `legado/PedidosLegado.dpr` y por cada tabla de `legado/db/esquema-dbisam.sql` (6 tablas), con tipo, responsabilidad en una
  frase y líneas contadas. Más 2 o 3 observaciones en «Lo que me llamó la atención».
- **RD-02 · `dependencias.md`.** Un diagrama Mermaid válido (`flowchart`) donde cada flecha `A --> B` significa «A usa a B»
  (cláusula `uses` o consulta SQL) e **incluye la base de datos** como nodo. Responde las dos preguntas guía: qué unidad usan
  casi todas y cuál no depende de formularios ni de la base.
- **RD-03 · `reglas-negocio.md`.** Al menos **8** reglas, cada una con condición, efecto, `archivo:línea` y la columna
  «Verificada». Si una regla vive en más de un lugar, se anotan todos.
- **RD-04 · `riesgos.md`.** Al menos **5** riesgos, cada uno con evidencia `archivo:línea`, categoría (Seguridad, Datos e
  integridad, Fechas o Mantenibilidad) y prioridad (Alta, Media o Baja).
- **RD-05 · `matriz-decision.md`.** Pesos de los 5 criterios que **suman 100 %**; al menos **4 módulos** puntuados contra las 4
  estrategias (Mantener, Refactorizar, Modernizar, Reemplazar); una recomendación por módulo que se desprende del puntaje
  ponderado; y un **resumen ejecutivo de exactamente 5 líneas** (qué es y su estado, hallazgo principal con evidencia, riesgo más
  urgente, estrategia y por dónde empezar, próximo paso que se pide aprobar).
- **RD-06 · Subagente.** `analista-legado` con `tools: Read, Grep, Glob` y nada más; exige evidencia `archivo:línea`, dice «por
  verificar» cuando no la encuentra y nunca propone ni hace cambios al código.
- **RD-07 · Regla `deny`.** `.claude/settings.json` impide editar `legado/**` y conserva lo que ya tenga (hoy no existe).

## Criterios de aceptación

- **CA-01 · Legado protegido.** Dado el proyecto con la regla `deny`, cuando se le pide a Claude editar un archivo de
  `legado/`, entonces la herramienta lo bloquea y `git status --short -- legado` queda vacío.
- **CA-02 · Subagente listo.** Dado `.claude/agents/analista-legado.md`, entonces su cabecera tiene `name`, `description` y
  `tools: Read, Grep, Glob`, y el subagente aparece en una sesión nueva.
- **CA-03 · Inventario completo.** Dado `inventario.md`, entonces tiene tantas filas como archivos hay en `legado/src/` (16),
  más el `.dpr` y las 6 tablas, y ninguna fila tiene la responsabilidad ni las líneas en blanco.
- **CA-04 · Mapa con la base.** Dado `dependencias.md`, entonces el bloque Mermaid se dibuja en la vista previa del editor,
  contiene un nodo de base de datos y las dos preguntas guía están respondidas.
- **CA-05 · Reglas con evidencia.** Dado `reglas-negocio.md`, entonces hay 8 reglas o más, todas con `archivo:línea`, y al menos
  5 con «Sí» en Verificada por Claudio. Las reglas cuya línea no calza salen del catálogo.
- **CA-06 · Riesgos con evidencia.** Dado `riesgos.md`, entonces hay 5 riesgos o más, cada uno con evidencia, categoría y prioridad.
- **CA-07 · Citas reales.** Dado cualquier `archivo:línea` de las reglas y los riesgos, entonces el archivo existe y esa línea
  existe (no supera el largo del archivo); esto se revisa con un comando, no a ojo.
- **CA-08 · Matriz que cuadra.** Dado `matriz-decision.md`, entonces los pesos suman 100 %, hay 4 módulos o más puntuados y cada
  recomendación coincide con el mayor puntaje ponderado de su fila (o se explica por qué no).
- **CA-09 · Resumen para la gerencia.** Dado el resumen ejecutivo, entonces tiene exactamente 5 líneas numeradas, sin jerga
  técnica sin explicar, y la recomendación se apoya en la matriz.
- **CA-10 · Sin cambios de código.** Dado el trabajo terminado, entonces `git status --short -- legado src tests` no muestra
  cambios y `dotnet test` sigue en 83 en verde.

## Decisiones abiertas

- **[Decidir] Pesos de los criterios (RD-05):** los propone Claude en la etapa 3 con su explicación; los decides tú. Mañana a
  las 8:45, no hoy, porque dependen de lo que se encuentre en las etapas 1 y 2.
- **[Decidir] Qué módulos puntuar (RD-05):** salen del inventario; candidatos probables son cálculo de totales, stock, clientes,
  productos e informes. Se confirman en la etapa 3.
- **Decidido por Claudio · Tiempos:** los 60 minutos se reparten 5 + 15 + 25 + 10 + 5, tal como está en el plan.
- **Decidido por Claudio · Rama:** el taller se trabaja en `feature/u4-taller-4`, abierta desde la rama del paso 0 (que ya
  incluye el commit de U3); la integración a `develop` la hace Claudio al final.
- **Verificar en T1:** la sintaxis exacta de la regla `deny` (`Edit(legado/**)` y su ruta relativa) se confirma en la
  documentación oficial de permisos y con una prueba real; no se asume.

## Fuera de alcance

- Migrar, corregir o comentar código de `legado/`; instalar o ejecutar el sistema Delphi.
- Tocar `src/`, `tests/`, `AGENTS.md` o las plantillas de `docs/`.
- El plan de migración completo y la migración del cálculo de totales (Unidad 5).
- Instalar plugins nuevos (el plugin `code-modernization` es una extensión opcional y no se usa).

## Historial

| Versión | Fecha | Cambio | Quién |
|---|---|---|---|
| 0.1 | 2026-11-11 | Borrador inicial a partir del intent y de la guía del taller 4 | Claude Code |
| 1.0 | 2026-11-11 | Spec aprobada; se aceptan el reparto de tiempos y la rama; pesos y módulos se deciden en T4 | Claudio Ortiz Castillo |
