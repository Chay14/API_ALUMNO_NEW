using Microsoft.AspNetCore.Mvc;
using ApiMaestrosAlumnos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMaestrosAlumnos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaestrosController : ControllerBase
    {
        private readonly EscuelaContext _context;

        public MaestrosController(EscuelaContext context)
        {
            _context = context;
        }

        // GET: api/maestros
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var maestros = await _context.Maestros.Include(m => m.Alumnos).ToListAsync();
            return Ok(maestros);
        }

        // POST: api/maestros - Crear maestro con alumnos opcionales
        [HttpPost]
        public async Task<IActionResult> CrearMaestro([FromBody] Maestro input)
        {
            if (string.IsNullOrWhiteSpace(input.Nombre))
                return BadRequest(new { error = "El nombre del maestro es obligatorio" });

            // Crear maestro aunque no tenga alumnos
            var maestro = new Maestro { Nombre = input.Nombre };
            _context.Maestros.Add(maestro);

            // Agregar alumnos si existen
            if (input.Alumnos != null)
            {
                foreach (var a in input.Alumnos)
                {
                    if (!string.IsNullOrWhiteSpace(a.Nombre))
                        maestro.Alumnos.Add(new Alumno { Nombre = a.Nombre });
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Maestro creado correctamente" });
        }

        // POST: api/maestros/alumno - Crear alumno sin maestro
[HttpPost("alumno")]
public async Task<IActionResult> CrearAlumno([FromBody] Alumno input)
{
    if (string.IsNullOrWhiteSpace(input.Nombre))
        return BadRequest(new { error = "El nombre del alumno es obligatorio" });

    var alumno = new Alumno
    {
        Nombre = input.Nombre,
        MaestroId = input.MaestroId // explícito que no tiene maestro
    };

    _context.Alumnos.Add(alumno);
    await _context.SaveChangesAsync();

    return Ok(new { mensaje = "Alumno creado correctamente sin maestro" });
}


        // DELETE: api/maestros/maestro - Eliminar maestro completo
        [HttpDelete("maestro")]
        public async Task<IActionResult> EliminarMaestro([FromBody] string nombre)
        {
            var maestro = await _context.Maestros.Include(m => m.Alumnos)
                                                 .FirstOrDefaultAsync(m => m.Nombre == nombre);
            if (maestro == null) return NotFound(new { error = "Maestro no encontrado" });

            _context.Maestros.Remove(maestro);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Maestro eliminado" });
        }

        // DELETE: api/maestros/alumno - Eliminar alumno (con o sin maestro)
        [HttpDelete("alumno")]
        public async Task<IActionResult> EliminarAlumno([FromBody] string nombre)
        {
            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.Nombre == nombre);
            if (alumno == null) return NotFound(new { error = "Alumno no encontrado" });

            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Alumno eliminado" });
        }

        // GET: api/maestros/buscar/maestro/{nombre}
        [HttpGet("buscar/maestro/{nombre}")]
        public async Task<IActionResult> BuscarMaestro(string nombre)
        {
            var maestro = await _context.Maestros.Include(m => m.Alumnos)
                                                 .FirstOrDefaultAsync(m => m.Nombre.Contains(nombre));
            if (maestro == null) return NotFound(new { mensaje = "Maestro no encontrado" });

            return Ok(maestro);
        }

        // GET: api/maestros/buscar/alumno/{nombre}
        [HttpGet("buscar/alumno/{nombre}")]
        public async Task<IActionResult> BuscarAlumno(string nombre)
        {
            var alumno = await _context.Alumnos
                                               .FirstOrDefaultAsync(a => a.Nombre.Contains(nombre));
            if (alumno == null) return NotFound(new { mensaje = "Alumno no encontrado" });

            return Ok(new
            {
                alumno = alumno.Nombre,
                //maestro = alumno.Maestro != null ? alumno.Maestro.Nombre : "Sin maestro"
            });
        }
    }
}
