# Integracion_Datos.md
**Nombre:** Samuel  
**Carné:** 202505036  
**Curso:** Introducción a la Programación y Computación 2  
**Actividad:** Laboratorio – Interoperabilidad y Carga Masiva de Datos  
**Fecha:** 26 de junio de 2026

---

## Parte 1: Evaluación Conceptual

### Formatos de Intercambio

| Formato | Ventajas | Desventajas |
|---------|----------|-------------|
| CSV | Simple, liviano y fácil de generar. Compatible con casi cualquier herramienta (Excel, bases de datos, etc.). Ideal para datos tabulares masivos. | No soporta jerarquías ni tipos de datos complejos. No incluye metadatos ni descripción de los campos. |
| XML | Soporta estructuras jerárquicas y anidadas. Autodescriptivo gracias a las etiquetas. Ampliamente compatible con sistemas legados. | Archivo más pesado por la verbosidad de las etiquetas. Más lento de parsear en comparación con JSON o CSV. |

---

### 1. Diferenciación de Procesos

**Serialización** es el proceso de convertir un objeto de C# en una cadena de texto en formato JSON, para poder transmitirlo o almacenarlo. Con `System.Text.Json` se hace con `JsonSerializer.Serialize(objeto)`.

**Deserialización** es el proceso inverso: tomar una cadena JSON y convertirla de vuelta en un objeto de C#. Se hace con `JsonSerializer.Deserialize<Tipo>(json, opciones)`.

En resumen: serializar es objeto → JSON, y deserializar es JSON → objeto.

---

### 2. El Antipatrón N+1

El error N+1 ocurre cuando, al leer un archivo masivo, se realiza una consulta o inserción a la base de datos por cada línea del archivo. Si el archivo tiene 1000 registros, se generan 1001 operaciones (1 para abrir + 1000 individuales), lo que satura la conexión y hace muy lento el proceso.

La solución es usar **Batching**: acumular todos los registros en una lista mientras se lee el archivo, y al terminar el ciclo realizar una sola inserción con `AddRange()` y `SaveChangesAsync()`. Así se reduce a una única operación sobre la base de datos.

---

## Parte 2: Implementación Práctica

El código completo está en el archivo adjunto `IntegracionDatos.cs`.

### Desafío 1 – Consumo de Endpoint

```csharp
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

        return JsonSerializer.Deserialize<Alumno>(json, opciones);
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Error al consumir el endpoint: {ex.Message}");
        return null;
    }
}
```

### Desafío 2 – Carga Masiva CSV

```csharp
[HttpPost("carga-csv")]
public async Task<IActionResult> CargarCsv(IFormFile archivo)
{
    if (archivo == null || archivo.Length == 0)
        return BadRequest("El archivo está vacío.");

    var alumnos = new List<Alumno>();

    using (var stream = archivo.OpenReadStream())
    using (var reader = new StreamReader(stream))
    {
        await reader.ReadLineAsync(); // saltar encabezado

        string linea;
        while ((linea = await reader.ReadLineAsync()) != null)
        {
            var columnas = linea.Split(',');
            alumnos.Add(new Alumno
            {
                Id     = int.Parse(columnas[0]),
                Nombre = columnas[1],
                Carnet = columnas[2]
            });
        }
    }

    await _context.Alumnos.AddRangeAsync(alumnos);
    await _context.SaveChangesAsync();

    return Ok($"Se cargaron {alumnos.Count} alumnos correctamente.");
}
```

---

## Parte 3: Referencias Bibliográficas

Facultad de Ingeniería, USAC. (2026). *Sesión 20: Integración de Datos. Consumo de APIs Externas y Carga Masiva (CSV/XML)*. Laboratorio del curso Introducción a la Programación y Computación 2. Guatemala.
