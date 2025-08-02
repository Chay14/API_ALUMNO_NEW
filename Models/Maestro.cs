
namespace ApiMaestrosAlumnos.Models
{
    public class Maestro
    {
        public string Nombre { get; set; } = string.Empty;
        public List<string> Alumnos { get; set; } = new();
    }
}
