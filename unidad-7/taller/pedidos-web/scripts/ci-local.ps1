<#
.SYNOPSIS
  CI local de Pedidos Web (TEC-05): build y test en un contenedor; si pasan, despliega la API en Docker.
.PARAMETER Puerto
  Puerto del host (por defecto 5080). Dentro del contenedor la API escucha en 8080.
.PARAMETER Detener
  Baja y borra el contenedor pedidos-web y termina.
#>
param(
    [int]$Puerto = 5080,
    [switch]$Detener
)

$ErrorActionPreference = 'Continue'
$raiz = Split-Path -Parent $PSScriptRoot
$log = Join-Path $raiz 'ci-local.log'
$nombre = 'pedidos-web'
$imagen = 'pedidos-web:local'

function Falla([string]$mensaje) {
    Write-Host "CI LOCAL EN ROJO: $mensaje" -ForegroundColor Red
    exit 1
}

# 0. Docker debe responder (Docker Desktop abierto).
docker info *> $null
if ($LASTEXITCODE -ne 0) { Falla 'Docker no responde. Abre Docker Desktop, espera a que diga "running" y reintenta.' }

if ($Detener) {
    docker rm -f $nombre *> $null
    Write-Host "Contenedor $nombre detenido." -ForegroundColor Green
    exit 0
}

# El puerto no debe estar ocupado por otro proceso (el contenedor anterior sí se reemplaza).
docker rm -f $nombre *> $null
if (Get-NetTCPConnection -LocalPort $Puerto -State Listen -ErrorAction SilentlyContinue) {
    Falla "El puerto $Puerto está ocupado. Usa otro con -Puerto."
}

Push-Location $raiz
try {
    # 1. Compuerta: build y test dentro del contenedor. cmd /c evita el problema de PowerShell 5.1 con el stderr de docker.
    Write-Host '1/3 Build y test en contenedor...' -ForegroundColor Cyan
    cmd /c "docker build --target test --progress=plain . > ""$log"" 2>&1"
    if ($LASTEXITCODE -ne 0) {
        Get-Content $log -Tail 25
        Falla "build o tests fallaron. No se despliega. Log completo: $log"
    }
    Remove-Item $log -ErrorAction SilentlyContinue

    # 2. Imagen final (repite el test desde la caché) y despliegue solo en localhost.
    Write-Host '2/3 Construyendo la imagen y desplegando...' -ForegroundColor Cyan
    docker build -t $imagen . *> $null
    if ($LASTEXITCODE -ne 0) { Falla 'no se pudo construir la imagen final.' }
    docker run -d -p "127.0.0.1:${Puerto}:8080" --name $nombre $imagen *> $null
    if ($LASTEXITCODE -ne 0) { Falla 'no se pudo iniciar el contenedor.' }

    # 3. Comprobación de /salud.
    Write-Host "3/3 Comprobando http://localhost:$Puerto/salud ..." -ForegroundColor Cyan
    # curl.exe abre una conexión nueva en cada intento (Invoke-RestMethod de PS 5.1 reutiliza la que el proxy de Docker aceptó antes de que la API arrancara).
    $ok = $false
    $ultimo = 'sin respuesta'
    for ($i = 0; $i -lt 20 -and -not $ok; $i++) {
        $cuerpo = curl.exe -s -m 2 "http://localhost:$Puerto/salud"
        if ($LASTEXITCODE -eq 0 -and $cuerpo) {
            $ultimo = $cuerpo
            $ok = ($cuerpo | ConvertFrom-Json).estado -eq 'ok'
        }
        if (-not $ok) { Start-Sleep -Seconds 1 }
    }
    if (-not $ok) {
        docker logs $nombre
        Falla "/salud no respondió {`"estado`":`"ok`"}. Última respuesta: $ultimo"
    }
    Write-Host "CI LOCAL EN VERDE: API desplegada en http://localhost:$Puerto (usa -Detener para bajarla)." -ForegroundColor Green
}
finally {
    Pop-Location
}
