using System.Text.Json.Serialization;

namespace ApiMaestrosAlumnos.Models
{
    public class Alumno
    {
        public int Id { get; set; }

        public string Nombre { get; set; } // Ya no [Required], puede estar solo

        public int? MaestroId { get; set; } // Nullable

       // [JsonIgnore] // Ignora la relación para que Swagger no intente mapear Maestro
        //public Maestro Maestro { get; set; }
    }
}
