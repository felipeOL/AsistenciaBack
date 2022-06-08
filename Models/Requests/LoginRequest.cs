using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class LoginRequest
{
	[JsonPropertyName("email")]
	public string Email { get; set; } = string.Empty;
	[JsonPropertyName("contraseña")]
	public string Password { get; set; } = string.Empty;
}