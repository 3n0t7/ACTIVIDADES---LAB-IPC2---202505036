// Archivo: Controllers/HomeController.cs
// Autor: Samuel Esteban Pichillá Duartes — Carné: 202505036
// Curso: IPC2 Laboratorio A — USAC
//
// Controlador raíz del sistema. Maneja la ruta por defecto: /Home o simplemente /
// Demuestra el comportamiento de la plantilla {controller=Home}/{action=Index}/{id?}
// cuando no se especifican segmentos en la URL.

using Microsoft.AspNetCore.Mvc;

namespace ControlAcademicoMvc.Controllers
{
    public class HomeController : Controller
    {
        // ── GET: /Home o GET: / ────────────────────────────────────────────────────────────
        /// <summary>
        /// Punto de entrada principal de la aplicación.
        /// Prueba de ruta: https://localhost/Home → HomeController.Index()
        ///                 https://localhost/     → HomeController.Index() (valores por defecto)
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }
    }
}
