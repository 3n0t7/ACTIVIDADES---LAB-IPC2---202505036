# Sistema de Control Académico — ASP.NET Core MVC

**Autor:** Samuel Esteban Pichillá Duartes  
**Carné:** 202505036 | **CUI:** 3735407520101  
**Curso:** Introducción a la Programación y Computación 2 — Laboratorio A  
**Universidad de San Carlos de Guatemala — Facultad de Ingeniería**  
**Actividad:** 17 de junio de 2026

---

## Descripción

Aplicación web funcional implementada en **ASP.NET Core MVC (.NET 8)** que demuestra:

- **Arquitectura N-Tier**: separación física de responsabilidades entre presentación, lógica y datos.
- **Patrón MVC**: desacoplamiento lógico mediante Modelo, Vista y Controlador independientes.
- **Skinny Controllers**: ningún método supera las 20 líneas de código.
- **Enrutamiento semántico**: plantilla `{controller=Home}/{action=Index}/{id?}`.

---

## Estructura del Proyecto

```
ControlAcademicoMvc/
├── ControlAcademicoMvc.csproj   # Configuración del proyecto .NET 8
├── Program.cs                    # Pipeline HTTP + configuración de enrutamiento
│
├── Models/
│   └── Estudiante.cs             # Entidad POCO de dominio (Capa: Modelo)
│
├── Controllers/
│   ├── HomeController.cs         # Controlador raíz — ruta: /Home o /
│   └── EstudianteController.cs   # Controlador de estudiantes — Skinny Controller
│
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml        # Layout maestro reutilizado por todas las vistas
│   ├── Home/
│   │   └── Index.cshtml          # Vista: página de inicio con tabla de rutas
│   └── Estudiante/
│       ├── Listar.cshtml         # Vista: listado con formulario de registro POST
│       └── Historial.cshtml      # Vista: detalle individual por carné
│
└── wwwroot/
    └── css/
        └── site.css              # Estilos con colores institucionales USAC
```

---

## Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado
- Verificar instalación: `dotnet --version` (debe mostrar `8.x.x`)

---

## Instrucciones de Ejecución

```bash
# 1. Navegar al directorio del proyecto
cd ControlAcademicoMvc

# 2. Restaurar dependencias
dotnet restore

# 3. Ejecutar la aplicación
dotnet run

# 4. Abrir en el navegador
#    http://localhost:5000
```

---

## Endpoints Disponibles

| Método | Ruta                              | Controlador               | Acción       | Descripción                     |
|--------|-----------------------------------|---------------------------|--------------|----------------------------------|
| GET    | `/`                               | `HomeController`          | `Index`      | Página de inicio con demo de rutas |
| GET    | `/Home`                           | `HomeController`          | `Index`      | Ruta alternativa al inicio       |
| GET    | `/Estudiante/Listar`              | `EstudianteController`    | `Listar`     | Lista todos los estudiantes      |
| GET    | `/Estudiante/Historial/{carne}`   | `EstudianteController`    | `Historial`  | Detalle de un estudiante         |
| POST   | `/Estudiante/Registrar`           | `EstudianteController`    | `Registrar`  | Registra nuevo estudiante (JSON) |

---

## Prueba del Endpoint POST con curl

```bash
# Registrar un nuevo estudiante
curl -X POST http://localhost:5000/Estudiante/Registrar \
  -H "Content-Type: application/json" \
  -d '{"carne": 202509999, "nombre": "Carlos López", "promedio": 78.5}'

# Respuesta esperada: HTTP 201 Created
# Body: {"carne":202509999,"nombre":"Carlos López","promedio":78.5,"clasificacionAcademica":"Satisfactorio"}

# Intento con datos inválidos
curl -X POST http://localhost:5000/Estudiante/Registrar \
  -H "Content-Type: application/json" \
  -d '{"carne": -1, "nombre": "", "promedio": 0}'

# Respuesta esperada: HTTP 400 Bad Request
# Body: {"mensaje":"Datos del estudiante inválidos."}
```

---

## Principios de Diseño Aplicados

### Skinny Controllers
Todos los métodos del `EstudianteController` tienen menos de 20 líneas, evitando el antipatrón **Fat Controller**.

### Separación de Preocupaciones (SoC)
- **Modelo** (`Estudiante.cs`): lógica de dominio, incluyendo `ClasificacionAcademica` calculada en el modelo.
- **Vista** (`.cshtml`): solo renderiza datos recibidos, sin SQL ni cálculos.
- **Controlador**: despacha peticiones y coordina Modelo ↔ Vista.

### Enrutamiento por Convención
Configurado en `Program.cs` con la plantilla estándar `{controller=Home}/{action=Index}/{id?}`.

---

## Referencias

1. Facultad de Ingeniería, USAC. (2026). *Sesión 11: Modelado Base y Arquitecturas de Despliegue.* IPC2 Laboratorio.
2. Facultad de Ingeniería, USAC. (2026). *Sesión 12: Arquitectura y Componentes del Patrón MVC.* IPC2 Laboratorio.
3. Microsoft. (2024). *ASP.NET Core MVC Overview.* https://learn.microsoft.com/aspnet/core/mvc/overview
