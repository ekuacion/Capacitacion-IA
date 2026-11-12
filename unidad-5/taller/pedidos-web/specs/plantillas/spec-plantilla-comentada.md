# Spec: [nombre de la funcionalidad]

> **Cómo usar esta plantilla**
> Cada sección trae una guía en un comentario `<!-- -->`: qué va, por qué importa,
> un ejemplo bueno, un ejemplo malo y un error frecuente.
> Los comentarios no se ven al renderizar el markdown. Bórralos cuando la spec esté lista.
> Regla de oro: **si un dev o un agente tiene que preguntarte algo para escribir un test, falta información en la spec.**

| Campo | Valor |
|---|---|
| Estado | [borrador \| en revisión \| aprobada \| implementada] |
| Versión | [1.0] |
| Fecha | [AAAA-MM-DD] |
| Autor(es) | [nombres y roles] |
| Intent de origen | [ruta a intent.md] |

<!--
QUÉ VA: datos de control del documento.
POR QUÉ IMPORTA: el agente y el equipo deben saber si la spec se puede implementar ya.
  Solo se implementa una spec en estado "aprobada".
BUENO: "Estado: aprobada · Versión: 1.2 · Fecha: 2026-03-10"
MALO:  Sin estado ni versión. Nadie sabe si es la última.
ERROR FRECUENTE: cambiar la spec después de aprobada sin subir la versión.
  Los tests y el código quedan apuntando a reglas que ya no existen.
-->

---

## 1. Contexto y objetivo

<!--
QUÉ VA: 2 a 4 frases. El problema que se resuelve y el resultado esperado.
  Resume el intent; no lo copia entero.
POR QUÉ IMPORTA: le da al lector el "para qué". Cuando una regla no cubre un caso,
  el objetivo ayuda a decidir.
BUENO: "Los socios no saben cuándo vence su préstamo y pagan multas.
  Queremos avisarles por correo 2 días antes del vencimiento."
MALO:  "Hacer el módulo de notificaciones."
ERROR FRECUENTE: describir la solución técnica ("usar una cola de mensajes")
  en vez del problema. La técnica va en plan.md.
-->

## 2. Historia de usuario

<!--
QUÉ VA: una historia en formato "Como [rol], quiero [acción], para [beneficio]".
  Si hay más de un rol, una historia por rol.
POR QUÉ IMPORTA: fija quién usa la funcionalidad y qué valor obtiene.
  Evita construir cosas que nadie pidió.
BUENO: "Como socio de la biblioteca, quiero recibir un aviso 2 días antes
  de que venza mi préstamo, para devolver el libro a tiempo y no pagar multa."
MALO:  "Como usuario, quiero notificaciones, para estar informado."
ERROR FRECUENTE: usar "usuario" genérico. Casi siempre hay un rol concreto
  (cliente, administrador, invitado) y cada uno tiene permisos distintos.
-->

Como [rol], quiero [acción], para [beneficio].

## 3. Reglas de negocio

<!--
QUÉ VA: lista numerada (RN-01, RN-02...). Una regla por línea.
  Cada regla dice valores exactos: montos, límites, unidades, redondeo, zona horaria.
POR QUÉ IMPORTA: es la parte que más se inventa cuando falta. Los IDs permiten
  citarlas en el plan, las tareas y los tests.
BUENO: "RN-03: El aviso se envía a las 09:00 (hora de Chile) del día que está
  2 días antes del vencimiento. Si ese día es domingo, se envía el sábado."
MALO:  "RN-03: Avisar con anticipación."
ERROR FRECUENTE: no decidir los bordes. "Más de 10" y "desde 10" no son lo mismo.
  Usa siempre "≥", "≤", "<" o ">" cuando hay un límite.
TIP: si tomaste una decisión que el negocio no dijo explícitamente,
  márcala como "Decisión:" y anota quién la confirmó.
-->

- **RN-01:**
- **RN-02:**

## 4. Criterios de aceptación

<!--
QUÉ VA: lista numerada (CA-01, CA-02...). Cada criterio es un caso concreto y verificable.
  Usa "Dado / Cuando / Entonces" con datos reales, no descripciones.
POR QUÉ IMPORTA: cada criterio se convierte en un test. Si el criterio es vago,
  el test es vago o no existe.
BUENO: "CA-04 — Dado un préstamo que vence el 2026-05-12,
  cuando el proceso corre el 2026-05-10 a las 09:00,
  entonces se envía 1 correo al socio con el asunto 'Tu préstamo vence el 12-05'."
MALO:  "CA-04 — El sistema notifica correctamente al usuario."
ERROR FRECUENTE: escribir solo el camino feliz. Por cada regla con un límite,
  agrega un criterio justo en el borde y uno justo afuera.
CHEQUEO: lee cada criterio y pregúntate "¿puedo escribir el test sin preguntar nada?".
  Si la respuesta es no, faltan datos.
-->

**CA-01 — [título corto]**
- **Dado** [estado inicial con datos concretos]
- **Cuando** [acción única]
- **Entonces** [resultado observable con valores exactos]

## 5. Ejemplos de entrada y salida

<!--
QUÉ VA: al menos 3 ejemplos completos: 2 exitosos y 1 de error.
  Muestra la entrada y la salida exactas (JSON, pantalla, archivo).
POR QUÉ IMPORTA: un ejemplo concreto elimina dudas de formato: nombres de campos,
  tipos, mayúsculas, unidades. Sirve directo como test de integración.
BUENO: un bloque con la petición HTTP completa y la respuesta JSON con todos sus campos.
MALO:  "Se envía el préstamo y se recibe la confirmación."
ERROR FRECUENTE: ejemplos que contradicen las reglas (un cálculo mal hecho a mano).
  Revisa la aritmética de cada ejemplo dos veces.
-->

### Ejemplo 1 — [caso exitoso]
### Ejemplo 2 — [otro caso exitoso]
### Ejemplo 3 — [caso de error]

## 6. Errores esperados

<!--
QUÉ VA: tabla con cada error posible: código, cuándo ocurre, código HTTP (o equivalente)
  y mensaje para el usuario.
POR QUÉ IMPORTA: el frontend y los tests dependen del código exacto.
  Sin esta tabla, cada dev inventa su propio formato.
BUENO: "PRESTAMO_VENCIDO · 422 · 'No puedes renovar un préstamo vencido.'"
MALO:  "Si algo falla, mostrar un error."
ERROR FRECUENTE: definir el código pero no el estado del sistema después del error.
  Di siempre si los datos quedan sin cambios.
-->

| Código | HTTP | Cuándo ocurre | Mensaje | Estado después |
|---|---|---|---|---|
| | | | | |

## 7. Fuera de alcance

<!--
QUÉ VA: lista de lo que esta spec NO incluye, aunque alguien podría suponerlo.
POR QUÉ IMPORTA: un agente tiende a "completar" lo que cree que falta.
  Esta lista lo frena y protege el plazo.
BUENO: "No incluye avisos por SMS ni WhatsApp. Solo correo."
MALO:  Sección vacía o "Todo lo demás".
ERROR FRECUENTE: olvidar lo obvio para el negocio pero no para el dev:
  pagos, permisos de administrador, migración de datos antiguos.
-->

-

## 8. Preguntas abiertas

<!--
QUÉ VA: dudas sin respuesta todavía. Cada una con responsable y si bloquea o no.
POR QUÉ IMPORTA: deja visible lo que falta. Es mejor una pregunta escrita
  que una suposición escondida en el código.
BUENO: "P-01: ¿Los socios menores de edad reciben el aviso en el correo del apoderado?
  Responsable: PO. Bloquea: no (fuera de esta versión)."
MALO:  Sin preguntas. Ninguna spec real nace sin dudas.
ERROR FRECUENTE: aprobar la spec con preguntas que bloquean.
  Si una pregunta afecta un criterio de aceptación, se responde antes de implementar.
-->

| ID | Pregunta | Responsable | ¿Bloquea? |
|---|---|---|---|
| P-01 | | | |

## 9. Historial de cambios

<!--
QUÉ VA: una línea por versión: fecha, qué cambió y quién lo aprobó.
POR QUÉ IMPORTA: explica por qué un test cambió y evita discusiones sobre qué se acordó.
BUENO: "1.1 · 2026-04-02 · RN-03 ahora considera feriados · Aprobó: M. Rojas (PO)"
MALO:  Editar la spec sin dejar rastro.
ERROR FRECUENTE: actualizar el código y olvidar la spec. La spec siempre va primero.
-->

| Versión | Fecha | Cambio | Aprobó |
|---|---|---|---|
| 1.0 | | Versión inicial | |
