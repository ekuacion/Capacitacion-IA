# Spec: TEC-01 · Ordenar `ClientesService`

> Versión: 1.1 · Estado: aprobada (Claudio Ortiz Castillo, 2026-11-10) · Intent de origen: `specs/TEC-01-ordenar-clientes-service/intent.md`
> Autor del borrador: Claude Code · Aprobada por: Claudio Ortiz Castillo (regla 1 de `AGENTS.md`)

> Formato: adaptado de `docs/plantilla-spec-breve.md`. En un refactor no hay historia ni funciones nuevas: la spec es
> el **contrato de comportamiento que debe quedar idéntico**. Salió de leer el código y los 4 tests, no de lo que
> «debería» hacer. Las decisiones marcadas con **[Decidir]** son tuyas.

## Qué se preserva

`ClientesService` tiene hoy dos operaciones. Después del refactor deben hacer exactamente lo mismo, con otros nombres.

### Operación 1 · la ficha del cliente (hoy `proc2`)

Recibe el id de un cliente y devuelve un texto de una línea: `Nombre | RUT | Zona | Estado`.

- **CP-01 · Cliente inexistente.** Si el id no existe, devuelve el texto vacío `""`. No lanza excepción.
- **CP-02 · Formato de la ficha.** `"{Nombre} | {RUT} | {Zona} | {Estado}"`, con una barra vertical entre espacios.
- **CP-03 · RUT normalizado.** Se le quitan los puntos y los espacios y se pasa a mayúsculas; el guion se conserva.
  Ejemplo: `76.990.011-k` → `76990011-K`.
- **CP-04 · Zona según la región.** `RM` y `V` → `Zona centro`; `VIII` → `Zona sur`; `II` → `Zona norte`;
  cualquier otra región → `Sin zona`. La comparación es exacta, sin ignorar mayúsculas.
- **CP-05 · Estado.** Cliente activo → `Activo`; inactivo → `Inactivo`.

### Operación 2 · si el cliente está disponible (hoy `proc3`)

Recibe el id de un cliente y devuelve un booleano.

- **CP-06 · Existe y está activo.** Devuelve `true` solo si el cliente existe y está activo. Si está inactivo o no
  existe, devuelve `false`. No lanza excepción.

### Reglas comunes

- **CP-07 · Búsqueda del cliente.** El id se compara de forma exacta (distingue mayúsculas) y, si hubiera ids
  repetidos, gana el primero de la lista.
- **CP-08 · Los datos.** El constructor sin parámetros usa los 8 clientes de `ClientesDeEjemplo`; el constructor con
  lista usa la lista recibida tal cual, sin copiarla.

## Qué cubren hoy los 4 tests

| Criterio | ¿Lo prueba algún test hoy? |
|---|---|
| CP-01 cliente inexistente | Sí: `Proc2_ClienteInexistente_DevuelveTextoVacio` |
| CP-02 formato de la ficha | Sí: `Proc2_ClienteActivo_DevuelveLaFicha` |
| CP-03 RUT normalizado | **Parcial:** prueba puntos y la `k` minúscula (C-004), no los espacios (C-008) |
| CP-04 zona | **Parcial:** `RM` y `V` (centro); **no** `VIII` (sur), `II` (norte) ni `Sin zona` |
| CP-05 estado | **Parcial:** solo `Activo`; **no** `Inactivo` en la ficha |
| CP-06 existe y activo | Sí: `Proc3_ClienteActivoEInactivoDevuelve...` |
| CP-07 y CP-08 | No |

> Estos huecos son el riesgo de la etapa: un refactor podría cambiar una zona o el texto «Inactivo» y los 4 tests
> seguirían en verde. Ver la decisión abierta.

## Decisión abierta

- **Decidido por Claudio: sí se agregan tests de caracterización** antes de refactorizar, para cubrir los huecos de la
  tabla (zonas sur y norte, `Sin zona`, estado `Inactivo` y RUT con espacios). Son tests nuevos que describen lo que
  el código hace hoy; no modifican los 4 existentes. Van como un paso aparte y con su commit (T1), antes de tocar el
  servicio. El taller dice que los 4 tests son la red, pero esa red no ve esas partes.

## Criterios de aceptación

- **CA-01 · Comportamiento idéntico.** Dado el servicio ordenado, cuando se ejecutan los 4 tests de
  `ClientesService`, entonces los 4 pasan y verifican lo mismo que antes.
- **CA-02 · Nombres claros.** Dado el servicio ordenado, entonces no quedan `proc2`, `proc3` ni variables de una
  letra, y cada nombre dice qué hace.
- **CA-03 · Sin repetición.** Dado el servicio ordenado, entonces la búsqueda del cliente por id existe una sola vez.
- **CA-04 · Sin cambios fuera del servicio.** Dado el refactor terminado, entonces `Cliente`, `ClientesDeEjemplo` y
  el resto del código no cambiaron; de los tests (los 4 originales y los de caracterización) solo cambió la forma de
  llamar a los métodos renombrados y los nombres de los tests.

## Fuera de alcance

- Corregir comportamientos que parezcan errores (por ejemplo, que `RM` y `V` compartan zona o que la región se
  compare sin ignorar mayúsculas). Si aparece uno, se anota y se decide en otra historia.
- Cambiar el formato de la ficha, las zonas o los textos de estado.
- Usar `ClientesService` desde la API o desde el detalle del pedido.
- Documentar el módulo (etapa 3).

## Historial

| Versión | Fecha | Cambio | Quién |
|---|---|---|---|
| 0.1 | 2026-11-10 | Borrador inicial a partir del código y de los 4 tests | Claude Code |
| 1.0 | 2026-11-10 | Spec aprobada; se decide agregar tests de caracterización (T1) | Claudio Ortiz Castillo |
| 1.1 | 2026-11-10 | CA-04 incluye los tests de caracterización del T1 entre los que solo cambian en la llamada | Claudio Ortiz Castillo |
