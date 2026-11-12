# Registro de calidad · Unidad 6

> Taller hecho por Claudio Ortiz Castillo con Claude Code (spec TEC-05 v0.5). Fecha: 2026-11-12. Lo que no se hizo por falta de tiempo queda dicho como tal.

## Preparación

- **Tests en `Skip` (pendientes de unidades anteriores):** ninguno. Base: 143 en verde, 0 omitidos.
- **Extra de la spec (H1):** `PorcentajeVolumen` pasa de `>` a `>=` por decisión del PO. Cambian 13 casos del golden master (suma de `total` 58.202.144 → 57.773.744). Commits `09da648` (código y CSV) y `7b3ceef` (documentación).

## Etapa 1 · Tests que sí prueban

- **Módulo elegido:** CalculoTotales.
- **Tests que propuso Claude:** 7 (5 de borde: pedido sin líneas, categoría OFE con espacios y minúsculas, IVA 0, flete base 0 y flete base 10 en región 12; y 2 de umbral: base de 500.000 y de 1.000.000). Valores esperados calculados a mano; los aprobé uno por uno.
- **Cambio que hice a mano para probarlos:** **ninguno. No lo hice por falta de tiempo.**
- **Tests que me quedé:** los 7 (`CalculoTotalesBordeTests.cs`, commit `368e88e`). Pasan con el código sano (150 en verde en total), pero **no están verificados por mutación**: no sé si cada uno puede fallar.
- **Tests que borré y por qué:** ninguno, porque no hubo prueba que lo justificara. **El criterio CA-02 queda sin cumplir.** Las roturas propuestas (quitar `lineas.Count == 0`, quitar `.Trim()`, fijar `0.19m`, sumar 1 al flete, `+ 0.49m`, y `>` en cada umbral) siguen pendientes.

## Etapa 2 · Pipeline seguro

- **`ci.yml`:** escrito (commit `bb323b8`, rama `feature/u6-taller-6` subida). Enlace al primer run en verde: **por completar por Claudio** (no lo he visto).
- **`claude.yml` en `main`:** **no** (al 2026-11-12 `main` sigue en `abc9635`; los workflows solo están en la rama de trabajo).
- **Revisé (en los archivos):** permisos mínimos ☑ (`contents: read`) · timeout ☑ (10 min) · versiones fijas ☑ (`checkout v7.0.1`, `setup-dotnet v6.1.0`, `claude-code-action v1.0.271`, SDK 10.0.403, `ubuntu-24.04`) · token solo como secreto ☑ (en los archivos solo aparece el nombre `CLAUDE_CODE_OAUTH_TOKEN`; la creación del secreto en GitHub la hace Claudio y no está comprobada aquí).
- **CI local en Docker** (commit `3fd3e2a`): `scripts/ci-local.ps1` en verde. Construye y prueba en contenedor, despliega la API como usuario `app` y `GET http://localhost:5080/salud` responde `{"estado":"ok"}`. Ajuste: el chequeo usa `curl.exe` porque `Invoke-RestMethod` de PowerShell 5.1 fallaba por timeout.
- **Compuerta con un test en rojo (CA-05):** **no probada**, porque exige romper código.

## Etapa 3 · La falla

- **PR:** **no se hizo.**
- **Qué rompí (a mano):** nada; la falla de la tasa de IVA no se ejecutó por falta de tiempo.
- **Diagnóstico de `@claude` (en una línea):** no aplica (no hubo falla ni PR).
- **Mi decisión:** no aplica.
- **Por qué:** el taller se cerró sin la etapa de la falla. **El criterio CA-07 queda sin cumplir**, y con él la prueba de que `claude.yml` responde.
- **Corrección y merge:** no aplica.

## Pendiente para completarlo

1. Romper a mano cada test nuevo (tabla de roturas de la etapa 1) y borrar los que no fallen.
2. Mergear `ci.yml` y `claude.yml` a `main` y confirmar el run en verde.
3. Provocar la falla de la tasa de IVA en una rama nueva, ver el rojo en `ci-local.ps1` y en Actions, y comentar `@claude` en el PR.
