using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class GetStudentClassesFromDate
{
	[JsonPropertyName("fecha")]
	public DateTimeOffset Date { get; set; }
}