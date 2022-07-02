using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class AttendanceRequest
{
	[JsonPropertyName("idclase")]
	public int ClazzId { get; set; }
	[JsonPropertyName("asistencias")]
	public List<AttendanceResponse> AttendanceResponses { get; set; } = new List<AttendanceResponse>();
}