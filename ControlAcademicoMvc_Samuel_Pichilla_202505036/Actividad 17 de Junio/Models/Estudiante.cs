// Archivo: Models/Estudiante.cs
// Autor: Samuel Esteban Pichillá Duartes — Carné: 202505036
// Curso: IPC2 Laboratorio A — USAC

namespace ControlAcademicoMvc.Models
{
    /// <summary>
    /// Entidad POCO (Plain Old CLR Object) que representa al Estudiante en la capa de dominio.
    /// Esta clase no tiene dependencias de ninguna capa superior (Controller ni View),
    /// garantizando el principio de Alta Cohesión y Bajo Acoplamiento del patrón MVC.
    /// </summary>
    public class Estudiante
    {
        /// <summary>Identificador único del estudiante (número de carné).</summary>
        public int Carne { get; set; }

        /// <summary>Nombre completo del estudiante.</summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Promedio académico acumulado (escala 0-100).</summary>
        public double Promedio { get; set; }

        /// <summary>
        /// Propiedad derivada que clasifica al estudiante según su promedio.
        /// Demuestra que la lógica de dominio pertenece al Modelo, no al Controlador ni a la Vista.
        /// </summary>
        public string ClasificacionAcademica => Promedio switch
        {
            >= 90 => "Excelente",
            >= 75 => "Satisfactorio",
            >= 61 => "Suficiente",
            _      => "Insuficiente"
        };
    }
}
