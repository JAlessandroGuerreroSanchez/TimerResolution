# Changelog

All notable changes to this project will be documented in this file.

La numeración **1.2 → 1.4** agrupa el trabajo que antes figuraba como un único salto a 1.5; cada versión corresponde a un hito real en el historial de Git.

## [1.4.1] - 2026-04-09

### Added
- **NOTICE.md**: atribución a TimerTool (referencia), aviso sobre APIs no documentadas de `ntdll`, .NET y redistribución.
- Texto de **ayuda** en la app con resumen de licencia y enlace conceptual a `LICENSE` / `NOTICE.md` en el repo.

### Changed
- **LICENSE**: titular de copyright **KALUR STUDIO** (2025–2026).
- **README**: sección legal, redacción corregida (implementación independiente frente a TimerTool).

## [1.4.0] - 2026-04-09

### Added
- Endurecimiento de **seguridad**: `settings.json` con límites y escritura atómica, validación de URLs al abrir el navegador, comprobación de ms antes de `NtSetTimerResolution`, carga de **ntdll** solo desde System32, **instancia única** (mutex).
- Pruebas adicionales (`BrowserLaunch`, `Normalize` en ajustes, rangos en `TimerMath`).

### Changed
- Versión de ensamblado alineada con la línea **1.2–1.4** (sin salto respecto a 1.1).

## [1.3.0] - 2026-04-08

### Added
- Enlaces del **README** al repositorio real en GitHub; plantilla **winget** con `PackageUrl` y `InstallerUrl` coherentes con Releases.

### Changed
- **`.gitignore`**: entradas para `.env`, certificados `.pfx` y `secrets.json`.
- Eliminación del archivo de ayuda local que contenía rutas de máquina (sustituido por el flujo normal de clon/push).

## [1.2.0] - 2026-04-08

### Added
- Biblioteca **TimerResolutionApp.Core**: `TimerMath`, `TimerResolutionNative` (P/Invoke), `AppSettings`, `AppConstants`.
- Proyecto de pruebas **TimerResolutionApp.Tests** (xUnit) y solución **TimerResolutionApp.sln**.
- Línea visible de **atajos** bajo el área principal; opción **Confirmar al cerrar si hay alta precisión** (persistente).
- **Icono de bandeja** con distintivo en alta precisión; menú **Web y soporte (kalur.me)**.
- Script `publish-single-exe.ps1` que copia `TimerResolutionApp-v{versión}.exe` en `dist\`.
- Carpeta **winget/** con plantilla de manifiesto y notas para publicar en winget-pkgs.

### Changed
- Interfaz oscura (**Mica** / DWM), botones personalizados, menos consumo en primer plano y en bandeja.
- `WarnOnExitIfHighRes` en ajustes (por defecto activado); enlaces usan `AppConstants.SupportWebsiteUrl`.

## [1.1.0] - 2025-03-06

### Added
- Resolución personalizada (0,5 / 1 / 2 / 5 / 15,625 ms) con combo y botón Aplicar
- Bandeja del sistema: minimizar a la bandeja, menú Restaurar / Maximum / Default / Salir
- Inicio con Windows (registro) y opción "Máximo al iniciar"
- Indicador de modo (Modo: Máximo / Por defecto) con color
- Medir Sleep(1): prueba integrada (min/medio/máx en ms)
- Atajos de teclado: Ctrl+M (Maximum), Ctrl+D (Default), Escape (cerrar)
- Valores mostrados también en microsegundos (µs)
- Ayuda (ventana explicativa) y Acerca de (versión)
- Tema oscuro (checkbox)
- Aviso cuando otro proceso mantiene alta precisión
- Aviso de batería al usar Maximum
- Persistencia de opciones en %AppData%\TimerResolutionApp\settings.json

### Changed
- Interfaz reorganizada con GroupBox y barra de estado
- Actualización automática de la resolución cada 1,5 s
- Mensajes de éxito en barra de estado en lugar de MessageBox

## [1.0.0] - 2025-03-06

### Added
- Versión inicial: consultar y establecer resolución del temporizador (ntdll)
- Botones Maximum (0,5 ms), Default y Close
- Restauración al cerrar
