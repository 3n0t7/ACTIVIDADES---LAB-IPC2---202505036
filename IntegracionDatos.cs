using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// =============================================
// Desafío 1: Consumo de Endpoint y Deserialización
// =============================================

public class Alumno
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Carnet { get; set; }
}

public class AlumnoService
{
    private readonly HttpClient _httpClient;

    public AlumnoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Alumno> ObtenerAlumnoAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://api.usac.edu/v1/alumnos");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            Alumno alumno = JsonSerializer.Deserialize<Alumno>(json, opciones);
            return alumno;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error al consumir el endpoint: {ex.Message}");
            return null;
        }
    }
}

// =============================================
// Desafío 2: Endpoint para Carga Masiva CSV
// =============================================

[ApiController]
[Route("api/[controller]")]
public class AlumnosController : ControllerBase
{
    private readonly AppDbContext _context;

    public AlumnosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("carga-csv")]
    public async Task<IActionResult> CargarCsv(IFormFile archivo)
    {
        if (archivo == null || archivo.Length == 0)
            return BadRequest("El archivo está vacío.");

        var alumnos = new List<Alumno>();

        using (var stream = archivo.OpenReadStream())
        using (var reader = new StreamReader(stream))
        {
            // Saltar encabezado
            await reader.ReadLineAsync();

            string linea;
            while ((linea = await reader.ReadLineAsync()) != null)
            {
                var columnas = linea.Split(',');

                var alumno = new Alumno
                {
                    Id     = int.Parse(columnas[0]),
                    Nombre = columnas[1],
                    Carnet = columnas[2]
                };

                alumnos.Add(alumno);
            }
        }

        // Inserción por lotes: una sola operación a la BD
        await _context.Alumnos.AddRangeAsync(alumnos);
        await _context.SaveChangesAsync();

        return Ok($"Se cargaron {alumnos.Count} alumnos correctamente.");
    }
}

// Contexto de base de datos (referencia)
public class AppDbContext : DbContext
{
    public DbSet<Alumno> Alumnos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
