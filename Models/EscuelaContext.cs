using Microsoft.EntityFrameworkCore; 
// Importa el namespace de Entity Framework Core, necesario para trabajar con DbContext y operaciones de base de datos.

namespace ApiMaestrosAlumnos.Models
{
    // Clase que representa el contexto de la base de datos
    public class EscuelaContext : DbContext
    {
        // Estas opciones incluyen la cadena de conexión y otras configuraciones
        public EscuelaContext(DbContextOptions<EscuelaContext> options) : base(options) { }

        // Representa la tabla "Maestros" en la base de datos
        // DbSet permite realizar consultas y operaciones CRUD sobre la tabla
        public DbSet<Maestro> Maestros { get; set; }

        // Representa la tabla "Alumnos" en la base de datos
        // Permite agregar, eliminar, actualizar y consultar registros de alumnos
        public DbSet<Alumno> Alumnos { get; set; }
    }
}
