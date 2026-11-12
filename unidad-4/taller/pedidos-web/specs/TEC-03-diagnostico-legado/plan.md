# Plan: TEC-03 · Diagnóstico de Pedidos Legado

> Spec de origen: `specs/TEC-03-diagnostico-legado/spec.md` v1.0 · Autor: Claude Code · Revisado por: Claudio Ortiz Castillo
> Estado: aprobado (Claudio Ortiz Castillo, 2026-11-11), con el reparto de tiempos y la rama propuestos

> Formato: `specs/plantillas/plan-plantilla.md`. Responde **cómo** se hace el diagnóstico en 60 minutos; no repite la spec.
> Las correcciones del dev se dejan visibles con el formato `> **[Revisión del dev]** qué está mal, por qué y qué hacer en su lugar.`
> No se empieza hasta que la spec esté «aprobada» (v1.0) y este plan «aprobado» (regla 1 de `AGENTS.md`).

## Enfoque

El diagnóstico se hace en tres etapas encadenadas, cada una alimenta a la siguiente, después de una preparación que protege
el legado. La **preparación** (T1) se hace hoy, en cuanto apruebes la planificación: una regla `deny` sobre `legado/**` y el
subagente `analista-legado`, para que mañana a las 8:00 solo haya que abrir una sesión nueva. La **etapa 1** (T2) usa plan mode:
Claude lee todo `legado/`, propone un plan y, solo con tu aprobación, escribe `inventario.md` y `dependencias.md` (RD-01, RD-02).
La **etapa 2** (T3) delega en el subagente, que lee en su propio contexto y devuelve solo las tablas de `reglas-negocio.md` y
`riesgos.md` (RD-03, RD-04); tú abres el código y verificas. La **etapa 3** (T4) con el inventario, las reglas y los riesgos ya
escritos, Claude propone pesos y puntajes y tú decides (RD-05). El **cierre** (T5) comprueba que `legado/` no cambió.

## Reparto de los 60 minutos (mañana, 2026-11-12)

| Hora | Min | Tarea | Qué pasa | Si te atrasas |
|---|---|---|---|---|
| 8:00–8:05 | 5 | Arranque | Sesión nueva en `pedidos-web`, comprobar que `analista-legado` aparece y que la regla `deny` está | Si T1 no quedó hoy, se hace aquí y se recorta la etapa 1 |
| 8:05–8:20 | 15 | T2 · Etapa 1 | Plan mode: plan, aprobación, inventario y dependencias | **A las 8:20, si falta algo, plan B:** copiar `u4-respaldo/` y seguir |
| 8:20–8:45 | 25 | T3 · Etapa 2 | Subagente (~10 min), tu verificación en Rider de 5 reglas (~10 min), riesgos y revisión (~5 min) | Los mínimos no se bajan (8 reglas, 5 verificadas, 5 riesgos); sobra en el colchón de T4 |
| 8:45–8:55 | 10 | T4 · Etapa 3 | Pesos, puntajes por módulo (4 como mínimo), recomendación y resumen de 5 líneas | Se puntúan exactamente 4 módulos |
| 8:55–9:00 | 5 | T5 · Cierre | Comprobaciones de la spec, stage y mensaje de commit | El commit lo haces tú después de las 9:00 si hace falta |

La guía original calcula 80 minutos; el recorte sale de tener hecha la preparación y de no explorar el plan B salvo que haga falta.

## Decisiones técnicas

- **Regla `deny` en `.claude/settings.json` del proyecto** (no en el de usuario), para que viaje con el repositorio y proteja
  a cualquier persona del equipo. Propuesta inicial: `{"permissions":{"deny":["Edit(legado/**)"]}}`; en Claude Code, `Edit`
  cubre también a `Write`. Descartado: confiar solo en `AGENTS.md`, porque una instrucción no impide nada. **Límite conocido:**
  la regla no bloquea comandos de shell (`sed -i`, `echo >`); por eso se complementa con la regla 10 de `AGENTS.md`, con la
  verificación `git status` y con un subagente sin Bash. La sintaxis exacta se confirma en la documentación de permisos y con
  una prueba (T1), no se da por cierta.
- **Subagente `analista-legado` con solo `Read, Grep, Glob`.** Descartado: darle `Bash` o `Edit`, porque el diagnóstico no
  necesita ejecutar ni escribir, y una herramienta de menos es un riesgo de menos. Escribe él solo en su respuesta; las tablas
  las vuelco yo a los archivos después de que las revises.
- **Descripción del subagente:** «Lee el código de legado/ (Delphi 5/7 + base de datos) para encontrar reglas de negocio ocultas y
  riesgos, siempre con evidencia archivo:línea. Úsalo para preguntas sobre qué hace el sistema legado.» Se ajusta si Claude no lo
  elige solo.
- **Etapa 1 en plan mode (Shift+Tab hasta ver «plan mode on»).** Descartado: pedir el inventario directo, porque mirar sin
  escribir es justo el objetivo y la aprobación del plan es el punto de control E1.
- **Líneas con herramienta, no a ojo** (`wc -l legado/src/*`), y citas revisadas con un comando que compara `archivo:línea` con
  el largo real del archivo (CA-07).
- **Reglas verificadas por ti en Rider** (Ctrl+Shift+N y escribir `archivo:línea`), no por mí. Lo que no ubiques, sale del
  catálogo. Descartado: marcar «Sí» porque la IA «está segura».
- **Pesos y puntajes: Claude propone y explica; tú decides.** Descartado: que Claude fije los pesos solo, porque la
  recomendación pasaría a ser suya y no de la matriz.
- **Los commits los haces tú** (regla 7). T1 lleva su propio commit; T2, T3 y T4 se agrupan en el commit de cierre de la guía
  (`docs: diagnóstico de Pedidos Legado (U4)`) para ahorrar tiempo; si prefieres uno por etapa, se separa sin cambiar nada más.
- **Sin paquetes ni código nuevo.** Solo Markdown y JSON de configuración.

## Archivos afectados

| Archivo | Nuevo / Modificado | Tarea | Para qué |
|---|---|---|---|
| `.claude/settings.json` | Nuevo | T1 | Regla `deny` sobre `legado/**` |
| `.claude/agents/analista-legado.md` | Nuevo | T1 | Subagente de solo lectura |
| `docs/diagnostico/inventario.md` | Modificado (de plantilla) | T2 | Una fila por archivo y tabla |
| `docs/diagnostico/dependencias.md` | Modificado (de plantilla) | T2 | Diagrama Mermaid con la base |
| `docs/diagnostico/reglas-negocio.md` | Modificado (de plantilla) | T3 | 8 reglas o más con evidencia |
| `docs/diagnostico/riesgos.md` | Modificado (de plantilla) | T3 | 5 riesgos o más con evidencia |
| `docs/diagnostico/matriz-decision.md` | Modificado (de plantilla) | T4 | Pesos, matriz, recomendación y resumen |
| `specs/README.md` y `tasks.md` | Modificado | T0 y cada tarea | Fila de TEC-03 y estado de las tareas |

No se tocan: `legado/`, `src/`, `tests/`, `AGENTS.md`, `docs/plantilla-*.md` ni los documentos de TEC-01, TEC-02 y HU-02.

## Riesgos

| Riesgo | Mitigación |
|---|---|
| No alcanza el tiempo (60 min en vez de 80) | Preparación hecha hoy; reparto con hora de corte; plan B `u4-respaldo/` a las 8:20; los mínimos de la spec no se bajan, se recorta el resto. |
| Claude quiere «arreglar» el legado | Regla `deny`, subagente sin Edit, regla 10 de `AGENTS.md`; se rechaza la propuesta y se comprueba con `git status -- legado`. |
| La regla `deny` no bloquea (sintaxis mal) o no cubre el shell | T1 la prueba de verdad y confirma la sintaxis en la documentación; el límite del shell se cubre con `git status` en T5. |
| El subagente no aparece | T1 se hace hoy; mañana se abre una sesión nueva (la guía pide cerrar y reabrir Claude Code la primera vez). |
| Reglas inventadas o tomadas de comentarios que mienten | Evidencia `archivo:línea` obligatoria; 5 reglas verificadas por ti; contradicciones comentario vs. código anotadas. |
| Una cita apunta a una línea que no calza | Comprobación con comando (CA-07); lo que no se ubica, sale del catálogo. |
| Los `.pas` están en Windows-1252 y los acentos se ven mal al leerlos | Cuenta de líneas y evidencia no se afectan; se anota si pasa y no se «corrige» el archivo. |
| Pesos o puntajes salen de la intuición | Cada puntaje se explica; las cuentas se revisan; la recomendación debe coincidir con el mayor ponderado (CA-08). |
| El resumen queda con jerga | Se redacta para gerencia y se revisa contra CA-09 antes de cerrar. |

## Pasos

0. Aprobar la planificación (spec v1.0 y este plan) y abrir `feature/u4-taller-4` desde la rama del paso 0; la integración a `develop` la hace Claudio al final. → documentos en «aprobada» y «aprobado».
1. T1, protección y subagente. → regla `deny` probada y `analista-legado` creado (hoy).
2. T2, etapa 1: inventario y dependencias. → 16 archivos, `.dpr` y 6 tablas; diagrama con la base (8:05).
3. T3, etapa 2: reglas y riesgos. → 8 o más reglas (5 verificadas) y 5 o más riesgos con evidencia (8:20).
4. T4, etapa 3: matriz y resumen. → pesos que suman 100 %, 4 módulos, recomendación y 5 líneas (8:45).
5. T5, cierre. → CA-10 comprobado y commit listo para que lo hagas (8:55).

El detalle de cada paso está en `tasks.md`.

## Fuera del plan

- Tocar o ejecutar el legado, y cualquier cambio en `src/` o `tests/`.
- El plan de migración completo y la migración de totales (Unidad 5).
- Plugins o herramientas nuevas fuera de las que la guía usa.
