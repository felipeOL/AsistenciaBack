using Newtonsoft.Json;

namespace AsistenciaBack.Model.Request;

public class RegisterRequest
{
	[JsonProperty("email")]
	public string Email { get; set; } = string.Empty;
	[JsonProperty("nombre")]
	public string Name { get; set; } = string.Empty;
	[JsonProperty("contraseña")]
	public string Password { get; set; } = string.Empty;
	[JsonProperty("rut")]
	public string Rut { get; set; } = string.Empty;
	[JsonProperty("rol")]
	public AccountType Role { get; set; }
}