using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class AttendRequest
{
	[JsonPropertyName("idclase")]
	public int ClazzId { get; set; }
}