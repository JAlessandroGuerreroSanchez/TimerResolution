# Publica un único .exe en dist\ y copia una copia con versión en el nombre.
# Cierra Timer Resolution antes de ejecutar si MSB3027 bloquea el archivo.
Set-Location $PSScriptRoot
$m = Select-String -Path .\TimerResolutionApp.csproj -Pattern '<Version>([^<]+)</Version>' | Select-Object -First 1
$ver = if ($m) { $m.Matches.Groups[1].Value } else { "0.0.0" }

dotnet publish -p:PublishProfile=SingleExe-win-x64
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$src = Join-Path $PSScriptRoot "dist\TimerResolutionApp.exe"
$dst = Join-Path $PSScriptRoot "dist\TimerResolutionApp-v$ver.exe"
if (Test-Path $src) {
    Copy-Item -Path $src -Destination $dst -Force
    Write-Host "Listo: $dst"
} else {
    Write-Warning "No se encontró $src"
}
