# Plan de migración · Pedidos Legado → Pedidos Web

> Base: `docs/diagnostico/matriz-decision.md` (U4) y `docs/migracion/defectos-corregidos.md`. Una página; el orden de las etapas 2 a 5 es una propuesta a confirmar.

## 1. Etapas
Un módulo a la vez (strangler fig), de menor a mayor riesgo y dependencia:
1. **Cálculo de totales en C#** (`src/PedidosWeb.Core/Totales/`): hecho en la Unidad 5, con 60 casos en verde y R05 corregido. Falta decidir H1 (umbrales).
2. **Clientes y RUT** (Modernizar): mantenedor simple; unifica la validación del RUT, hoy con dos criterios.
   **Golden master que haría falta:** un CSV como el de totales, con un caso por RUT (válidos, inválidos, con dígito verificador `0` y `K`, y sin guion ni puntos) y lo que responde hoy
   cada validador del legado: `ValidaRut` (`UUtilidades.pas:72-79`) y `RutOk` (`UFrmClientes.pas:192-199`), que difieren con el resto 11 (regla #13 del diagnóstico). Más casos del
   cliente inactivo (regla #15). Como en totales, primero se iguala al legado, defectos incluidos, y la unificación del RUT va en un commit aparte.
3. **Stock** (Reemplazar): rediseñar el rechazo antes de grabar y el registro de movimientos.
4. **Informes** (Reemplazar): informe de lectura sobre la base nueva, con IVA parametrizado.
5. **Toma y grabado de pedidos** (Reemplazar): al final, porque usa a todos los demás (folio, transacción y reglas dentro del formulario).

## 2. Convivencia
Mientras no se apaga el legado, ambos sistemas calculan. El portal usa `CalculoTotales` y el legado sigue con su `Calc3`. Para compararlos:
- El golden master (60 casos) corre en cada `dotnet test` y es la red de seguridad del C#.
- Con pedidos reales, se calculan los totales en los dos sistemas y se comparan por folio. Una diferencia de 1 o 2 pesos en cerca de 22 % de los pedidos
  se concilia como «redondeo R05»; cualquier otra diferencia es un defecto y se investiga.
- Los pedidos con base de exactamente 500.000 o 1.000.000 se marcan aparte (H1) hasta que contabilidad decida el umbral.

## 3. Riesgos
| Riesgo | Mitigación |
|---|---|
| Diferencias de 1 o 2 pesos con el legado por R05 que alguien lea como error | Registro en `defectos-corregidos.md`, aviso a contabilidad y conciliación por folio |
| Umbrales de 500.000 y 1.000.000 sin decidir (H1): 13 de 60 pedidos del golden master | Escalar a contabilidad; cambiar un operador y actualizar el CSV cuando decidan |
| El golden master sale de una réplica y no del sistema vivo (el legado no compila) | Capturar casos reales ejecutando el legado antes del corte y sumarlos al CSV |
| Otras reglas fuera del cálculo (pedido mínimo 30.000 frente a 25.000 en `PARAMETROS.csv`, folio `MAX + 1`, falta de transacción) | Están en `docs/diagnostico/`; se tratan en las etapas 3 a 5, no en esta |
| Los defectos corregidos se mezclan con la migración y ya no se distinguen | Un commit aparte por defecto (como R05) y un caso del CSV por cada cambio |

## 4. Criterio de corte
- **Se apaga el cálculo del legado** cuando: (a) los 60 casos y un lote de pedidos reales dan lo mismo, salvo las diferencias R05 aceptadas por contabilidad;
  (b) H1 está decidido y aplicado; (c) pasa un ciclo de cierre mensual sin diferencias sin explicar.
- **Vuelta atrás:** el legado queda instalado y sin cambios (`legado/` es solo lectura) durante todo ese ciclo. Si aparece una diferencia sin explicar, el portal
  vuelve a mostrar el cálculo del legado y se corrige el C# con un caso nuevo en el golden master.
