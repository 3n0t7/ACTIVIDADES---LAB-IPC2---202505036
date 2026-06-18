// Archivo: Program.cs
// Autor: Samuel Esteban Pichillá Duartes — Carné: 202505036
// Curso: IPC2 Laboratorio A — USAC
//
// Punto de entrada de la aplicación ASP.NET Core MVC (.NET 8).
// Configura el contenedor de inyección de dependencias (DI),
// el pipeline de middleware HTTP y la plantilla estándar de enrutamiento por convención.

var builder = WebApplication.CreateBuilder(args);

// ── Registro de Servicios en el Contenedor DI ──────────────────────────────────────────────
// AddControllersWithViews registra:
//   - El sistema de controladores MVC
//   - El motor de vistas Razor
//   - El modelo de enlace de datos (Model Binding)
//   - El sistema de validación de modelos
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ── Configuración del Pipeline de Middleware HTTP ──────────────────────────────────────────
// El orden importa: cada middleware pasa la petición al siguiente en la cadena.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Sirve archivos estáticos (CSS, JS, imágenes) desde wwwroot/ sin pasar por el controlador.
app.UseStaticFiles();

// Activa el análisis de rutas en las peticiones entrantes.
app.UseRouting();

// Habilita el sistema de autorización (preparado para extensión futura con roles/claims).
app.UseAuthorization();

// ── Configuración de la Plantilla Estándar de Enrutamiento por Convención ────────────────
// Plantilla: {controller=Home}/{action=Index}/{id?}
//
// Ejemplos de mapeo:
//   /                             → HomeController.Index()          id = null
//   /Home                         → HomeController.Index()          id = null
//   /Estudiante/Listar            → EstudianteController.Listar()   id = null
//   /Estudiante/Historial/2026012 → EstudianteController.Historial(2026012)
//   /Asignacion/Detalle/10        → AsignacionController.Detalle(10)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
