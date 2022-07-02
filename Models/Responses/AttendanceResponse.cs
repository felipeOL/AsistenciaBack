using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Response;

public class AttendanceResponse
{
	[JsonPropertyName("email")]
	public string Email { get; set; } = string.Empty;
	[JsonPropertyName("asistio")]
	public bool HasAttend { get; set; }
}