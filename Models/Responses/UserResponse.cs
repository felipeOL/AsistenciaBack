using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Response;

public class UserResponse
{
	[JsonPropertyName("email")]
	public string Email { get; set; } = string.Empty;
	[JsonPropertyName("nombre")]
	public string UserName { get; set; } = string.Empty;
	[JsonPropertyName("rut")]
	public string Rut { get; set; } = string.Empty;
	[JsonPropertyName("rol")]
	public string Role { get; set; } = string.Empty;
}