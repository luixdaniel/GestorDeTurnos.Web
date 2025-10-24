namespace GestorDeTurnos.Web.Models
{
    public abstract class Persona(
    string nombre,
    string apellido,
    string identificacion,
    string numeroDeTelefono,
    string correo,
    string? direccion,
    DateOnly fechaDeNacimiento)
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nombre { get; set; } = nombre.ToLower().Trim();
        public string Apellido { get; set; } = apellido.ToLower().Trim();
        public string Identificacion { get; set; } = identificacion;
        public string NumeroDeTelefono { get; set; } = numeroDeTelefono;
        public string Correo { get; set; } = correo.ToLower().Trim();
        public string? Direccion { get; set; } = string.Empty ?? direccion.ToLower().Trim();
        public DateOnly FechaDeNacimiento { get; set; } = fechaDeNacimiento;
    }
}
