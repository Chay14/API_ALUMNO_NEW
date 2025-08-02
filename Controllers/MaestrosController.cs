using Microsoft.AspNetCore.Mvc;
using ApiMaestrosAlumnos.Models;
using System.Text.Json;

namespace ApiMaestrosAlumnos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaestrosController : ControllerBase
    {
        // Ruta del archivo JSON que almacena los registros
        private readonly string archivo = "registros.json";

        // Método para leer los datos desde el archivo JSON
        private List<Maestro> LeerDatos()
        {
            if (!System.IO.File.Exists(archivo))
                return new List<Maestro>(); // Si no existe el archivo, retorna una lista vacía

            var contenido = System.IO.File.ReadAllText(archivo); // Lee el contenido del archivo
            return JsonSerializer.Deserialize<List<Maestro>>(contenido) ?? new List<Maestro>();
        }

        // Método para guardar los datos en el archivo JSON
        private void GuardarDatos(List<Maestro> datos)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true }; // Formatea el JSON con indentación
            System.IO.File.WriteAllText(archivo, JsonSerializer.Serialize(datos, opciones)); // Guarda los datos
        }

        // GET: api/maestros - Retorna todos los registros
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(LeerDatos());
        }

        // POST: api/maestros - Agrega un alumno a un maestro (o crea uno nuevo si no existe)
        [HttpPost]
        public IActionResult AgregarAlumno([FromBody] Maestro input)
        {
            // Validación: se requiere nombre y al menos un alumno
            if (string.IsNullOrWhiteSpace(input.Nombre) || input.Alumnos == null || input.Alumnos.Count == 0)
                return BadRequest(new { error = "Datos incompletos" });

            string alumno = input.Alumnos[0]; // Se toma solo el primer alumno de la lista
            var registros = LeerDatos();

            // Se busca si el maestro ya existe
            var maestro = registros.FirstOrDefault(m => m.Nombre.Equals(input.Nombre, StringComparison.OrdinalIgnoreCase));
            if (maestro != null)
            {
                // Si el alumno no está ya en la lista, lo agrega
                if (!maestro.Alumnos.Contains(alumno))
                    maestro.Alumnos.Add(alumno);
            }
            else
            {
                // Si el maestro no existe, se crea uno nuevo con el alumno
                registros.Add(new Maestro { Nombre = input.Nombre, Alumnos = new List<string> { alumno } });
            }

            GuardarDatos(registros); // Guarda los cambios
            return Ok(new { mensaje = "Registro agregado" });
        }

        // DELETE: api/maestros/maestro - Elimina un maestro completo
        [HttpDelete("maestro")]
        public IActionResult EliminarMaestro([FromBody] string nombre)
        {
            var registros = LeerDatos();
            // Filtra para eliminar el maestro con el nombre especificado
            var nuevos = registros.Where(m => !m.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)).ToList();

            if (nuevos.Count == registros.Count)
                return NotFound(new { error = "Maestro no encontrado" }); // No se eliminó ningún maestro

            GuardarDatos(nuevos); // Guarda la lista sin el maestro eliminado
            return Ok(new { mensaje = "Maestro eliminado" });
        }

        // DELETE: api/maestros/alumno - Elimina un alumno específico de un maestro
        [HttpDelete("alumno")]
        public IActionResult EliminarAlumno([FromBody] Dictionary<string, string> body)
        {
            // Valida que se envíen ambos datos
            if (!body.TryGetValue("maestro", out var maestroNombre) || !body.TryGetValue("alumno", out var alumnoNombre))
                return BadRequest(new { error = "Datos incompletos" });

            var registros = LeerDatos();
            // Busca al maestro
            var maestro = registros.FirstOrDefault(m => m.Nombre.Equals(maestroNombre, StringComparison.OrdinalIgnoreCase));

            // Valida existencia del maestro y del alumno
            if (maestro == null || !maestro.Alumnos.Contains(alumnoNombre))
                return NotFound(new { error = "Maestro o alumno no encontrado" });

            maestro.Alumnos.Remove(alumnoNombre); // Elimina el alumno
            if (maestro.Alumnos.Count == 0)
                registros.Remove(maestro); // Si el maestro queda sin alumnos, también se elimina

            GuardarDatos(registros);
            return Ok(new { mensaje = "Alumno eliminado" });
        }

        // GET: api/maestros/buscar/maestro/{nombre} - Busca un maestro por nombre
        [HttpGet("buscar/maestro/{nombre}")]
        public IActionResult BuscarMaestro(string nombre)
        {
            var registros = LeerDatos();
            // Busca un maestro cuyo nombre contenga el texto indicado
            var maestro = registros.FirstOrDefault(m => m.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
            if (maestro == null)
                return NotFound(new { mensaje = "Maestro no encontrado" });

            return Ok(maestro); // Retorna el maestro encontrado
        }

        // GET: api/maestros/buscar/alumno/{nombre} - Busca a qué maestro pertenece un alumno
        [HttpGet("buscar/alumno/{nombre}")]
        public IActionResult BuscarAlumno(string nombre)
        {
            var registros = LeerDatos();
            // Recorre todos los maestros buscando el alumno
            foreach (var maestro in registros)
            {
                if (maestro.Alumnos.Any(a => a.Contains(nombre, StringComparison.OrdinalIgnoreCase)))
                {
                    return Ok(new { alumno = nombre, maestro = maestro.Nombre });
                }
            }

            return NotFound(new { mensaje = "Alumno no encontrado" });
        }
    }
}
