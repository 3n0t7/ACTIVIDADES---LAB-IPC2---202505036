// Archivo: Controllers/EstudianteController.cs
// Autor: Samuel Esteban Pichillá Duartes — Carné: 202505036
// Curso: IPC2 Laboratorio A — USAC
//
// PRINCIPIO APLICADO: Skinny Controller (Controlador Delgado)
// Ningún método supera las 20 líneas de código.
// El controlador es un despachador perimetral: recibe peticiones,
// delega al Modelo y retorna la Vista apropiada. No contiene lógica de negocio.

using Microsoft.AspNetCore.Mvc;
using ControlAcademicoMvc.Models;

namespace ControlAcademicoMvc.Controllers
{
    public class EstudianteController : Controller
    {
        // ── Almacenamiento simulado en memoria centralizada (emula el Tier 3 — Data Tier) ──
        // En un sistema productivo, este bloque sería reemplazado por una interfaz
        // IEstudianteRepository inyectada mediante el contenedor de dependencias,
        // apuntando a una base de datos real en el nivel físico correspondiente.
        private static readonly List<Estudiante> _baseDatosMemoria = new()
        {
            new Estudiante { Carne = 2026012, Nombre = "Fernando Velásquez",         Promedio = 91.5 },
            new Estudiante { Carne = 2026045, Nombre = "María Mercedes",             Promedio = 84.0 },
            new Estudiante { Carne = 202505036, Nombre = "Samuel Pichillá Duartes",  Promedio = 95.0 }
        };

        // ── GET: /Estudiante/Listar ────────────────────────────────────────────────────────
        /// <summary>
        /// Retorna la vista de listado con todos los estudiantes registrados.
        /// El controlador extrae los datos del modelo y los inyecta a la vista sin procesarlos.
        /// </summary>
        public IActionResult Listar()
        {
            return View(_baseDatosMemoria);
        }

        // ── GET: /Estudiante/Historial/{id} ───────────────────────────────────────────────
        /// <summary>
        /// Demuestra el enrutamiento con parámetro id en la plantilla {controller}/{action}/{id?}.
        /// Retorna el historial (detalle) de un estudiante específico por su carné.
        /// </summary>
        public IActionResult Historial(int id)
        {
            var estudiante = _baseDatosMemoria.FirstOrDefault(e => e.Carne == id);
            if (estudiante is null)
                return NotFound(new { mensaje = $"Estudiante con carné {id} no encontrado." });

            return View(estudiante);
        }

        // ── POST: /Estudiante/Registrar ───────────────────────────────────────────────────
        /// <summary>
        /// Recibe un nuevo estudiante en formato JSON, aplica validación perimetral
        /// y delega la persistencia al almacenamiento en memoria.
        /// Retorna HTTP 201 Created con la ubicación del nuevo recurso.
        /// </summary>
        [HttpPost]
        public IActionResult Registrar([FromBody] Estudiante nuevoEstudiante)
        {
            if (nuevoEstudiante.Carne <= 0 || string.IsNullOrEmpty(nuevoEstudiante.Nombre))
                return BadRequest(new { mensaje = "Datos del estudiante inválidos." });

            if (_baseDatosMemoria.Any(e => e.Carne == nuevoEstudiante.Carne))
                return Conflict(new { mensaje = "Ya existe un estudiante con ese carné." });

            _baseDatosMemoria.Add(nuevoEstudiante);
            return Created($"/Estudiante/Historial/{nuevoEstudiante.Carne}", nuevoEstudiante);
        }
    }
}
