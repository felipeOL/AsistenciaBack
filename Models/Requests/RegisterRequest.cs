using System.Text.Json.Serialization;

namespace AsistenciaBack.Model.Request;

public class RegisterRequest
{
	[JsonPropertyName("email")]
	public string Email { get; set; } = string.Empty;
	[JsonPropertyName("nombre")]
	public string Name { get; set; } = string.Empty;
	[JsonPropertyName("contraseña")]
	public string Password { get; set; } = string.Empty;
	[JsonPropertyName("rut")]
	public string Rut { get; set; } = string.Empty;
	[JsonPropertyName("rol")]
	public AccountType Role { get; set; }
}