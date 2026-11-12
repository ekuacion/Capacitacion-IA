# Demo SDD: carrito con descuentos en TiendaMini

Material de clase sobre **Spec-Driven Development (SDD)** para desarrolladores web junior.
TiendaMini es una tienda online ficticia. La funcionalidad es un carrito con descuentos.
Stack de referencia: API REST en ASP.NET Core (.NET 10) + xUnit. Aquí no hay código: solo documentos.

## Idea central
Un agente (o una persona) programa mejor cuando recibe una spec clara en vez de un prompt suelto.
La spec es el contrato: de ella salen el plan, las tareas y los tests.

## Orden de lectura
1. `ejemplo-carrito/0-prompt-suelto.md`: el problema. Todo lo que un agente tendría que inventar.
2. `ejemplo-carrito/1-intent.md`: qué quiere el PO y por qué.
3. `ejemplo-carrito/2-spec.md`: reglas, criterios de aceptación y ejemplos.
4. `ejemplo-carrito/3-plan.md`: plan del agente, corregido por el dev.
5. `ejemplo-carrito/4-tasks.md`: tareas pequeñas y verificables.
6. `ejemplo-carrito/5-trazabilidad.md`: criterio → tarea → test.
7. `plantillas/`: para usar en sus propios proyectos. Parte por `spec-plantilla-comentada.md`.

## Cómo usarla en clase (90 min)
- **10 min**: lee en voz alta el prompt suelto. Pide al curso que adivine las 6 decisiones ocultas.
- **15 min**: recorre intent y spec. Muestra cómo cada pregunta del paso 1 quedó resuelta.
- **15 min**: revisa el plan. Pregunta qué está mal antes de mostrar los comentarios del dev.
- **10 min**: tareas y trazabilidad. Elige un criterio y síguelo hasta su test.
- **40 min**: en parejas, escriben una spec para otra funcionalidad con la plantilla comentada.

`AGENTS.md` muestra cómo darle al agente las reglas del proyecto antes de pedirle algo.
