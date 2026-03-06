# Timer Resolution

Aplicación de escritorio para Windows que permite consultar y cambiar la resolución del temporizador del sistema (timer resolution), usando las APIs de **ntdll.dll** (`NtQueryTimerResolution` y `NtSetTimerResolution`) para alcanzar hasta **0,5 ms** (en lugar de la resolución por defecto de ~15,6 ms).

Clon de la utilidad [Timer Resolution](https://github.com/tebjan/TimerTool) para Windows, escrita en C# con WinForms (.NET 8).

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

## Requisitos

- Windows (x64).
- .NET 8 Runtime (o ejecutar el .exe autocontenido incluido en `publish/`).

## Compilar y publicar

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

El ejecutable quedará en la carpeta `publish/`.

## Uso

Ejecuta `TimerResolutionApp.exe`. Usa **Maximum** para reducir la resolución a 0,5 ms (útil para menor latencia en juegos o mediciones) y **Default** para volver al valor normal. **Close** restaura y cierra.

## Licencia

Proyecto de uso libre. Basado en la utilidad Timer Resolution de la comunidad.
