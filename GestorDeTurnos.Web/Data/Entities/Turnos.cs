using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace GestorDeTurnos.Web.Data.Entities;

public class Turnos
{
    // Identificadores
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid AfiliadoId { get; set; }

    // Datos del turno
    [Required]
    public DateTime FechaHoraInicio { get; set; }

    [Range(5, 480)] // entre 5 minutos y 8 horas
    public int DuracionMinutos { get; set; } = 30;

    public ModalidadTurno Modalidad { get; set; } = ModalidadTurno.Presencial;

    // Ubicación/servicio (si aplica)
    [MaxLength(100)]
    public string? SucursalId { get; set; }

    [MaxLength(100)]
    public string? ServicioCodigo { get; set; }

    [MaxLength(150)]
    public string? ProfesionalNombre { get; set; }

    [MaxLength(200)]
    public string? Motivo { get; set; }

    [MaxLength(500)]
    public string? Observaciones { get; set; }

    // Estado del turno y canal de origen
    public EstadoTurno Estado { get; set; } = EstadoTurno.Solicitado;
    public CanalOrigenTurno CanalOrigen { get; set; } = CanalOrigenTurno.Web;

    // Confirmación / check-in / cancelación
    [MaxLength(16)]
    public string? CodigoConfirmacion { get; set; }
    public DateTime? ConfirmadoEn { get; set; }

    [MaxLength(16)]
    public string? CodigoCheckIn { get; set; }
    public DateTime? CheckInEn { get; set; }

    public DateTime? CanceladoEn { get; set; }
    [MaxLength(200)]
    public string? CanceladoMotivo { get; set; }

    public DateTime? NoAsistioEn { get; set; }

    // Trazabilidad de solicitud
    [MaxLength(64)]
    public string? OrigenDispositivoId { get; set; }
    [MaxLength(45)]
    public string? IpOrigen { get; set; }
    [MaxLength(256)]
    public string? UserAgent { get; set; }

    // Auditoría
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaActualizacion { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }

    // Propiedades derivadas y helpers
    public DateTime FechaHoraFin => FechaHoraInicio.AddMinutes(DuracionMinutos);

    public bool EsCancelable(DateTime ahoraUtc) => Estado is EstadoTurno.Solicitado or EstadoTurno.Confirmado
                                                  && FechaHoraInicio > ahoraUtc;

    // Fábrica para solicitud desde canal (kiosko/tablet/web)
    public static Turnos Create(
        Guid afiliadoId,
        DateTime fechaHoraInicioUtc,
        int duracionMinutos,
        CanalOrigenTurno canal,
        string? sucursalId = null,
        string? servicioCodigo = null,
        string? profesionalNombre = null,
        string? motivo = null,
        string? origenDispositivoId = null,
        string? ipOrigen = null,
        string? userAgent = null)
    {
        if (afiliadoId == Guid.Empty) throw new ArgumentException("AfiliadoId es requerido", nameof(afiliadoId));
        if (duracionMinutos < 5 || duracionMinutos > 480) throw new ArgumentOutOfRangeException(nameof(duracionMinutos));

        var turno = new Turnos
        {
            AfiliadoId = afiliadoId,
            FechaHoraInicio = DateTime.SpecifyKind(fechaHoraInicioUtc, DateTimeKind.Utc),
            DuracionMinutos = duracionMinutos,
            CanalOrigen = canal,
            SucursalId = sucursalId,
            ServicioCodigo = servicioCodigo,
            ProfesionalNombre = profesionalNombre,
            Motivo = motivo,
            OrigenDispositivoId = origenDispositivoId,
            IpOrigen = ipOrigen,
            UserAgent = userAgent,
            Estado = EstadoTurno.Solicitado,
            CodigoConfirmacion = GenerarCodigoNumerico(6)
        };

        // Para kiosko/tabla se puede generar también un código de check-in
        if (canal is CanalOrigenTurno.Kiosko or CanalOrigenTurno.Tablet)
        {
            turno.CodigoCheckIn = GenerarCodigoNumerico(6);
        }

        return turno;
    }

    private static string GenerarCodigoNumerico(int longitud)
    {
        if (longitud <= 0 || longitud > 16) throw new ArgumentOutOfRangeException(nameof(longitud));
        var bytes = new byte[longitud];
        RandomNumberGenerator.Fill(bytes);
        var sb = new StringBuilder(longitud);
        foreach (var b in bytes)
        {
            sb.Append((char)('0' + (b % 10)));
        }
        return sb.ToString();
    }
}

public enum EstadoTurno
{
    Solicitado,
    Confirmado,
    Cancelado,
    NoAsistio,
    Atendido
}

public enum CanalOrigenTurno
{
    Kiosko,
    Tablet,
    Web,
    CallCenter,
    AppMovil,
    Interno
}

public enum ModalidadTurno
{
    Presencial,
    Virtual
}