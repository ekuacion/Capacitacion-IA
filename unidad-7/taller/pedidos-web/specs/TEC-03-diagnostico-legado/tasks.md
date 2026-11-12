# Tasks: TEC-03 · Diagnóstico de Pedidos Legado

> Plan de origen: `specs/TEC-03-diagnostico-legado/plan.md` (aprobado) · Spec: `specs/TEC-03-diagnostico-legado/spec.md` v1.0 (aprobada)

> Formato: `specs/plantillas/tasks-plantilla.md`. Una tarea = una etapa con su verificación. Aquí no hay código: todo son
> archivos Markdown y un JSON de configuración. `legado/`, `src/` y `tests/` no se tocan en ninguna tarea.
> Las horas son las de mañana, 2026-11-12, y salen del reparto del plan.

## Resumen

| ID | Tarea | Hora | CA cubiertos | Estado |
|---|---|---|---|---|
| T0 | Aprobar spec v1.0 y plan; abrir la rama del taller | Hoy | — | hecha |
| T1 | Proteger `legado/` y crear el subagente `analista-legado` | Hoy | CA-01, CA-02 | hecha (regla probada; el subagente se confirma en la sesión nueva de mañana) |
| T2 | Etapa 1 · Entender: inventario y dependencias | 8:05–8:20 | CA-03, CA-04 | hecha |
| T3 | Etapa 2 · Encontrar: reglas de negocio y riesgos | 8:20–8:45 | CA-05, CA-06, CA-07 | hecha (15 reglas, 5 verificadas por Claudio; 13 riesgos; 130 líneas citadas comprobadas con `wc -l`) |
| T4 | Etapa 3 · Decidir: matriz de decisión y resumen | 8:45–8:55 | CA-08, CA-09 | hecha (pesos 15/15/35/25/10 = 100 %; 5 módulos; resumen de 5 líneas; aritmética confirmada por Claudio) |
| T5 | Cierre: comprobar que el legado no cambió y dejar el commit listo | 8:55–9:00 | CA-10 | hecha (`git status -- legado src tests` vacío; `dotnet test` 83 en verde) |

Regla de oro de todas las tareas: **`legado/` se lee, no se toca.** Si Claude propone «arreglar» algo, se rechaza.

---

## T0 — Aprobar la planificación y abrir la rama

- **Depende de:** ninguna.
- **Archivos:** `specs/TEC-03-diagnostico-legado/spec.md`, `plan.md`, `specs/README.md`.
- **Qué hacer:**
  - Claudio lee `intent.md`, `spec.md`, `plan.md` y `tasks.md`, pide los cambios que quiera y decide los puntos **[Decidir]** abiertos.
  - Claudio deja la spec en «aprobada» v1.0 y el plan en «aprobado».
  - Crear `feature/u4-taller-4` desde `feature/u4-paso-0-traer-taller-3` (que ya incluye el commit de U3). Integrar a `develop` lo hace Claudio al final.
- **Verificación:** `grep -n "Estado" specs/TEC-03-diagnostico-legado/spec.md specs/TEC-03-diagnostico-legado/plan.md` → «aprobada» y «aprobado»; `git branch --show-current` → `feature/u4-taller-4`.
- **Hecho cuando:** los documentos están aprobados por una persona y la rama de trabajo existe.
- **Commit:** `docs(u4): planificación y aprobación de TEC-03 (diagnóstico de Pedidos Legado)`

## T1 — Proteger `legado/` y crear el subagente `analista-legado`

- **CA cubiertos:** CA-01, CA-02.
- **Depende de:** T0.
- **Archivos:** `.claude/settings.json` (nuevo), `.claude/agents/analista-legado.md` (nuevo).
- **No tocar:** `legado/`, `src/`, `tests/`.
- **Qué hacer:**
  - Confirmar la sintaxis de la regla `deny` en la documentación de permisos (`code.claude.com/docs/en/permissions`) y crear
    `.claude/settings.json` con la regla que impide editar `legado/**`. Hoy el archivo no existe, así que no hay nada que conservar.
  - Copiar el bloque de `docs/plantilla-subagente.md` a `.claude/agents/analista-legado.md`, completar los huecos (qué busca:
    reglas de negocio ocultas y riesgos; evidencia `archivo:línea`; «por verificar» si no hay evidencia; nunca propone ni hace cambios;
    respuesta en español de Chile) y dejar `tools: Read, Grep, Glob`.
  - **Probar la regla:** pedirle a Claude que agregue una línea de prueba a un archivo de `legado/src/`; debe ser bloqueado.
  - Cerrar y volver a abrir Claude Code para que reconozca el subagente (es el primero del proyecto).
- **Verificación:**
  - `cat .claude/settings.json` muestra la regla sobre `legado/**`; la prueba de edición es bloqueada y `git status --short -- legado` → vacío.
  - `head -6 .claude/agents/analista-legado.md` muestra `name: analista-legado` y `tools: Read, Grep, Glob`; `/agents` lo lista.
- **Hecho cuando:** `legado/` está protegido (probado) y el subagente aparece en una sesión nueva.
- **Commit:** `chore(u4): regla deny de legado y subagente analista-legado (T1)`

## T2 — Etapa 1 · Entender: inventario y dependencias

- **CA cubiertos:** CA-03, CA-04.
- **Depende de:** T1.
- **Hora:** 8:05–8:20 (15 min). **Corte:** si a las 8:20 falta algo, plan B: pedirle a Claude que copie `inventario.md` y
  `dependencias.md` desde `unidad-4/taller/u4-respaldo/docs/diagnostico/` y seguir en T3.
- **Archivos:** `docs/diagnostico/inventario.md`, `docs/diagnostico/dependencias.md`.
- **No tocar:** `legado/`, `src/`, `tests/`.
- **Qué hacer:**
  - Activar **plan mode** (Shift+Tab hasta ver «plan mode on») y pedir: *«Explora legado/ y propónme un plan para llenar
    docs/diagnostico/inventario.md (cada .pas, .dfm y tabla, con su responsabilidad y líneas) y dependencias.md (diagrama
    Mermaid, incluida la base).»*
  - Revisar el plan y **aprobarlo**; Claude sale de plan mode y escribe los dos archivos.
  - En el inventario: los 16 archivos de `legado/src/`, el `.dpr` y las 6 tablas, con las líneas contadas por herramienta.
    Anotar en «Lo que me llamó la atención» el desajuste entre `legado/README.md` y el README del proyecto (Delphi 5 + Paradox
    frente a Delphi 7 + DBISAM) si el código lo confirma.
  - En las dependencias: `flowchart` con la base de datos como nodo y flechas «A usa a B» sacadas de las cláusulas `uses` y las
    consultas; responder las dos preguntas guía.
- **Verificación:**
  - `ls legado/src | wc -l` → 16, y `inventario.md` tiene una fila por cada uno (más `.dpr` y 6 tablas); ninguna celda vacía.
  - `grep -n "\[(" docs/diagnostico/dependencias.md` muestra al menos un nodo de base de datos (forma `[(…)]`); el bloque
    Mermaid se dibuja en la vista previa del editor.
  - Preguntas de revisión: ¿tantos `.pas` como en `legado/src/`? ¿incluye la base? ¿qué unidad no depende de nada?
- **Hecho cuando:** inventario con todos los archivos y diagrama con la base, ambos revisados por Claudio.
- **Commit:** va en el commit de cierre (T5).

## T3 — Etapa 2 · Encontrar: reglas de negocio y riesgos

- **CA cubiertos:** CA-05, CA-06, CA-07.
- **Depende de:** T2.
- **Hora:** 8:20–8:45 (25 min): subagente ~10 min, tu verificación ~10 min, riesgos y revisión ~5 min. **Sin plan mode.**
- **Archivos:** `docs/diagnostico/reglas-negocio.md`, `docs/diagnostico/riesgos.md`.
- **No tocar:** `legado/`, `src/`, `tests/`.
- **Qué hacer:**
  - Pedir: *«Usa el subagente analista-legado para llenar docs/diagnostico/reglas-negocio.md y riesgos.md según sus plantillas.
    Cada fila con archivo:línea; lo dudoso, «por verificar». No te fíes de los comentarios.»*
  - **Pistas, no respuestas:** las reglas no viven en un solo archivo; hay código que nunca se ejecuta; una regla puede estar dos
    veces y comportarse distinto.
  - **Claudio verifica 5 reglas en Rider** (Ctrl+Shift+N, escribir `archivo:línea`) y marca «Sí» en Verificada. La que no se ubique, sale.
  - Revisar cada riesgo: evidencia, categoría (Seguridad, Datos e integridad, Fechas, Mantenibilidad) y prioridad.
- **Verificación:**
  - `grep -c "^| [0-9]" docs/diagnostico/reglas-negocio.md` → 8 o más; `grep -c "| Sí |" docs/diagnostico/reglas-negocio.md` → 5 o más.
  - `grep -c "^| RS-" docs/diagnostico/riesgos.md` → 5 o más, cada uno con evidencia y prioridad.
  - **Citas reales (CA-07):** para cada `archivo:línea`, `wc -l legado/src/<archivo>` es mayor o igual que la línea citada; Claude lo comprueba con un comando.
- **Hecho cuando:** 8 reglas o más (5 verificadas por ti) y 5 riesgos o más, todos con evidencia y prioridad, sin citas que no calcen.
- **Commit:** va en el commit de cierre (T5).

## T4 — Etapa 3 · Decidir: matriz de decisión y resumen

- **CA cubiertos:** CA-08, CA-09.
- **Depende de:** T3.
- **Hora:** 8:45–8:55 (10 min). **Sin plan mode.**
- **Archivos:** `docs/diagnostico/matriz-decision.md`.
- **No tocar:** `legado/`, `src/`, `tests/`.
- **Qué hacer:**
  - Pedir: *«Con mi inventario, reglas y riesgos, ayúdame a llenar docs/diagnostico/matriz-decision.md. Propón pesos y
    puntajes por módulo y explícame cada uno: yo decido. Cierra con un resumen de 5 líneas.»*
  - Claudio **decide los pesos** de los 5 criterios (deben sumar 100 %) y **los módulos** a puntuar (4 como mínimo).
  - Claude calcula el puntaje ponderado de cada estrategia por módulo; Claudio revisa las cuentas de al menos un módulo a mano.
  - La recomendación de cada módulo sale del mayor puntaje ponderado. Si se aparta, se explica por qué.
  - Redactar el resumen ejecutivo de **5 líneas**, para la gerencia y sin jerga.
- **Verificación:**
  - La columna de pesos suma 100 %; la matriz tiene 4 módulos o más; cada recomendación coincide con el mayor puntaje de su fila.
  - `grep -c "^[1-5]\. \*\*" docs/diagnostico/matriz-decision.md` → 5 (las cinco líneas del resumen).
  - Preguntas de revisión: ¿los pesos suman 100 %? ¿la recomendación sale de los puntajes o de la intuición?
- **Hecho cuando:** pesos que suman 100 %, 4 módulos puntuados, una recomendación que se desprende de la matriz y el resumen de 5 líneas.
- **Commit:** va en el commit de cierre (T5).

## T5 — Cierre: comprobar que el legado no cambió y dejar el commit listo

- **CA cubiertos:** CA-10 (y repaso de CA-01 a CA-09).
- **Depende de:** T4.
- **Hora:** 8:55–9:00 (5 min).
- **Archivos:** `specs/TEC-03-diagnostico-legado/tasks.md` y `spec.md` (estado), todos los de `docs/diagnostico/`.
- **No tocar:** `legado/`, `src/`, `tests/`.
- **Qué hacer:**
  - Comprobar que los 5 archivos están en `docs/diagnostico/` y que `legado/` no cambió.
  - Ejecutar `dotnet test` para confirmar que nada se rompió.
  - Marcar las tareas como hechas y la spec como «implementada»; dejar todo en stage con el mensaje sugerido. **El commit lo haces tú**
    (regla 7): `docs: diagnóstico de Pedidos Legado (U4)`.
- **Verificación:**
  - `git status --short -- legado src tests` → vacío.
  - `dotnet test` → 83 en verde (78 de Core y 5 de Api).
  - `ls docs/diagnostico` → `inventario.md`, `dependencias.md`, `reglas-negocio.md`, `riesgos.md`, `matriz-decision.md`.
- **Hecho cuando:** los 5 archivos están completos, `legado/` no tiene cambios y el commit queda listo para que lo hagas.
- **Commit:** `docs: diagnóstico de Pedidos Legado (U4)` (lo haces tú; incluye T2, T3 y T4).

## Mañana, antes de las 8:00

- Cerrar y reabrir Claude Code en `pedidos-web` para empezar con sesión nueva (así el subagente queda reconocido).
- Tener a mano Rider con la solución abierta y la vista de archivos activada para ver `legado/`.
- Si T1 no quedó hecha hoy, se hace al arrancar y se recorta la etapa 1.
