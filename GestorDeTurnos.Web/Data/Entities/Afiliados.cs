using System.ComponentModel.DataAnnotations;
namespace GestorDeTurnos.Web.Data.Entities;

public class Afiliados
{
    // Identificación básica
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [EmailAddress, MaxLength(254)]
    public string? Email { get; set; }

    [Phone, MaxLength(20)]
    public string? Telefono { get; set; }

    // Documento en Afiliados (ya no se usa clase Personas)
    [Required, MaxLength(30)]
    public string Documento { get; set; } = string.Empty;
    public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.Cc;

    // Plan / entidad de salud (si aplica a tu dominio)
    [MaxLength(100)]
    public string? Eps { get; set; }
    
    // Auditoría y concurrencia
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaActualizacion { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}


public enum TipoDocumento
{
    Cc,
    Dni,
    Ce,
    Nit
}