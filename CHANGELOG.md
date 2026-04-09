# Changelog

All notable changes to this project will be documented in this file.

## [1.5.0] - 2026-04-08

### Added
- Biblioteca **TimerResolutionApp.Core**: `TimerMath`, `TimerResolutionNative` (P/Invoke), `AppSettings`, `AppConstants`.
- Proyecto de pruebas **TimerResolutionApp.Tests** (xUnit) y solución **TimerResolutionApp.sln**.
- Línea visible de **atajos** bajo el área principal; opción **Confirmar al cerrar si hay alta precisión** (persistente).
- **Icono de bandeja** con distintivo en alta precisión; menú **Web y soporte (kalur.me)**.
- Script `publish-single-exe.ps1` copia además `TimerResolutionApp-v{versión}.exe` en `dist\`.
- Carpeta **winget/** con plantilla de manifiesto y notas para publicar en winget-pkgs.

### Changed
- `WarnOnExitIfHighRes` en ajustes (por defecto activado); `OpenStudioUrl` usa `AppConstants.SupportWebsiteUrl`.

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
- Aviso cuando otro proceso mantiene alta resolución
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
