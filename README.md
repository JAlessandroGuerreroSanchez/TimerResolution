# Timer Resolution

![.NET 8](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet)
![Platform](https://img.shields.io/badge/Platform-Windows-0078D6?logo=windows)
![License](https://img.shields.io/badge/License-MIT-green)

Aplicación de escritorio para Windows que permite consultar y cambiar la resolución del temporizador del sistema (timer resolution), usando las APIs de **ntdll.dll** (`NtQueryTimerResolution` y `NtSetTimerResolution`) para alcanzar hasta **0,5 ms** (en lugar de la resolución por defecto de ~15,6 ms).

Clon de la utilidad [Timer Resolution](https://github.com/tebjan/TimerTool) para Windows, escrita en C# con WinForms (.NET 8).

---

## Descarga

- **[Releases](https://github.com/JAlessandroGuerreroSanchez/TimerResolution/releases)** — Ejecutable para Windows (x64) en la última release (cuando subas el `.exe` como adjunto). El build autocontenido no requiere instalar .NET por separado.

---

## Captura

*(Añade aquí una captura de pantalla de la ventana principal y reemplaza el enlace)*

<!-- ![Timer Resolution](screenshot.png) -->

---

## Características

- Ver resolución **mínima**, **máxima** y **actual** (en ms y µs).
- **Maximum**: establecer resolución al mínimo (0,5 ms).
- **Default**: restaurar el valor por defecto de Windows.
- Resolución **personalizada** (0,5 / 1 / 2 / 5 / 15,625 ms).
- **Bandeja del sistema**: minimizar a la bandeja con menú contextual.
- **Inicio con Windows** y opción **Máximo al iniciar**.
- **Medir Sleep(1)**: prueba integrada para ver el efecto de la resolución.
- Atajos: **Ctrl+M** (Maximum), **Ctrl+D** (Default), **Escape** (cerrar).
- Tema **claro/oscuro**, aviso en **batería**, **Ayuda** y **Acerca de**.

Al cerrar la aplicación se restaura siempre la resolución por defecto.

---

## Desarrollo

- Abre **`TimerResolutionApp.sln`** en Visual Studio o ejecuta:
  - `dotnet build TimerResolutionApp.sln`
  - `dotnet test TimerResolutionApp.Tests\TimerResolutionApp.Tests.csproj`
- La lógica compartida (conversiones, ntdll, ajustes JSON) está en **`TimerResolutionApp.Core`**.
- Publicación portable: `.\publish-single-exe.ps1` (genera `dist\TimerResolutionApp.exe` y `dist\TimerResolutionApp-v{versión}.exe`).
- Plantilla **winget**: carpeta `winget\` (sustituir URL y SHA256 del instalador al publicar).

---

## Requisitos

- Windows (x64).
- .NET 8 Runtime (o usar el .exe autocontenido de [Releases](https://github.com/JAlessandroGuerreroSanchez/TimerResolution/releases)).

---

## Compilar y publicar

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

El ejecutable quedará en la carpeta `publish/`.

---

## Uso

Ejecuta `TimerResolutionApp.exe`. Usa **Maximum** para reducir la resolución a 0,5 ms (útil para menor latencia en juegos o mediciones) y **Default** para volver al valor normal. **Close** restaura y cierra.

---

## Licencia

[MIT](LICENSE) — uso libre. Basado en la utilidad Timer Resolution de la comunidad.
