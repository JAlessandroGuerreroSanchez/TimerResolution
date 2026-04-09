# Avisos legales y atribuciones — Timer Resolution

*Este texto informa; no sustituye asesoramiento jurídico.*

## Licencia de este proyecto

El código fuente de **Timer Resolution** (este repositorio) se publica bajo la **Licencia MIT**. El texto completo está en el archivo [`LICENSE`](LICENSE) en la raíz del repositorio.

Titular del copyright: **KALUR STUDIO** (2025–2026).

Al redistribuir binarios o código, conserva el archivo `LICENSE` y, si procede, este `NOTICE.md`.

## Inspiración y proyectos de referencia

Esta aplicación implementa en **C# / .NET** la idea de **consultar y cambiar la resolución del temporizador del sistema en Windows**, en la línea de utilidades como **TimerTool**:

- **TimerTool** (Tebjan Hirsch): [github.com/tebjan/TimerTool](https://github.com/tebjan/TimerTool)

**Timer Resolution no distribuye el código fuente de TimerTool** ni se presenta como fork oficial. Es una **implementación independiente**.

En el repositorio de TimerTool **no consta un archivo de licencia SPDX** en la rama principal según la información pública de GitHub. Si incorporas en el futuro fragmentos de código de terceros, revisa siempre la licencia del origen y cumple sus condiciones (atribución, copia de licencia, etc.).

## APIs del sistema (ntdll)

La aplicación usa las funciones exportadas por **`ntdll.dll`**:

- `NtQueryTimerResolution`
- `NtSetTimerResolution`

Estas funciones **no forman parte de la API documentada y estable de Microsoft** para aplicaciones de escritorio. Microsoft puede cambiar su comportamiento entre versiones de Windows. El uso es bajo tu propia responsabilidad; los autores no garantizan idoneidad para un fin concreto.

El programa **no modifica** `ntdll.dll`; solo la invoca en tiempo de ejecución en el equipo del usuario, como otras herramientas similares de administración del temporizador.

## Plataforma y dependencias

- **Windows** y componentes del sistema operativo están sujetos a los términos de licencia de Microsoft.
- **.NET** (runtime y bibliotecas usadas por el proyecto) se distribuye bajo licencias de Microsoft / .NET Foundation (p. ej. MIT para gran parte del runtime). Consulta [licencias de .NET](https://github.com/dotnet/runtime/blob/main/LICENSE.TXT) si empaquetas o redistribuyes el runtime.

## Marca

**KALUR STUDIO** y el sitio asociado son identidad del editor. **Timer Resolution** es un nombre descriptivo del propósito de la aplicación.

## Contacto

Soporte / web del proyecto: [kalur.me](https://kalur.me/) (según `AppConstants` en el código).
