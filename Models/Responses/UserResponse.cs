using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Response;

public class UserResponse
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;
	[JsonPropertyName("nombre")]
	public string UserName { get; set; } = string.Empty;
	[JsonPropertyName("rut")]
	public string Rut { get; set; } = string.Empty;
}