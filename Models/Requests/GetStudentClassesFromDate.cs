using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class GetStudentClassesFromDate
{
	[JsonPropertyName("fecha")]
	public DateTime Date { get; set; }
}