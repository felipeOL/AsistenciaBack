using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Response;

public class BatchStudentResponse
{
	[JsonPropertyName("email")]
	public string Email { get; set; } = string.Empty;
	[JsonPropertyName("resultado")]
	public string Result { get; set; } = string.Empty;
	[JsonPropertyName(name: "resultadobool")]
	public bool HasCreated { get; set; }
}