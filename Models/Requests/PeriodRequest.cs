using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class PeriodRequest
{
	[JsonPropertyName("nombre")]
	public string Name { get; set; } = string.Empty;
	[JsonPropertyName("anio")]
	public int Year { get; set; }
	[JsonPropertyName("fechainicio")]
	public DateTimeOffset Start { get; set; }
	[JsonPropertyName("fechafin")]
	public DateTimeOffset End { get; set; }
}