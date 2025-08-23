using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiMaestrosAlumnos.Models
{
    public class Maestro
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public List<Alumno> Alumnos { get; set; } = new List<Alumno>();
    }
}
