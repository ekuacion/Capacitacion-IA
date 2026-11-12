# Plan: TEC-05 · Calidad, pruebas y CI/CD local con Docker

> Spec de origen: `specs/TEC-05-calidad-pruebas-ci/spec.md` v0.5 · Autor: Claude Code · Estado: **aprobado** (Claudio, 2026-11-12)

## Enfoque

Orden: **normalizar H1 → tests que sí prueban → pipeline seguro → falla real**. H1 va primero para que los tests de borde nazcan sobre la regla final
(patrón de R05: un commit aparte, con los casos antes y después). El resto sigue la guía, pero con el pipeline en Docker y en local: no se conecta GitHub,
no hay secretos y no se instala nada. La compuerta es el propio `docker build --target test`: si un test falla, no hay imagen y no hay despliegue.

## Reparto de los 80 minutos

| Min | Tarea | Qué pasa | Si te atrasas |
|---|---|---|---|
| 5 | T0 · Preparación | Spec aprobada, rama, base verde (143), Docker Desktop abierto | — |
| 15 | T1 · Etapa 1 | H1 a `>=`: código, 13 casos del CSV y documento, en commits aparte | Documento breve |
| 20 | T2 · Etapa 2 | 5 tests de borde y 2 de umbral, cada uno roto a mano | Quedarse con 3 tests revisados |
| 15 | T3 · Etapa 3b | `ci.yml` y `claude.yml`; Claudio sube el código, instala la App y crea el secreto | `u6-respaldo/claude.yml` |
| 15 | T4 · Etapa 3 | `Dockerfile`, `.dockerignore`, `ci-local.ps1` y despliegue en verde | Despliegue sin el chequeo de `/salud` |
| 20 | T5 · Etapa 4 | Falla a mano (IVA), CI rojo, PR con `@claude`, corrección y merge | Diagnóstico local con el log de Docker |
| 5 | T6 · Cierre | Registro completo y comprobaciones | — |

## Decisiones técnicas

- **Un solo archivo de tests nuevo** (`CalculoTotalesBordeTests.cs`), sin tocar `GoldenMasterTests.cs` ni el CSV.
- **Valores esperados a mano**, derivados de las reglas RG-01…RG-08, no calculados con `CalculoTotales`.
- **El «romper a mano» lo hace Claudio en Rider**, para que el hook no le devuelva el error a Claude y este lo «arregle».
- **Dockerfile multietapa** (`test` y `final`) en lugar de dos archivos: el mismo código se prueba y se despliega, y el CI es `--target test`.
- **Imágenes oficiales de Microsoft con versión fija** (`10.0`); sin caché externa ni registros; sin paquetes NuGet nuevos (las dependencias ya están en el `.csproj`).
- **Puerto 8080 dentro del contenedor, 5080 en el host**; la API se publica solo en `localhost`.
- **Sin secretos:** no hay variables sensibles; `.dockerignore` deja fuera `.git/` y `.claude/`.
- **GitHub antes que Docker:** el verde de `ci.yml` solo se ve tras el push de Claudio, así que se deja listo temprano.
- **Secretos solo en GitHub:** Claude no genera ni ve el token; solo escribe el nombre `CLAUDE_CODE_OAUTH_TOKEN` en `claude.yml`.

## Riesgos

| Riesgo | Mitigación |
|---|---|
| `pedidos-web` no es la raíz del repo de GitHub | Resuelto (v0.5): workflows en la raíz del repo con `working-directory` y `paths` hacia `unidad-6/taller/pedidos-web` |
| Repo privado: Actions gasta minutos del plan | Un solo job corto (timeout 10 min) y `paths` para no correr con cambios de otras unidades |
| `claude.yml` no está en `main` y `@claude` no responde | Mergear `ci.yml` y `claude.yml` a `main` antes del PR de la falla |
| `@claude` no responde (App, `claude.yml` en `main`, nombre del secreto) | Revisar los tres; si sigue, `u6-respaldo/` y diagnóstico local |
| Token pegado en un archivo | Generar otro; `git grep` antes de cada commit (CA-11) |
| Cuota de Claude Pro | `--max-turns 5`; un solo `@claude` en la etapa 4 |
| Docker Desktop no está corriendo | Abrirlo antes de T4; el script lo comprueba con `docker info` y explica qué falta |
| `dotnet restore` en el contenedor necesita internet (NuGet) | Es el mismo acceso que ya usa `dotnet test` en tu máquina; si falla, se anota en el registro |
| Puerto 5080 ocupado | D-02; el script avisa y permite cambiarlo con un parámetro |
| El hook corrige la rotura de la etapa 2 o 4 | Romper en Rider, nunca con Claude |
| CI verde aunque el código esté roto (tests que no prueban) | Etapa 2: cada test debe fallar una vez |
| Contenedor huérfano o imagen vieja | `ci-local.ps1 -Detener` y `docker image prune` solo con tu aprobación |
